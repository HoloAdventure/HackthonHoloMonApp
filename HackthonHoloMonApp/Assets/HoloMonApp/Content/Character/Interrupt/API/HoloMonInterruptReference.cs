using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.AI;
using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.SaveLoad;

namespace HoloMonApp.Content.Character.Interrupt
{
    public class HoloMonInterruptReference : MonoBehaviour
    {
        /// <summary>
        /// ホロモンAIの参照
        /// </summary>
        [SerializeField]
        private HoloMonAICenterAPI p_AI;
        public HoloMonAICenterAPI AI => p_AI;

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

        /// <summary>
        /// ホロモンのセーブロードAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonSaveLoadAPI p_SaveLoad;
        public HoloMonSaveLoadAPI SaveLoad => p_SaveLoad;
    }
}