using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Sensations.FeelEyeGaze;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Animations.Body;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Dance
{
    public class HoloMonModeLogicDance : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.Dance;
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
            return result;;
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
                case HoloMonInterruptType.EyeGazeBody:
                    {
                        // 視線入力の割込み
                        InterruptFeelEyeGazeData interruptGazeBodyData = a_InterruptInfo.InterruptGazeBodyData;
                        // 見られているときは恥ずかしがる
                        isProcessed = EyeGazeFeel(interruptGazeBodyData.IsSeen);
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
        private ModeLogicDanceData p_Data =>
            (ModeLogicDanceData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// ホロモンのダンス種別
        /// </summary>
        [SerializeField, Tooltip("ホロモンのダンス種別")]
        DanceType p_HoloMonDanceType;

        /// <summary>
        /// 恥ずかしがりポイント
        /// </summary>
        int p_EmbarrassPoint;

        /// <summary>
        /// 恥ずかしがりポイント敷居値
        /// </summary>
        int p_EmbarrassThreshold = 3;

        /// <summary>
        /// ダンスの開始時間
        /// </summary>
        float p_DanceStartTime;

        /// <summary>
        /// ダンスの継続時間(s)
        /// </summary>
        [SerializeField, Tooltip("ホロモンのダンスの継続時間(s)")]
        float p_DanceTime = -1;

        void Update()
        {
            if (p_ModeLogicCommon.ModeLogicStatus != HoloMonActionModeStatus.Runtime) return;

            if (p_DanceTime > 0)
            {
                // 指定の経過時間を過ぎていればダンスを終了する
                if ((Time.time - p_DanceStartTime) > p_DanceTime)
                {
                    // 目標を達成したと判定する
                    Debug.Log("DanceEnd Achievement");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicDanceReturn()));
                }
            }
        }

        /// <summary>
        /// ダンスモードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションをダンスモードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartDanceMode();
            
            // 初期状態はダンス待機状態に移行する
            MigrationDanceStepDanceStatus();

            // 恥ずかしがりポイントの初期化
            p_EmbarrassPoint = 0;

            // 開始時間の初期化
            p_DanceStartTime = Time.time;

            // 現在行うべきダンス状態をチェックする
            CheckDanceSection();

            return true;
        }

        /// <summary>
        /// ダンスモードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // ステータスをダンス待機状態に戻しておく
            MigrationDanceStepDanceStatus();

            return true;
        }

        /// <summary>
        /// 現在行うべきダンス状態をチェックする
        /// </summary>
        private void CheckDanceSection()
        {
            // 視線に合わせて恥ずかしがりの状態を変更する
            bool isSeen = p_ModeLogicReference.View.SensationsFeelEyeGazeAPI.EyeGazed;
            EyeGazeFeel(isSeen);
        }

        /// <summary>
        /// 視線の変化に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_IsSeen"></param>
        private bool EyeGazeFeel(bool a_IsSeen)
        {
            bool isProcessed = false;

            if (a_IsSeen)
            {
                // 見られている場合は恥ずかしがり状態に移行する
                MigrationDanceEmbarrassStatus();
                isProcessed = true;

                // 恥ずかしがりポイントを加算
                p_EmbarrassPoint++;
                // 敷居値を超えた場合はダンスを止める
                if (p_EmbarrassPoint >= p_EmbarrassThreshold)
                {
                    // 目標を達成したと判定する
                    Debug.Log("DanceEnd Achievement");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicDanceReturn()));
                }
            }
            else
            {
                // 見られていない場合はダンスステップ状態に移行する
                MigrationDanceStepDanceStatus();
                isProcessed = true;
            }

            return isProcessed;
        }


        /// <summary>
        /// ダンスステップ状態に移行する際の処理
        /// </summary>
        private void MigrationDanceStepDanceStatus()
        {
            // ダンスタイプ無し
            p_HoloMonDanceType = DanceType.StepDance;

            p_ModeLogicReference.Control.AnimationsBodyAPI.SetDanceType(p_HoloMonDanceType);
        }

        /// <summary>
        /// ダンス恥ずかしがり状態に移行する際の処理
        /// </summary>
        private void MigrationDanceEmbarrassStatus()
        {
            // 恥ずかしがり状態タイプ
            p_HoloMonDanceType = DanceType.Embarrass;

            p_ModeLogicReference.Control.AnimationsBodyAPI.SetDanceType(p_HoloMonDanceType);
        }
    }
}
