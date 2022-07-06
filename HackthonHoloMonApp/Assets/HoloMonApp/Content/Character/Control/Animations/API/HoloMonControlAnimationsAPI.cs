using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Control.Animations.Body;
using HoloMonApp.Content.Character.Control.Animations.Limbs;

namespace HoloMonApp.Content.Character.Control.Animations
{
    /// <summary>
    /// アニメーションAPI
    /// </summary>
    public class HoloMonControlAnimationsAPI : MonoBehaviour
    {
        /// <summary>
        /// ボディアニメーションの参照
        /// </summary>
        [SerializeField, Tooltip("ボディアニメーションの参照")]
        private HoloMonControlAnimationsBodyAPI p_Body;
        public HoloMonControlAnimationsBodyAPI Body => p_Body;

        /// <summary>
        /// 四肢アニメーションの参照
        /// </summary>
        [SerializeField, Tooltip("四肢アニメーションの参照")]
        private HoloMonControlAnimationsLimbsAPI p_Limbs;
        public HoloMonControlAnimationsLimbsAPI Limbs => p_Limbs;
    }
}