using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble
{
    public class HoloMonWordBubbleAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("非同期操作コントローラの参照")]
        private WordBubbleAsyncController p_WordBubbleAsyncController;

        /// <summary>
        /// 言霊到達モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> ArriveStartAsync()
        {
            bool result = await p_WordBubbleAsyncController.ArriveStartAsync();
            return result;
        }

        /// <summary>
        /// 言霊破裂モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> BreakStartAsync()
        {
            bool result = await p_WordBubbleAsyncController.BreakStartAsync();
            return result;
        }

        #region "TestMethod"
        /// <summary>
        /// 言霊到達表示開始(試験用)
        /// </summary>
        public async void ArriveStartTest()
        {
            bool result = await ArriveStartAsync();
            Debug.Log("ArriveStartTest : " + result.ToString());
        }

        /// <summary>
        /// 言霊破裂表示開始(試験用)
        /// </summary>
        public async void BreakStartTest()
        {
            bool result = await BreakStartAsync();
            Debug.Log("BreakStartTest : " + result.ToString());
        }
        #endregion
    }
}