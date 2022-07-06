using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel
{
    public class HoloMonEmotionPanelAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("非同期操作コントローラの参照")]
        private EmotionPanelAsyncController p_EmotionPanelAsyncController;

        /// <summary>
        /// びっくりモーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> ExclamationStartAsync()
        {
            bool result = await p_EmotionPanelAsyncController.ExclamationStartAsync();
            return result;
        }

        /// <summary>
        /// はてなモーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> QuestionStartAsync()
        {
            bool result = await p_EmotionPanelAsyncController.QuestionStartAsync();
            return result;
        }

        /// <summary>
        /// 無視モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> IgnoredStartAsync()
        {
            bool result = await p_EmotionPanelAsyncController.IgnoredStartAsync();
            return result;
        }

        #region "TestMethod"
        /// <summary>
        /// びっくりUI表示開始(試験用)
        /// </summary>
        public async void ExclamationStartTest()
        {
            bool result = await ExclamationStartAsync();
            Debug.Log("ExclamationStartTest : " + result.ToString());
        }

        /// <summary>
        /// はてなUI表示開始(試験用)
        /// </summary>
        public async void QuestionStartTest()
        {
            bool result = await QuestionStartAsync();
            Debug.Log("QuestionStartTest : " + result.ToString());
        }

        /// <summary>
        /// 無視表示開始(試験用)
        /// </summary>
        public async void IgnoredStartTest()
        {
            bool result = await IgnoredStartAsync();
            Debug.Log("IgnoredStartTest : " + result.ToString());
        }
        #endregion
    }
}