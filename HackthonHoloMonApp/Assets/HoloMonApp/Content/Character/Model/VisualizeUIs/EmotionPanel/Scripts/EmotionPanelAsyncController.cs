using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel
{
    public class EmotionPanelAsyncController : MonoBehaviour
    {
        [SerializeField, Tooltip("感情UIコントローラの参照")]
        private EmotionUIController p_EmotionUIController;

        /// <summary>
        /// アニメーション実行中フラグ
        /// </summary>
        private bool p_CurrentAnimFlg;

        /// <summary>
        /// キャンセルトークン
        /// </summary>
        private CancellationTokenSource p_CancellationTokenSource;

        /// <summary>
        /// 自動タイムアウト時間(ミリ秒)
        /// </summary>
        private int p_TimeOutMilliSec = 5000;

        private enum AnimationType
        {
            Nothing,
            Exclamation,
            Question,
            Ignored,
        }

        /// <summary>
        /// 実行中のアニメーションタイプ
        /// </summary>
        private AnimationType p_CurrentAnimationType;

        private void OnEnable()
        {
            // 感情UIの完了でアニメーション完了とする
            if (p_EmotionUIController != null) p_EmotionUIController.CompletedEvent += CompletedAnimation;
        }

        private void OnDisable()
        {
            // 感情UIの完了でアニメーション完了とする
            if (p_EmotionUIController != null) p_EmotionUIController.CompletedEvent -= CompletedAnimation;
        }

        /// <summary>
        /// びっくりモーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> ExclamationStartAsync()
        {
            bool result = await CommonStartAsync(AnimationType.Exclamation);
            return result;
        }

        /// <summary>
        /// はてなモーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> QuestionStartAsync()
        {
            bool result = await CommonStartAsync(AnimationType.Question);
            return result;
        }

        /// <summary>
        /// 無視モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> IgnoredStartAsync()
        {
            bool result = await CommonStartAsync(AnimationType.Ignored);
            return result;
        }


        #region "private"
        private async UniTask<bool> CommonStartAsync(AnimationType a_AnimationType)
        {
            // アニメーション実行中は表示しない
            if (p_CurrentAnimFlg)
            {
                return false;
            }

            // アニメーション操作の参照がない場合はアニメーションを実行しない
            if (p_EmotionUIController == null)
            {
                return true;
            }

            // アニメーション種別を設定する
            p_CurrentAnimFlg = true;
            p_CurrentAnimationType = a_AnimationType;

            // アニメーションの実行
            EmotionCall();

            // 条件一致でも WaitUntil で1フレーム待機してしまうため
            // 初回フレームはif文で条件チェックを行う
            if (p_CurrentAnimFlg == true)
            {
                // タイムアウト用のキャンセルトークンを作成する
                p_CancellationTokenSource = new CancellationTokenSource();

                // 指定秒数後には自動的に待機をキャンセルする
                p_CancellationTokenSource.CancelAfter(p_TimeOutMilliSec);

                // フラグが False (実行完了)になるまで待機する
                try
                {
                    await UniTask.WaitUntil(
                        () => p_CurrentAnimFlg == false,
                        cancellationToken: p_CancellationTokenSource.Token
                        );
                }
                catch (OperationCanceledException ex)
                {
                    Debug.Log(ex.Message);
                }
            }

            return true;
        }

        /// <summary>
        /// エモーションの呼び出し関数
        /// </summary>
        private void EmotionCall()
        {
            switch (p_CurrentAnimationType)
            {
                case AnimationType.Exclamation:
                    p_EmotionUIController.ExclamationStart();
                    break;
                case AnimationType.Question:
                    p_EmotionUIController.QuestionStart();
                    break;
                case AnimationType.Ignored:
                    p_EmotionUIController.IgnoredStart();
                    break;
                default:
                    p_EmotionUIController.NothingStart();
                    break;
            }
        }

        /// <summary>
        /// アニメーション完了時の呼び出し関数
        /// </summary>
        private void CompletedAnimation()
        {
            // アニメーション実行中フラグをOFFにする
            p_CurrentAnimFlg = false;
        }
        #endregion
    }
}