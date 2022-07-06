using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Conditions;
using HoloMonApp.Content.Character.Data.Conditions.Life;

namespace HoloMonApp.Content.Character.Model.Parameters.Conditions.Life
{
    /// <summary>
    /// ライフコンディションのAPI
    /// </summary>
    public class HoloMonConditionsLifeAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのライフコンディションデータの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンのボディコンディションデータの参照")]
        private HoloMonConditionLifeData p_HoloMonConditionLifeData;

        /// <summary>
        /// ホロモンのライフコンディションのReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonLifeStatus> ReactivePropertyStatus
            => p_HoloMonConditionLifeData.IReadOnlyReactivePropertyHoloMonLifeStatus;

        /// <summary>
        /// ホロモンの空腹度の短縮参照用変数
        /// </summary>
        public int HungryPercent => p_HoloMonConditionLifeData.Status.HungryPercent.Value;
        private DateTime HungryPercentLastUpdate => p_HoloMonConditionLifeData.Status.HungryPercent.LastUpdateDateTime;

        /// <summary>
        /// ホロモンの機嫌度の短縮参照用変数
        /// </summary>
        public int HumorPercent => p_HoloMonConditionLifeData.Status.HumorPercent.Value;
        private DateTime HumorPercentLastUpdate => p_HoloMonConditionLifeData.Status.HumorPercent.LastUpdateDateTime;

        /// <summary>
        /// ホロモンの元気度の短縮参照用変数
        /// </summary>
        public int StaminaPercent => p_HoloMonConditionLifeData.Status.StaminaPercent.Value;
        private DateTime StaminaPercentLastUpdate => p_HoloMonConditionLifeData.Status.StaminaPercent.LastUpdateDateTime;

        /// <summary>
        /// ホロモンのうんち度の短縮参照用変数
        /// </summary>
        public int PoopPercent => p_HoloMonConditionLifeData.Status.PoopPercent.Value;
        private DateTime PoopPercentLastUpdate => p_HoloMonConditionLifeData.Status.PoopPercent.LastUpdateDateTime;

        /// <summary>
        /// ホロモンの眠気度の短縮参照用変数
        /// </summary>
        public HoloMonSleepinessLevel SleepinessLevel => p_HoloMonConditionLifeData.Status.SleepinessLevel.Value;
        private DateTime SleepinessLevelLastUpdate => p_HoloMonConditionLifeData.Status.SleepinessLevel.LastUpdateDateTime;


        /// <summary>
        /// ホロモンの空腹設定
        /// </summary>
        [SerializeField, Tooltip("ホロモンの空腹設定")]
        private HoloMonHungrySetting p_HoloMonHungrySetting = new HoloMonHungrySetting();

        /// <summary>
        /// ホロモンの機嫌設定
        /// </summary>
        [SerializeField, Tooltip("ホロモンの機嫌設定")]
        private HoloMonHumorSetting p_HoloMonHumorSetting = new HoloMonHumorSetting();

        /// <summary>
        /// ホロモンの元気設定
        /// </summary>
        [SerializeField, Tooltip("ホロモンの元気設定")]
        private HoloMonStaminaSetting p_HoloMonStaminaSetting = new HoloMonStaminaSetting();

        /// <summary>
        /// ホロモンのうんち設定
        /// </summary>
        [SerializeField, Tooltip("ホロモンのうんち設定")]
        private HoloMonShitSetting p_HoloMonShitSetting = new HoloMonShitSetting();

        /// <summary>
        /// ホロモンの睡眠設定
        /// </summary>
        [SerializeField, Tooltip("ホロモンの睡眠設定")]
        private HoloMonSleepSetting p_HoloMonSleepSetting = new HoloMonSleepSetting();


        /// <summary>
        /// 時刻判定(分刻み)のトリガー
        /// </summary>
        private IDisposable p_MinuteTimeTrigger;

        /// <summary>
        /// 経過時間の計算インスタンス
        /// </summary>
        private CalculationActivityTime p_CalculationActivityTime;


        /// <summary>
        /// 空腹度を設定する
        /// </summary>
        /// <param name="a_Value"></param>
        public bool SetHungry(int a_Value)
        {
            return p_HoloMonConditionLifeData.ReceptionHungryPercent(a_Value);
        }

