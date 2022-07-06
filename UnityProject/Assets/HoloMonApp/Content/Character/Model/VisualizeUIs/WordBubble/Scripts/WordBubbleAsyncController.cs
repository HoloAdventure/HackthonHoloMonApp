using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble
{
    public class WordBubbleAsyncController : MonoBehaviour
    {
        [SerializeField, Tooltip("言霊UIコントローラの参照")]
        private WordBubbleController p_BreakBubbleController;

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
            Arrive,
            Break,
        }

        /// <summary>
        /// 実行中のアニメーションタイプ
        /// </summary>
        private AnimationType p_CurrentAnimationType;

        private void OnEnable()
        {
            // 言霊アニメーションの完了でアニメーション完了とする
            if (p_BreakBubbleController != null) p_BreakBubbleController.CompletedEvent += CompletedAnimation;
        }

        private void OnDisable()
        {
            // 言霊アニメーションの完了でアニメーション完了とする
            if (p_BreakBubbleController != null) p_BreakBubbleController.CompletedEvent -= CompletedAnimation;
        }

        /// <summary>
        /// 言霊到達モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> ArriveStartAsync()
        {
            bool result = await CommonStartAsync(AnimationType.Arrive);
            return result;
        }

        /// <summary>
        /// 言霊破裂モーション実行
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> BreakStartAsync()
        {
            bool result = await CommonStartAsync(AnimationType.Break);
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
            if (p_BreakBubbleController == null)
            {
                return true;
            }

            // アニメーション種別を設定する
            p_CurrentAnimFlg = true;
            p_CurrentAnimationType = a_AnimationType;

            // アニメーションの実行
            BreakBubbleCall();

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
        /// 言霊アニメーションの開始
        /// </summary>
        private void BreakBubbleCall()
        {
            switch (p_CurrentAnimationType)
            {
                case AnimationType.Arrive:
                    p_BreakBubbleController.NonBreakStart();
                    break;
                case AnimationType.Break:
                    p_BreakBubbleController.BreakStart();
                    break;
                default:
                    p_BreakBubbleController.NothingStart();
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