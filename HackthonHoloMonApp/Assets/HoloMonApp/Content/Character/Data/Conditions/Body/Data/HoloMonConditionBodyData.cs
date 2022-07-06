using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Conditions.Body
{
    /// <summary>
    /// ボディコンディションのデータ
    /// </summary>
    public class HoloMonConditionBodyData : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのボディコンディション
        /// </summary>
        [SerializeField, Tooltip("ホロモンのボディコンディション")]
        private HoloMonBodyStatusReactiveProperty p_HoloMonBodyStatus
            = new HoloMonBodyStatusReactiveProperty();

        /// <summary>
        /// ホロモンのボディコンディションのReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<HoloMonBodyStatus> p_IReadOnlyReactivePropertyHoloMonBodyStatus;

        /// <summary>
        /// ホロモンのボディコンディションのReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonBodyStatus> IReadOnlyReactivePropertyHoloMonBodyStatus
            => p_IReadOnlyReactivePropertyHoloMonBodyStatus
            ?? (p_IReadOnlyReactivePropertyHoloMonBodyStatus
            = p_HoloMonBodyStatus.ToSequentialReadOnlyReactiveProperty());

        /// <summary>
        /// ホロモンのボディコンディションのデータ参照変数
        /// </summary>
        public HoloMonBodyStatus Status => p_HoloMonBodyStatus.Value;


        /// <summary>
        /// 初期化済みフラグの設定
        /// </summary>
        public bool ReceptionInitialized()
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonBodyStatus status = p_HoloMonBodyStatus.Value;

            // 状態を設定する
            status.Initialized = true;

            // 状態を更新する
            p_HoloMonBodyStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// 強制通知を行う
        /// </summary>
        public void ForcedNotify()
        {
            // 通知を行う
            p_HoloMonBodyStatus.SetValueAndForceNotify(p_HoloMonBodyStatus.Value);
        }

        /// <summary>
        /// 誕生日時の設定
        /// </summary>
        /// <param name="a_BirthTime"></param>
        public bool ReceptionBirthTime(DateTime a_BirthTime)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonBodyStatus status = p_HoloMonBodyStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.BirthTime.Value == a_BirthTime) return false;

            // 状態を設定する
            status.BirthTime.UpdateValue(a_BirthTime);

            // 状態を更新する
            p_HoloMonBodyStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// 日齢の変化
        /// </summary>
        /// <param name="a_AgeOfDay"></param>
        public bool ReceptionAgeOfDay(int a_AgeOfDay)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonBodyStatus status = p_HoloMonBodyStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.AgeOfDay.Value == a_AgeOfDay) return false;

            // 状態を設定する
            status.AgeOfDay.UpdateValue(a_AgeOfDay);

            // 状態を更新する
            p_HoloMonBodyStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// 大きさの変化
        /// </summary>
        /// <param name="a_BodyHeight"></param>
        public bool ReceptionBodyHeight(float a_BodyHeight)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonBodyStatus status = p_HoloMonBodyStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.BodyHeight.Value == a_BodyHeight) return false;

            // 状態を設定する
            status.BodyHeight.UpdateValue(a_BodyHeight);

            // 状態を更新する
            p_HoloMonBodyStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// デフォルトの大きさの変化
        /// </summary>
        /// <param name="a_DefaultHeight"></param>
        public bool ReceptionDefaultHeight(float a_DefaultHeight)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonBodyStatus status = p_HoloMonBodyStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.DefaultBodyHeight.Value == a_DefaultHeight) return false;

            // 状態を設定する
            status.DefaultBodyHeight.UpdateValue(a_DefaultHeight);

            // 状態を更新する
            p_HoloMonBodyStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// ちからの変化
        /// </summary>
        /// <param name="a_BodyPower"></param>
        public bool ReceptionBodyPower(float a_BodyPower)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonBodyStatus status = p_HoloMonBodyStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.BodyPower.Value == a_BodyPower) return false;

            // 状態を設定する
            status.BodyPower.UpdateValue(a_BodyPower);

            // 状態を更新する
            p_HoloMonBodyStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// ボディコンディションデータの反映
        /// </summary>
        public void ApplyBodyStatus(HoloMonBodyStatus a_BodyStatus)
        {
            p_HoloMonBodyStatus.Value = a_BodyStatus;
        }
    }
}