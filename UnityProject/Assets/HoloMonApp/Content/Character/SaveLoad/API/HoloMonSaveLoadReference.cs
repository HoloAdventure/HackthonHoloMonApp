using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.SaveLoad
{
    public class HoloMonSaveLoadReference : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのコントロールAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonControlAPI p_Control;
        public HoloMonControlAPI Control => p_Control;

        /// <summary>
        /// ホロモンのビューAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonViewAPI p_View;
        public HoloMonViewAPI View => p_View;
    }
}