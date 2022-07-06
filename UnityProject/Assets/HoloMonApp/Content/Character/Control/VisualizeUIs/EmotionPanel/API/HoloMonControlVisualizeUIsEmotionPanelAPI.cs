using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

using HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel;

namespace HoloMonApp.Content.Character.Control.VisualizeUIs.EmotionPanel
{
    public class HoloMonControlVisualizeUIsEmotionPanelAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("APIの参照")]
        private HoloMonEmotionPanelAPI p_EmotionPanelAPI;

        /// <summary>
        /// びっくりモーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> ExclamationStartAsync()
        {
            return await p_EmotionPanelAPI.ExclamationStartAsync();
        }

        /// <summary>
        /// はてなモーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> QuestionStartAsync()
        {
            return await p_EmotionPanelAPI.QuestionStartAsync();
        }

        /// <summary>
        /// 無視モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> IgnoredStartAsync()
        {
            return await p_EmotionPanelAPI.IgnoredStartAsync();
        }
    }
}