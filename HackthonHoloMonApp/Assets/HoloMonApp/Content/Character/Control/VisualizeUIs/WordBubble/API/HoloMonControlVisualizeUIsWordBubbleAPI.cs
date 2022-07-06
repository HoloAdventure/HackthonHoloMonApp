using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;

using HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble;

namespace HoloMonApp.Content.Character.Control.VisualizeUIs.WordBubble
{
    public class HoloMonControlVisualizeUIsWordBubbleAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("APIの参照")]
        private HoloMonWordBubbleAPI p_WordBubbleAPI;

        /// <summary>
        /// 言霊到達モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> ArriveStartAsync()
        {
            return await p_WordBubbleAPI.ArriveStartAsync();
        }

        /// <summary>
        /// 言霊破裂モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> BreakStartAsync()
        {
            return await p_WordBubbleAPI.BreakStartAsync();
        }
    }
}