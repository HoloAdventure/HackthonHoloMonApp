using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.AI
{
    public class HoloMonAICenterReference : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのビューAPIの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンのビューAPIの参照")]
        private HoloMonViewAPI p_View;
        public HoloMonViewAPI View => p_View;
    }
}