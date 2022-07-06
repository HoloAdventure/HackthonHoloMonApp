using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh
{
    public class HoloMonModeLogicTrackingTargetNavMesh : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.TrackingTargetNavMesh;
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

            return isProcessed;
        }


        /// <summary>
        /// 固有アクションデータの参照
        /// </summary>
        private ModeLogicTrackingTargetNavMeshData p_Data =>
            (ModeLogicTrackingTargetNavMeshData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 追跡ポイントロジックの参照
        /// </summary>
        [SerializeField, Tooltip("追跡ポイントロジックの参照")]
        FootPositionTracking p_FootPositionTracking;

        /// <summary>
        /// 追跡設定(デフォルト値)
        /// </summary>
        [SerializeField, Tooltip("(デフォルト値)")]
        TrackingSettingNavMesh p_DefaultTrackingSetting;


        /// <summary>
        /// 停止確認カウンタ
        /// </summary>
        int StopCheckCount;
        int StopCheckThreshold = 5;


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // NavMeshの初期値を取得する
            p_DefaultTrackingSetting.SetNavMeshSetting(
                CurrentHeightScaleRatio(),
                p_ModeLogicReference.View.BodyComponentsToNavMeshAgentAPI.Agent
                );
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            // モードが実行中かチェックする
            if (p_ModeLogicCommon.ModeLogicStatus == HoloMonActionModeStatus.Runtime)
            {
                // エージェントに目的地を設定する
                p_ModeLogicReference.Control.BodyComponentsToNavMeshAgentAPI.SetDestination(p_FootPositionTracking.FootTransform.position);

                // 現在の速度を取得する
                float agentSpeed = p_ModeLogicReference.View.BodyComponentsToNavMeshAgentAPI.Velocity.magnitude;

                // 速度をアニメーション調整用にスケール値で補正する
                // 同じ速度でもスケールが小さければ低い敷居値で走りになる
                float scaleRatio = p_ModeLogicReference.View.ConditionsBodyAPI.DefaultBodyHeightRatio;
                float scaleCorrection = (agentSpeed / scaleRatio) * 3.0f;

                // 速度をアニメーションに通知する
                p_ModeLogicReference.Control.AnimationsBodyAPI.SetModelSpeed(scaleCorrection);

                // 移動中か否か
                if (agentSpeed <= 0.0f)
                {
                    // 移動していないならカウントアップ
                    StopCheckCount++;

                    // 敷居値回数以上停止していれば停止と判定する
                    if (StopCheckCount > StopCheckThreshold)
                    {
                        // 現時点のターゲットまでの距離
                        float targetDistance = Vector3.Distance(
                            p_FootPositionTracking.FootTransform.position,
                            p_ModeLogicReference.View.BodyComponentsToNavMeshAgentAPI.CurrentPosition
                            );

                        // ターゲットに到達したと判定する距離
                        // 停止位置から10cmの猶予距離を設定する
                        float stoppingDistance = p_ModeLogicReference.View.BodyComponentsToNavMeshAgentAPI.StopDistance + 0.1f;

                        // 移動していなければ追跡を停止し、到達できたか否かの判定を行う
                        if (targetDistance <= stoppingDistance)
                        {
                            // 到達場所が停止距離以内であれば追跡達成と判定する
                            Debug.Log("Tracking Achievement");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicTrackingTargetNavMeshReturn()));
                        }
                        else
                        {
                            // 到達場所が停止距離以遠であれば追跡失敗と判定する
                            Debug.Log("Tracking Missing");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTrackingTargetNavMeshReturn()));
                        }
                    }
                }
                else
                {
                    // 移動していればカウント初期化
                    StopCheckCount = 0;
                }
            }
        }


        /// <summary>
        /// 追跡モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            if (p_Data.HoldItemSetting.IsHold)
            {
                // アイテム保持設定があればアイテム保持のための設定を行う
                EnableItemHold(p_Data.HoldItemSetting.HoldItemObject);
            }

            // アニメーションを追跡モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartTrackingMode();

            // 頭部追従ロジックを設定する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionLookAtTarget(p_Data.TargetObject);


            // 追跡中はアタリ判定を切る
            p_ModeLogicReference.Control.BodyComponentsToColliderAPI.SetEnabled(false);

            // 追跡中は重力を切る
            p_ModeLogicReference.Control.BodyComponentsToRigidbodyAPI.SetUseGravity(false);

            // NavMeshの設定を反映する(デフォルト値を設定する)
            p_DefaultTrackingSetting.ApplyNavMeshSetting(
                CurrentHeightScaleRatio(),
                p_ModeLogicReference.View.BodyComponentsToNavMeshAgentAPI.Agent
                );

            // NavMeshの設定を反映する(上書き値を設定する)
            p_Data.TrackingSetting.ApplyNavMeshSetting(
                CurrentHeightScaleRatio(),
                p_ModeLogicReference.View.BodyComponentsToNavMeshAgentAPI.Agent
                );

            // 追跡位置を設定する
            p_FootPositionTracking.SetTrackingTransform(p_Data.TargetObject.transform);

            // 追跡中はエージェントを有効化する
            p_ModeLogicReference.Control.BodyComponentsToNavMeshAgentAPI.SetEnabled(true);

            // 停止チェックカウントを初期化する
            StopCheckCount = 0;

            return true;
        }

        /// <summary>
        /// 追跡モードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            if (p_Data.HoldItemSetting.IsHold)
            {
                // アイテム保持設定があればアイテム解放のための設定を行う
                DisableItemHold();
            }

            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // 頭部追従ロジックを解除する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();

            // 追跡中はアタリ判定を切る
            p_ModeLogicReference.Control.BodyComponentsToColliderAPI.SetEnabled(true);

            // 追跡終了時は重力を元に戻す
            p_ModeLogicReference.Control.BodyComponentsToRigidbodyAPI.SetUseGravity(true);

            // 追跡終了時はエージェントを無効化する
            p_ModeLogicReference.Control.BodyComponentsToNavMeshAgentAPI.SetEnabled(false);

            // 追跡設定をデフォルトに戻す
            p_DefaultTrackingSetting.ApplyNavMeshSetting(
                CurrentHeightScaleRatio(),
                p_ModeLogicReference.View.BodyComponentsToNavMeshAgentAPI.Agent
                );

            // 停止チェックカウントを初期化する
            StopCheckCount = 0;

            return true;
        }

        /// <summary>
        /// アイテムの保持状態を有効化する
        /// </summary>
        /// <param name="a_HoldItemObject"></param>
        /// <returns></returns>
        private bool EnableItemHold(GameObject a_HoldItemObject)
        {
            // アニメーションのアイテム保持モードを有効化する
            p_ModeLogicReference.Control.AnimationsBodyAPI.ChangeHoldItemOption(true);

            // アイテムを保持位置に固定する
            p_ModeLogicReference.Control.ObjectInteractionsHoldItemAPI.PinnedHoldItem(a_HoldItemObject);

            return true;
        }

        /// <summary>
        /// アイテムの保持状態を無効化する
        /// </summary>
        private bool DisableItemHold()
        {
            // アニメーションのアイテム保持モードを無効化する
            p_ModeLogicReference.Control.AnimationsBodyAPI.ChangeHoldItemOption(false);

            // アイテムを保持位置から解放する
            p_ModeLogicReference.Control.ObjectInteractionsHoldItemAPI.ReleaseHoldItem();

            return true;
        }


        /// <summary>
        /// 現在のホロモンの身体スケール比率を取得する
        /// </summary>
        /// <returns></returns>
        private float CurrentHeightScaleRatio()
        {
            // 現在の体の大きさを取得する
            float currentHeightRatio = p_ModeLogicReference.View.ConditionsBodyAPI.DefaultBodyHeightRatio;

            return currentHeightRatio;
        }
    }
}