using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Interrupt.Sensations.FeelHandGrab;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.HungUp
{
    public class HoloMonModeLogicHungUp : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.HungUp;
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
                    {
                        // 視界オブジェクトの割込み
                        InterruptFieldOfVisionData interruptFieldOfVisionData = a_InterruptInfo.InterruptFieldOfVisionData;
                        // イベント種別をチェックする
                        switch (interruptFieldOfVisionData.FieldOfVisionEvent)
                        {
                            case HoloMonFieldOfVisionEvent.Add:
                                // 視界オブジェクトが新規検出された場合は 1 秒間よそ見をする
                                isProcessed = Lookaway(interruptFieldOfVisionData.VisionObjectWrap.Object, 1.0f);

                                // よそ見は割込み処理と判定しない
                                isProcessed = false;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case HoloMonInterruptType.HandGrabBody:
                    {
                        // 掴み状態の割込み
                        InterruptFeelHandGrabData interruptHungUpData = a_InterruptInfo.InterruptHandGrabBodyData;
                        if (!interruptHungUpData.IsGrabbed)
                        {
                            // 手放された場合は目的達成で終了する
                            Debug.Log("HungUpEnd Achievement : " + interruptHungUpData.IsGrabbed.ToString());
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicHungUpReturn()));
                        }
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
        private ModeLogicHungUpData p_Data => 
            (ModeLogicHungUpData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;

        /// <summary>
        /// 一時頭部追従実行のトリガー
        /// </summary>
        IDisposable p_TempHeadLogicTrigger;


        /// <summary>
        /// モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションを掴まれモードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartHungUpMode();

            // 頭部追従ロジックを無効化する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();

            return true;
        }

        /// <summary>
        /// モードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // 頭部追従ロジックを無効化する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();

            return true;
        }

        /// <summary>
        /// 一定時間よそ見をする
        /// </summary>
        /// <param name="a_LookObject"></param>
        private bool Lookaway(GameObject a_LookObject, float a_LookawaySec)
        {
            bool isProcessed = false;

            // 指定のオブジェクトを見る
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionLookAtTarget(a_LookObject);

            // トリガーを設定済みの場合は一旦破棄する
            p_TempHeadLogicTrigger?.Dispose();

            // 指定時間後によそ見キャンセルのトリガーを実行する
            p_TempHeadLogicTrigger = Observable
                .Timer(TimeSpan.FromSeconds(a_LookawaySec))
                .SubscribeOnMainThread()
                .Subscribe(x =>
                {
                    // 頭部追従ロジックを無効化する
                    p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();
                })
                .AddTo(this);

            isProcessed = true;

            return isProcessed;
        }
    }
}