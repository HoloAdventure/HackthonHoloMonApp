using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.LookAround
{
    public class HoloMonModeLogicLookAround : MonoBehaviour, HoloMonActionModeLogicIF
    {
        /// <summary>
        /// モードロジック共通参照
        /// </summary>
        private ModeLogicReference p_ModeLogicReference;

        /// <summary>
        /// モードロジック共通情報
        /// </summary>
        [SerializeField, Tooltip("モードロジック共通情報")]
        private HoloMonModeLogicCommon p_ModeLogicCommon = new HoloMonModeLogicCommon();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="reference"></param>
        public void AwakeInit(HoloMonBehaveReference reference)
        {
            p_ModeLogicReference = new ModeLogicReference(reference);
        }

        /// <summary>
        /// 現在の実行待機中フラグ
        /// </summary>
        /// <returns></returns>
        public bool CurrentRunAwaitFlg()
        {
            return p_ModeLogicCommon.RunAwaitFlg;
        }

        /// <summary>
        /// ホロモンアクションモード種別
        /// </summary>
        /// <returns></returns>
        public HoloMonActionMode GetHoloMonActionMode()
        {
            return HoloMonActionMode.LookAround;
        }

        /// <summary>
        /// モード実行(async/await制御)
        /// </summary>
        public async UniTask<ModeLogicResult> RunModeAsync(ModeLogicSetting a_ModeLogicSetting)
        {
            // 設定アクションデータを保持する
            p_ModeLogicCommon.SaveCommonSetting(a_ModeLogicSetting);

            // 開始処理を行う
            EnableSetting();

            // モードを開始して完了を待機する
            ModeLogicResult result = await p_ModeLogicCommon.RunModeAsync();

            // 終了状態を返却する
            return result;
        }

        /// <summary>
        /// モードキャンセル
        /// </summary>
        public void CancelMode()
        {
            // 停止処理を行う
            DisableSetting();

            // キャンセル処理を行う
            p_ModeLogicCommon.CancelMode();
        }

        /// <summary>
        /// モード内部停止
        /// </summary>
        private void StopMode(ModeLogicResult a_StopModeLogicResult)
        {
            // 停止処理を行う
            DisableSetting();

            // 停止状態を設定する
            p_ModeLogicCommon.StopMode(a_StopModeLogicResult);
        }

        /// <summary>
        /// 割込み通知
        /// </summary>
        public bool TransmissionInterrupt(InterruptInformation a_InterruptInfo)
        {
            bool isProcessed = false;

            // 処理対象の割込み処理のみ記述
            switch (a_InterruptInfo.HoloMonInterruptType)
            {
                case HoloMonInterruptType.FieldOfVision:
                    // 視界オブジェクトの割込み
                    InterruptFieldOfVisionData interruptFieldOfVisionData = a_InterruptInfo.InterruptFieldOfVisionData;
                    // イベント種別をチェックする
                    if (interruptFieldOfVisionData.FieldOfVisionEvent == HoloMonFieldOfVisionEvent.Add)
                    {
                        // 捜索対象のオブジェクトが検出された場合は割込みを処理する
                        isProcessed = CheckFindVision(interruptFieldOfVisionData.VisionObjectWrap);
                    }
                    break;
                default:
                    break;
            }

            return isProcessed;
        }


        /// <summary>
        /// 固有アクションデータの参照
        /// </summary>
        private ModeLogicLookAroundData p_Data =>
            (ModeLogicLookAroundData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 捜索中フラグ
        /// </summary>
        bool p_SeachFlg = false;

        /// <summary>
        /// アクション完了実行のトリガー
        /// </summary>
        IDisposable p_ActionEndTrigger;


        /// <summary>
        /// モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションを見回しモードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartLookAroundMode();

            // 捜索オブジェクトが指定されていれば捜索フラグを立てる
            p_SeachFlg = (p_Data.SearchObjectUnderstandType != ObjectUnderstandType.Nothing);

            // トリガーを設定済みの場合は一旦破棄する
            p_ActionEndTrigger?.Dispose();

            // 見回しアニメーションが完了するのを待機する
            p_ActionEndTrigger = p_ModeLogicReference.View.AnimationsBodyAPI.AnimationTrigger
                .OnStateUpdateAsObservable()
                .Where(onStateInfo => !onStateInfo.Animator.IsInTransition(onStateInfo.LayerIndex)) // アニメーションの移行が完了するのを待つ
                .Where(onStateInfo => onStateInfo.StateInfo.normalizedTime > 0.95) // アニメーションの完了直前を検出する
                .Take(1)
                .Subscribe(_ =>
                {
                    if (p_Data.SearchObjectUnderstandType != ObjectUnderstandType.Nothing)
                    {
                        // 捜索オブジェクトが設定されていた場合

                        // 発見できずに終了した場合は目的未達と判定する
                        Debug.Log("LookAroundEnd Missing : " + p_Data.SearchObjectUnderstandType.ToString());
                        StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicLookAroundReturn()));
                    }
                    else
                    {
                        // 捜索オブジェクトが設定されていなかった場合

                        // 時間経過で目的達成と判定する
                        Debug.Log("LookAroundEnd Achievement : " + p_Data.SearchObjectUnderstandType.ToString());
                        StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicLookAroundReturn()));
                    }
                })
                .AddTo(this);

            return true;
        }

        /// <summary>
        /// モードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // トリガーを設定済みの場合は破棄する
            p_ActionEndTrigger?.Dispose();

            // 捜索状態を初期化する
            p_SeachFlg = false;

            return true;
        }

        /// <summary>
        /// 視界内オブジェクト発見状態の変化時のアクションを実行する
        /// </summary>
        private bool CheckFindVision(VisionObjectWrap a_FindObjectWrap)
        {
            bool isProcessed = false;

            // 発見オブジェクトが探しているタイプの物体か否かをチェックする
            if (a_FindObjectWrap.CurrentFeatures().ObjectUnderstandType == p_Data.SearchObjectUnderstandType)
            {
                // ビックリマークの感情UIを表示する（完了待機なし）
                p_ModeLogicReference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();

                // 捜索オブジェクトを発見できた場合は発見オブジェクトを引き渡して目的達成と判定する
                Debug.Log("LookAroundEnd Achievement : " + p_Data.SearchObjectUnderstandType.ToString());
                StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicLookAroundReturn(a_FindObjectWrap.Object)));
                isProcessed = true;
            }

            return isProcessed;
        }
    }
}