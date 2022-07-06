using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Data.Animations.Body;

namespace HoloMonApp.Content.Character.Model.Animations.Body
{
    /// <summary>
    /// アニメーションAPI
    /// </summary>
    public class HoloMonAnimationsHandler : MonoBehaviour
    {
        /// <summary>
        /// ホロモンアニメーションデータ
        /// </summary>
        [SerializeField]
        private HoloMonAnimationsBodyData p_AnimationsBodyData;

        /// <summary>
        /// ステートマシンの変化トリガー
        /// </summary>
        [Tooltip("ステートマシンの変化トリガー")]
        private ObservableStateMachineTrigger p_ObservableStateMachineTrigger;

        /// <summary>
        /// ステートマシンの変化トリガー参照用
        /// </summary>
        public ObservableStateMachineTrigger AnimationTrigger => p_ObservableStateMachineTrigger
            ?? (p_ObservableStateMachineTrigger = p_AnimationsBodyData.Body.GetBehaviour<ObservableStateMachineTrigger>());
    }
}