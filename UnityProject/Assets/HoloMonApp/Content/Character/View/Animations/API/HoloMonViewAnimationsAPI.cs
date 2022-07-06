using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.View.Animations.Body;

namespace HoloMonApp.Content.Character.View.Animations
{
    /// <summary>
    /// アニメーションAPI
    /// </summary>
    public class HoloMonViewAnimationsAPI : MonoBehaviour
    {
        /// <summary>
        /// ボディアニメーションの参照
        /// </summary>
        [SerializeField, Tooltip("ボディアニメーションの参照")]
        private HoloMonViewAnimationsBodyAPI p_Body;
        public HoloMonViewAnimationsBodyAPI Body => p_Body;
    }
}