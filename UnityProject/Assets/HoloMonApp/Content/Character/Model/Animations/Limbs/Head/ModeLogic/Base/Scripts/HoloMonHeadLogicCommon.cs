using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    [Serializable]
    public class HoloMonHeadLogicCommon
    {
        /// <summary>
        /// 頭部ロジック状態
        /// </summary>
        [SerializeField, Tooltip("頭部ロジック状態")]
        private HoloMonHeadStatusReactiveProperty p_HeadLogicStatus
            = new HoloMonHeadStatusReactiveProperty(HoloMonActionHeadStatus.Nothing);

        /// <summary>
        /// 視線ロジック状態の参照変数
        /// </summary>
        public HoloMonActionHeadStatus HeadLogicStatus => p_HeadLogicStatus.Value;

        // EveryValueChanged を使ってフレーム間で変化があった時のみ通知する
        /// <summary>
        /// 頭部ロジック状態の EveryValueChanged オブザーバ保持変数
        /// </summary>
        private IObservable<HoloMonActionHeadStatus> p_IObservableHeadLogicStatusEveryValueChanged;

        /// <summary>
        /// 頭部ロジック状態の EveryValueChanged オブザーバ参照変数
        /// </summary>
        public IObservable<HoloMonActionHeadStatus> IObservableHeadLogicStatusEveryValueChanged
            => p_IObservableHeadLogicStatusEveryValueChanged
            ?? (p_IObservableHeadLogicStatusEveryValueChanged =
            p_HeadLogicStatus.ObserveEveryValueChanged(x => x.Value));


        /// <summary>
        /// 頭部ロジックの状態を設定する
        /// </summary>
        /// <returns></returns>
        public void SetHeadStatus(HoloMonActionHeadStatus a_HoloMonHeadStatus)
        {
            p_HeadLogicStatus.SetValueAndForceNotify(a_HoloMonHeadStatus);
        }
    }
}