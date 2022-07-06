using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Conditions.Life
{
    /// <summary>
    /// ライフコンディションのデータ
    /// </summary>
    public class HoloMonConditionLifeData : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのライフコンディション
        /// </summary>
        [SerializeField, Tooltip("ホロモンのライフコンディション")]
        private HoloMonLifeStatusReactiveProperty p_HoloMonLifeStatus
            = new HoloMonLifeStatusReactiveProperty();

        /// <summary>
        /// ホロモンのライフコンディションのReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<HoloMonLifeStatus> p_IReadOnlyReactivePropertyHoloMonLifeStatus;

        /// <summary>
        /// ホロモンのライフコンディションのReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonLifeStatus> IReadOnlyReactivePropertyHoloMonLifeStatus
            => p_IReadOnlyReactivePropertyHoloMonLifeStatus
            ?? (p_IReadOnlyReactivePropertyHoloMonLifeStatus
            = p_HoloMonLifeStatus.ToSequentialReadOnlyReactiveProperty());

        /// <summary>
        /// ホロモンのライフコンディションのデータ参照変数
        /// </summary>
        public HoloMonLifeStatus Status => p_HoloMonLifeStatus.Value;


        /// <summary>
        /// 初期化済みフラグの設定
        /// </summary>
        public bool ReceptionInitialized()
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonLifeStatus status = p_HoloMonLifeStatus.Value;

            // 状態を設定する
            status.Initialized = true;

            // 状態を更新する
            p_HoloMonLifeStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// 強制通知を行う
        /// </summary>
        public void ForcedNotify()
        {
            // 通知を行う
            p_HoloMonLifeStatus.SetValueAndForceNotify(p_HoloMonLifeStatus.Value);
        }

        /// <summary>
        /// 空腹度の変化
        /// </summary>
        /// <param name="a_HungryPercent"></param>
        public bool ReceptionHungryPercent(int a_HungryPercent)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonLifeStatus status = p_HoloMonLifeStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.HungryPercent.Value == a_HungryPercent) return false;

            // 状態を設定する
            status.HungryPercent.UpdateValue(a_HungryPercent);

            // 状態を更新する
            p_HoloMonLifeStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// 機嫌度の変化
        /// </summary>
        /// <param name="a_HumorPercent"></param>
        public bool ReceptionHumorPercent(int a_HumorPercent)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonLifeStatus status = p_HoloMonLifeStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.HumorPercent.Value == a_HumorPercent) return false;

            // 状態を設定する
            status.HumorPercent.UpdateValue(a_HumorPercent);

            // 状態を更新する
            p_HoloMonLifeStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// 元気度の変化
        /// </summary>
        /// <param name="a_StaminaPercent"></param>
        public bool ReceptionStaminaPercent(int a_StaminaPercent)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonLifeStatus status = p_HoloMonLifeStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.StaminaPercent.Value == a_StaminaPercent) return false;

            // 状態を設定する
            status.StaminaPercent.UpdateValue(a_StaminaPercent);

            // 状態を更新する
            p_HoloMonLifeStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// うんち度の変化
        /// </summary>
        /// <param name="a_PoopPercent"></param>
        public bool ReceptionPoopPercent(int a_PoopPercent)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonLifeStatus status = p_HoloMonLifeStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.PoopPercent.Value == a_PoopPercent) return false;

            // 状態を設定する
            status.PoopPercent.UpdateValue(a_PoopPercent);

            // 状態を更新する
            p_HoloMonLifeStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// 眠さの変化
        /// </summary>
        /// <param name="a_Sleepiness"></param>
        public bool ReceptionSleepiness(HoloMonSleepinessLevel a_Sleepiness)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            HoloMonLifeStatus status = p_HoloMonLifeStatus.Value;

            // 状態の変化が発生しない場合は処理しない
            if (status.SleepinessLevel.Value == a_Sleepiness) return false;

            // 状態を設定する
            status.SleepinessLevel.UpdateValue(a_Sleepiness);

            // 状態を更新する
            p_HoloMonLifeStatus.SetValueAndForceNotify(status);

            return true;
        }

        /// <summary>
        /// ライフコンディションデータの反映
        /// </summary>
        public void ApplyLifeStatus(HoloMonLifeStatus a_LifeStatus)
        {
            p_HoloMonLifeStatus.Value = a_LifeStatus;
        }
    }
}