        /// <summary>
        /// 機嫌度を設定する
        /// </summary>
        /// <param name="a_Value"></param>
        public bool SetHumor(int a_Value)
        {
            return p_HoloMonConditionLifeData.ReceptionHumorPercent(a_Value);
        }

        /// <summary>
        /// 元気度を設定する
        /// </summary>
        /// <param name="a_Value"></param>
        public bool SetStamina(int a_Value)
        {
            return p_HoloMonConditionLifeData.ReceptionStaminaPercent(a_Value);
        }

        /// <summary>
        /// うんち度を設定する
        /// </summary>
        /// <param name="a_Value"></param>
        public bool SetPoop(int a_Value)
        {
            return p_HoloMonConditionLifeData.ReceptionPoopPercent(a_Value);
        }

        /// <summary>
        /// ライフコンディションデータの反映
        /// </summary>
        public void ApplyLifeStatus(HoloMonLifeStatus a_LifeStatus)
        {
            p_HoloMonConditionLifeData.ApplyLifeStatus(a_LifeStatus);
        }


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // コンディションが初期化済みか否か
            if (!p_HoloMonConditionLifeData.Status.Initialized)
            {
                // ライフコンディションが初期化されていない場合は初期化する
                InitializeLifeStatus();
            }

            // 睡眠時間を計算ロジックに設定する
            p_CalculationActivityTime = new CalculationActivityTime(p_HoloMonSleepSetting.StartTime, p_HoloMonSleepSetting.EndTime);

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
            p_HoloMonConditionLifeData.ForcedNotify();
        }

        /// <summary>
        /// ライフコンディションを初期化する
        /// </summary>
        private void InitializeLifeStatus()
        {
            // ライフコンディションを初期化する
            p_HoloMonConditionLifeData.ReceptionHungryPercent(50);
            p_HoloMonConditionLifeData.ReceptionHumorPercent(50);
            p_HoloMonConditionLifeData.ReceptionStaminaPercent(100);
            p_HoloMonConditionLifeData.ReceptionPoopPercent(99);
            p_HoloMonConditionLifeData.ReceptionSleepiness(HoloMonSleepinessLevel.Nothing);

            // 初期化済みフラグを設定する
            p_HoloMonConditionLifeData.ReceptionInitialized();
        }

        /// <summary>
        /// 時間経過によるコンディション変化を管理する
        /// </summary>
        /// <param name="a_DateTime"></param>
        private void ChangeOverTimeCondition(DateTime a_DateTime)
        {
            Debug.Log("ChangeOverTimeCondition : " + a_DateTime.ToString());

            // 空腹度をチェックする
            CheckHungry(a_DateTime);

            // 機嫌度をチェックする
            CheckHumor(a_DateTime);

            // 元気度をチェックする
            CheckStamina(a_DateTime);

            // 睡眠時刻をチェックする
            CheckSleepTime(a_DateTime);
        }



        /// <summary>
        /// 空腹度の変化をチェックする
        /// </summary>
        /// <param name="a_DataTime"></param>
        private void CheckHungry(DateTime a_DataTime)
        {
            // 睡眠時間中は空腹度は変化しない
            if (p_CalculationActivityTime.IsSleepTime(a_DataTime)) return;

            // 前回の空腹度の変化時刻を取得する
            DateTime changeTime = HungryPercentLastUpdate;

            // 前回の空腹度変化からのアクティブな経過時間(分)を取得する
            int elapsedTotalMinutes = (int)Math.Round(
                p_CalculationActivityTime.AwakeElapsedTimeSpan(changeTime, a_DataTime).TotalMinutes
                );

            // 減少間隔から減少係数を計算する
            int decreseCoefficient = elapsedTotalMinutes / p_HoloMonHungrySetting.DecreaseMarginMinute;

            // 減少値を計算する
            int totalDecreasePoint = decreseCoefficient * p_HoloMonHungrySetting.DecreasePoint;

            // 結果の空腹度を減算する
            int resultHungryPercent = HungryPercent - totalDecreasePoint;

            // 空腹度は 0 未満にならない
            if (resultHungryPercent < 0) resultHungryPercent = 0;

            // 空腹度の減少値を計算する
            int actualDecrease = HungryPercent - resultHungryPercent;

            // 空腹度の自然減少値に合わせてうんち度を増加させる
            int resultShitPercent = PoopPercent + (actualDecrease * p_HoloMonShitSetting.IncreaseFactor);

            // 計算結果の空腹度を設定する
            p_HoloMonConditionLifeData.ReceptionHungryPercent(resultHungryPercent);

            // 計算結果のうんち度を設定する
            p_HoloMonConditionLifeData.ReceptionPoopPercent(resultShitPercent);
        }

        /// <summary>
        /// 機嫌度の変化をチェックする
        /// </summary>
        /// <param name="a_DataTime"></param>
        private void CheckHumor(DateTime a_DataTime)
        {
            // 睡眠時間中は空腹度は変化しない
            if (p_CalculationActivityTime.IsSleepTime(a_DataTime)) return;

            // 前回の機嫌度の変化時刻を取得する
            DateTime changeTime = HumorPercentLastUpdate;

            // 前回の機嫌度変化からのアクティブな経過時間(分)を取得する
            int elapsedTotalMinutes = (int)Math.Round(
                p_CalculationActivityTime.AwakeElapsedTimeSpan(changeTime, a_DataTime).TotalMinutes
                ); ;

            // 減少間隔から減少係数を計算する
            int decreseCoefficient = elapsedTotalMinutes / p_HoloMonHumorSetting.DecreaseMarginMinute;

            // 減少値を計算する
            int totalDecreasePoint = decreseCoefficient * p_HoloMonHumorSetting.DecreasePoint;

            // 機嫌度を減算する
            int resultHumorPercent = HumorPercent - totalDecreasePoint;

            // 機嫌度は 0 未満にならない
            if (resultHumorPercent < 0) resultHumorPercent = 0;

            // 計算結果の機嫌度を設定する
            p_HoloMonConditionLifeData.ReceptionHumorPercent(resultHumorPercent);
        }

        /// <summary>
        /// 元気度の変化をチェックする
        /// </summary>
        /// <param name="a_DataTime"></param>
        private void CheckStamina(DateTime a_DataTime)
        {
            // 睡眠時間中は空腹度は変化しない
            if (p_CalculationActivityTime.IsSleepTime(a_DataTime)) return;

            // 前回の元気度の変化時刻を取得する
            DateTime changeTime = StaminaPercentLastUpdate;

            // 前回の元気度変化からの全ての経過時間(分)を取得する
            int elapsedTotalMinutes = (int)Math.Round(
                p_CalculationActivityTime.AllElapsedTimeSpan(changeTime, a_DataTime).TotalMinutes
                );

            // 増加間隔から増加係数を計算する
            int increseCoefficient = elapsedTotalMinutes / p_HoloMonStaminaSetting.IncreaseMarginMinute;

            // 増加値を計算する
            int totalIncreasePoint = increseCoefficient * p_HoloMonStaminaSetting.IncreasePoint;

            // 元気度を加算する
            int resultStaminaPercent = StaminaPercent + totalIncreasePoint;

            // 元気度は自然回復では 100 を超えない
            if (resultStaminaPercent > 100) resultStaminaPercent = 100;

            // 計算結果の元気度を設定する
            p_HoloMonConditionLifeData.ReceptionStaminaPercent(resultStaminaPercent);
        }

        /// <summary>
        /// 睡眠時刻をチェックする
        /// </summary>
        /// <param name="a_DataTime"></param>
        private void CheckSleepTime(DateTime a_DataTime)
        {
            if(p_CalculationActivityTime.IsSleepTime(a_DataTime))
            {
                // 範囲内なら眠さ度の変更を行う
                p_HoloMonConditionLifeData.ReceptionSleepiness(HoloMonSleepinessLevel.Sleepy);
            }
            else
            {
                // 範囲外なら眠さ度の変更を行う
                p_HoloMonConditionLifeData.ReceptionSleepiness(HoloMonSleepinessLevel.Nothing);
            }
        }
    }
}