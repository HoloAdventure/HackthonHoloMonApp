using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Control.Animations.Limbs.Eye;
using HoloMonApp.Content.Character.Control.Animations.Limbs.Head;
using HoloMonApp.Content.Character.Control.Animations.Limbs.Tail;

namespace HoloMonApp.Content.Character.Control.Animations.Limbs
{
    /// <summary>
    /// 四肢コントロールAPI
    /// </summary>
    public class HoloMonControlAnimationsLimbsAPI : MonoBehaviour
    {
        /// <summary>
        /// 瞳コントロールAPIの参照
        /// </summary>
        [SerializeField, Tooltip("瞳コントロールAPIの参照")]
        private HoloMonControlAnimationsLimbsEyeAPI p_Eye;
        public HoloMonControlAnimationsLimbsEyeAPI Eye => p_Eye;

        /// <summary>
        /// 頭部コントロールAPIの参照
        /// </summary>
        [SerializeField, Tooltip("頭部コントロールAPIの参照")]
        private HoloMonControlAnimationsLimbsHeadAPI p_Head;
        public HoloMonControlAnimationsLimbsHeadAPI Head => p_Head;

        /// <summary>
        /// 尻尾コントロールAPIの参照
        /// </summary>
        [SerializeField, Tooltip("尻尾コントロールAPIの参照")]
        private HoloMonControlAnimationsLimbsTailAPI p_Tail;
        public HoloMonControlAnimationsLimbsTailAPI Tail => p_Tail;
    }
}