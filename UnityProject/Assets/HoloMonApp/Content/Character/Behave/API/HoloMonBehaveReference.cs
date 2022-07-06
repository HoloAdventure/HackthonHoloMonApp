using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Behave
{
    public class HoloMonBehaveReference : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのコントロールAPIの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンのコントロールAPIの参照")]
        private HoloMonControlAPI p_Control;
        public HoloMonControlAPI Control => p_Control;

        /// <summary>
        /// ホロモンのビューAPIの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンのビューAPIの参照")]
        private HoloMonViewAPI p_View;
        public HoloMonViewAPI View => p_View;
    }
}