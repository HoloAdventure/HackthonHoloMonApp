using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    [Serializable]
    public class HoloMonModeLogicCommon
    {
        /// <summary>
        /// モードロジック状態
        /// </summary>
        [SerializeField, Tooltip("モードロジック状態")]
        private HoloMonActionModeStatus p_ModeLogicStatus;

        /// <summary>
        /// モードロジック状態の参照変数
        /// </summary>
        /// <returns></returns>
        public HoloMonActionModeStatus ModeLogicStatus => p_ModeLogicStatus;

        /// <summary>
        /// 設定アクションデータ
        /// </summary>
        [SerializeField, Tooltip("設定アクションデータ")]
        private ModeLogicSetting p_ModeLogicSetting;

        /// <summary>
        /// 設定アクションデータの参照変数
        /// </summary>
        public ModeLogicSetting ModeLogicSetting => p_ModeLogicSetting;

        /// <summary>
        /// 完了結果データ
        /// </summary>
        [SerializeField, Tooltip("完了結果データ")]
        private ModeLogicResult p_ModeLogicResult;

        /// <summary>
        /// 完了結果データの参照変数
        /// </summary>
        public ModeLogicResult ModeLogicResult => p_ModeLogicResult;

        [SerializeField, Tooltip("実行待機中フラグ")]
        private bool p_RunAwaitFlg;

        /// <summary>
        /// 実行待機中フラグ
        /// </summary>
        public bool RunAwaitFlg => p_RunAwaitFlg;

        [Tooltip("キャンセルトークンソース")]
        private CancellationTokenSource p_CancellationTokenSource;

        /// <summary>
        /// キャンセルトークン
        /// </summary>
        public CancellationToken ModeCancellationToken
            => p_CancellationTokenSource.Token;

        /// <summary>
        /// モード開始完了待機(async/await処理)
        /// </summary>
        public async UniTask<ModeLogicResult> RunModeAsync(int a_TimeOutMilliSec = -1)
        {
            // 開始時の状態を設定する
            p_ModeLogicStatus = HoloMonActionModeStatus.Runtime;

            // モード実行中フラグをONにする
            p_RunAwaitFlg = true;

            // キャンセル用トークンを作成する
            p_CancellationTokenSource = new CancellationTokenSource();

            // タイムアウト時間が指定されていた場合はタイムアウト時間を設定する
            if (a_TimeOutMilliSec > -1)
            {
                p_CancellationTokenSource.CancelAfter(a_TimeOutMilliSec);
            }

            // 条件一致でも WaitUntil で1フレーム待機してしまうため
            // 初回フレームはif文で条件チェックを行う
            if (p_RunAwaitFlg == true)
            {
                // モードが完了するまで待機する
                await UniTask.WaitUntil(
                    () => p_RunAwaitFlg == false,
                    cancellationToken: p_CancellationTokenSource.Token
                    );
            }

            return p_ModeLogicResult;
        }

        /// <summary>
        /// モード停止処理
        /// </summary>
        public void StopMode(
            ModeLogicResult a_ModeLogicResult
            )
        {
            // 停止時の状態を設定する
            p_ModeLogicStatus = a_ModeLogicResult.FinishModeStatus;

            // 実行結果データを取得する
            p_ModeLogicResult = a_ModeLogicResult;

            // モード実行中フラグをOFFにする
            p_RunAwaitFlg = false;
        }

        /// <summary>
        /// モードキャンセル処理
        /// </summary>
        public void CancelMode()
        {
            // キャンセル時の状態を設定する
            p_ModeLogicStatus = HoloMonActionModeStatus.Cancel;

            // モード実行中フラグをOFFにする
            p_RunAwaitFlg = false;

            // キャンセルトークンが生成されている場合はキャンセルを行う
            if (p_CancellationTokenSource != null)
            {
                p_CancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// 設定アクションデータを保持する
        /// </summary>
        public void SaveCommonSetting(ModeLogicSetting a_ModeLogicSetting)
        {
            // 設定アクションデータを保持する
            p_ModeLogicSetting = a_ModeLogicSetting;
        }
    }
}