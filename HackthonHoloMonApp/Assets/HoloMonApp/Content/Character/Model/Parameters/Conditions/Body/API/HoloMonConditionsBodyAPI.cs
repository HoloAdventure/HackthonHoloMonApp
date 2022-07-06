using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Conditions.Body;
using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.Parameters.Conditions.Body
{
    /// <summary>
    /// ボディコンディションのAPI
    /// </summary>
    public class HoloMonConditionsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのボディコンディションデータの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンのボディコンディションデータの参照")]
        private HoloMonConditionBodyData p_HoloMonConditionBodyData;

        /// <summary>
        /// ホロモンのボディコンディションのReadOnlyReactivePropertyの参照用変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonBodyStatus> ReactivePropertyStatus
            => p_HoloMonConditionBodyData.IReadOnlyReactivePropertyHoloMonBodyStatus;

        /// <summary>
        /// データの参照
        /// </summary>
        [SerializeField]
        private HoloMonTransformsBodyData p_BodyTransformsDATA;



        /// <summary>
        /// ホロモンの誕生日時の短縮参照用変数
        /// </summary>
        public DateTime BirthTime => p_HoloMonConditionBodyData.Status.BirthTime.Value;

        /// <summary>
        /// ホロモンの日齢の短縮参照用変数
        /// </summary>
        public int AgeOfDay => p_HoloMonConditionBodyData.Status.AgeOfDay.Value;

        /// <summary>
        /// ホロモンの大きさの短縮参照用変数
        /// </summary>
        public float BodyHeight => p_HoloMonConditionBodyData.Status.BodyHeight.Value;

        /// <summary>
        /// ホロモンの初期の大きさの短縮参照用変数
        /// </summary>
        public float DefaultBodyHeight => p_HoloMonConditionBodyData.Status.DefaultBodyHeight.Value;

        /// <summary>
        /// ホロモンのちからの短縮参照用変数
        /// </summary>
        public float BodyPower => p_HoloMonConditionBodyData.Status.BodyPower.Value;

        /// <summary>
        /// ホロモンのデフォルトサイズ比率の短縮参照用変数
        /// </summary>
        public float DefaultBodyHeightRatio => p_HoloMonConditionBodyData.Status.GetDefaultBodyHeightRatio;


        /// <summary>
        /// 時刻判定(分刻み)のトリガー
        /// </summary>
        private IDisposable p_MinuteTimeTrigger;


        /// <summary>
        /// 身長を設定する
        /// </summary>
        /// <param name="a_Height"></param>
        public bool SetHeight(float a_Height)
        {
            // 値を小数点第三位で四捨五入する
            a_Height = RoundFloat(a_Height);

            // 身長を設定する
            return p_HoloMonConditionBodyData.ReceptionBodyHeight(a_Height);
        }

        /// <summary>
        /// ちからを設定する
        /// </summary>
        /// <param name="a_Power"></param>
        public bool SetPower(float a_Power)
        {
            // 値を小数点第三位で四捨五入する
            a_Power = RoundFloat(a_Power);

            // ちからを設定する
            return p_HoloMonConditionBodyData.ReceptionBodyPower(a_Power);
        }

        /// <summary>
        /// ボディコンディションデータの反映
        /// </summary>
        public void ApplyBodyStatus(HoloMonBodyStatus a_BodyStatus)
        {
            p_HoloMonConditionBodyData.ApplyBodyStatus(a_BodyStatus);
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // コンディションが初期化済みか否か
            if (!p_HoloMonConditionBodyData.Status.Initialized)
            {
                // ボディコンディションが初期化されていない場合は初期化する
                InitializeBodyStatus();
            }

            // 1分毎にトリガーを実行する
            p_MinuteTimeTrigger = Observable
                .Timer(TimeSpan.FromSeconds(60.0f - DateTime.Now.Second), TimeSpan.FromMinutes(1.0f))
                .SubscribeOnMainThread()
                .Subscribe(x => {
                    ChangeOverTimeCondition(DateTime.Now);
                })
                .AddTo(this);

            // 起動直後の初回に一度チェックを行う
            StartCoroutine("FirstChangeOverTimeCondition");
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
        }

        /// <summary>
        /// 初回コンディションチェック用呼び出し
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        IEnumerator FirstChangeOverTimeCondition()
        {
            // 他の初期化が完了するまで1フレームのみ待機する
            yield return null;

            // 時間経過によるコンディション変化のチェックを行う
            ChangeOverTimeCondition(DateTime.Now);

            // 変化の有無に関わらず強制通知を行う
            p_HoloMonConditionBodyData.ForcedNotify();
        }

        /// <summary>
        /// ボディコンディションを初期化する
        /// </summary>
        private void InitializeBodyStatus()
        {
            // 誕生日時を現在とする
            p_HoloMonConditionBodyData.ReceptionBirthTime(DateTime.Now);
            // 日齢を0とする
            p_HoloMonConditionBodyData.ReceptionAgeOfDay(0);
            // デフォルトの大きさを身長値に設定する
            p_HoloMonConditionBodyData.ReceptionBodyHeight(CalculateHeightScale());
            p_HoloMonConditionBodyData.ReceptionDefaultHeight(CalculateHeightScale());
            // デフォルトの強さを設定する
            p_HoloMonConditionBodyData.ReceptionBodyPower(0.0f);

            // 初期化済みフラグを設定する
            p_HoloMonConditionBodyData.ReceptionInitialized();
        }

        /// <summary>
        /// 時間経過によるコンディション変化を管理する
        /// </summary>
        /// <param name="a_DateTime"></param>
        private void ChangeOverTimeCondition(DateTime a_DateTime)
        {
            Debug.Log("ChangeOverTimeCondition : " + a_DateTime.ToString());

            // 日齢をチェックする
            CheckAgeOfDay(a_DateTime);
        }


        /// <summary>
        /// 日齢の変化をチェックする
        /// </summary>
        /// <param name="a_DataTime"></param>
        private void CheckAgeOfDay(DateTime a_DataTime)
        {
            // 現在の日齢を取得する
            int CurrentAgeOfDay = ActivityElapsedDays(BirthTime, a_DataTime);

            // 現在設定中の日齢と異なるか否か
            if (CurrentAgeOfDay != AgeOfDay)
            {
                // 異なる場合は日齢を設定する
                p_HoloMonConditionBodyData.ReceptionAgeOfDay(CurrentAgeOfDay);
            }
        }

        /// <summary>
        /// 指定時刻から指定時刻までの経過日数を返す
        /// </summary>
        /// <param name="a_StartTime"></param>
        /// <param name="a_EndTime"></param>
        /// <returns></returns>
        private int ActivityElapsedDays(DateTime a_StartTime, DateTime a_EndTime)
        {
            // 経過時刻を計算する
            TimeSpan SubtractionTime = a_EndTime - a_StartTime;
            
            // 日数を取得する
            int elapsedTotalDays = SubtractionTime.Days;

            return elapsedTotalDays;
        }

        /// <summary>
        /// 値を小数点第三位で四捨五入する
        /// </summary>
        /// <param name="a_Value"></param>
        /// <returns></returns>
        private float RoundFloat(float a_Value)
        {
            return (float)Math.Round(a_Value, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 現在のホロモンの身長を計算する
        /// </summary>
        /// <returns></returns>
        private float CalculateHeightScale()
        {
            // ワールド基準のY軸スケール値を身長として扱う
            return p_BodyTransformsDATA.Root.lossyScale.y;
        }
    }
}