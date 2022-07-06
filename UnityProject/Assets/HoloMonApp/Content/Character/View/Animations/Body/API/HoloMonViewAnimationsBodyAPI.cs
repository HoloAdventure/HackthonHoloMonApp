using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Model.Animations.Body;

namespace HoloMonApp.Content.Character.View.Animations.Body
{
    /// <summary>
    /// アニメーションAPI
    /// </summary>
    public class HoloMonViewAnimationsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonAnimationsBodyAPI p_AnimationsAPI;

        /// <summary>
        /// アニメーションのステートマシン変化の参照用変数
        /// </summary>
        public ObservableStateMachineTrigger AnimationTrigger => p_AnimationsAPI.AnimationTrigger;
    }
}