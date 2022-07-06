using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Sensations.TactileBody;
using HoloMonApp.Content.Character.Data.Animations.Body;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Sleep
{
    public class HoloMonModeLogicSleep : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.Sleep;
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
                case HoloMonInterruptType.TactileBody:
                    {
                        // 触覚オブジェクトの割込み
                        InterruptTactileBodyData interruptTactileBodyData = a_InterruptInfo.InterruptTactileBodyData;
                        // イベント種別をチェックする
                        if (interruptTactileBodyData.TactileBodyEvent == HoloMonTactileBodyEvent.Add)
                        {
                            if (interruptTactileBodyData.TactileBodyLimb == HoloMonTactileBodyLimb.Head)
                            {
                                // 頭部への触覚オブジェクトが新規検出された場合は反応する
                                isProcessed = TouchHeadReaction(interruptTactileBodyData.TactileObjectWrap);
                            }
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
        private ModeLogicSleepData p_Data =>
            (ModeLogicSleepData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 一時アクションの実行のトリガー
        /// </summary>
        IDisposable p_TempActionTrigger;


        /// <summary>
        /// モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションを睡眠モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartSleepMode();

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

            return true;
        }

        /// <summary>
        /// 頭に触れられたことに反応する
        /// </summary>
        /// <param name="a_TaouchObject"></param>
        private bool TouchHeadReaction(TactileObjectWrap a_TactileObjectWrap)
        {
            bool isProcessed = false;

            // 頭部に触れたオブジェクトが右手もしくは左手か
            if ((a_TactileObjectWrap.CurrentFeatures().ObjectUnderstandType == ObjectUnderstandType.FriendRightHand)
                || (a_TactileObjectWrap.CurrentFeatures().ObjectUnderstandType == ObjectUnderstandType.FriendLeftHand))
            {
                // 手の状態を取得する
                int HandStatusHash = a_TactileObjectWrap.CurrentFeatures().ObjectUnderstandDataInterface.StatusHash();

#if UNITY_EDITOR
                // Editor上では確認のため、ピストル状態でパーと判定する
                if (HandStatusHash == (int)ObjectStatusHand.Hand_Pistol) HandStatusHash = (int)ObjectStatusHand.Hand_Par;
#endif

                if (HandStatusHash == (int)ObjectStatusHand.Hand_Par)
                {
                    // パーの手で触れられていた場合は頭を撫でられたアクションを行う

                    /// 頭を撫でられたアクションを実行する
                    p_ModeLogicReference.Control.AnimationsBodyAPI.SetSleepPose(SleepPose.SleepFun);

                    // トリガーを設定済みの場合は一旦破棄する
                    p_TempActionTrigger?.Dispose();

                    // アニメーション完了後キャンセルのトリガーを実行する
                    p_TempActionTrigger = p_ModeLogicReference.View.AnimationsBodyAPI.AnimationTrigger
                        .OnStateUpdateAsObservable()
                        .Where(onStateInfo => !onStateInfo.Animator.IsInTransition(onStateInfo.LayerIndex)) // アニメーションの移行が完了するのを待つ
                        .Where(onStateInfo => onStateInfo.StateInfo.normalizedTime > 0.3) // 1回のみ尻尾振りを行う
                        .Take(1)
                        .Subscribe(_ =>
                        {
                            // 通常ポーズに戻す
                            p_ModeLogicReference.Control.AnimationsBodyAPI.SetSleepPose(SleepPose.Nothing);
                        })
                        .AddTo(this);

                    isProcessed = true;
                }
            }

            return isProcessed;
        }
    }
}