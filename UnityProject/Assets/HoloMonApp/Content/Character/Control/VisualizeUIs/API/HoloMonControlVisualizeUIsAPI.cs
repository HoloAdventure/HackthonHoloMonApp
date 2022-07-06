using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

using HoloMonApp.Content.Character.Control.VisualizeUIs.EmotionPanel;
using HoloMonApp.Content.Character.Control.VisualizeUIs.WordBubble;

namespace HoloMonApp.Content.Character.Control.VisualizeUIs
{
    /// <summary>
    /// 可視化UIの参照
    /// </summary>
    public class HoloMonControlVisualizeUIsAPI : MonoBehaviour
    {
        /// <summary>
        /// 感情UIコントローラの参照
        /// </summary>
        [SerializeField, Tooltip("感情UIコントローラの参照")]
        private HoloMonControlVisualizeUIsEmotionPanelAPI p_EmotionPanel;
        public HoloMonControlVisualizeUIsEmotionPanelAPI EmotionPanel => p_EmotionPanel;

        /// <summary>
        /// 言霊UIコントローラの参照
        /// </summary>
        [SerializeField, Tooltip("言霊UIコントローラの参照")]
        private HoloMonControlVisualizeUIsWordBubbleAPI p_WordBubble;
        public HoloMonControlVisualizeUIsWordBubbleAPI WordBubble => p_WordBubble;
    }
}