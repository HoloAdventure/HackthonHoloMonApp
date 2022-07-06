using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TurnTarget
{
    public class HoloMonModeLogicTurnTarget : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.TurnTarget;
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
            ModeLogicResult reult = await p_ModeLogicCommon.RunModeAsync();

            // 終了状態を返却する
            return reult;
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
        private ModeLogicTurnTargetData p_Data =>
            (ModeLogicTurnTargetData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 現在注視中のオブジェクト
        /// </summary>
        [SerializeField, Tooltip("現在注視中のオブジェクト")]
        GameObject p_LookingObject;

        /// <summary>
        /// 追跡対象オブジェクトの特徴
        /// </summary>
        ObjectUnderstandInformation p_LookObjectUnderstandInformation;

        /// <summary>
        /// 前回記録回転角
        /// </summary>
        Quaternion p_BeforeRotation;


        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            // モードが実行中かチェックする
            if (p_ModeLogicCommon.ModeLogicStatus == HoloMonActionModeStatus.Runtime)
            {
                if (p_Data.TargetObject == null)
                {
                    // 注視オブジェクト自体をロストした場合は即座に目的未達で終了する
                    Debug.Log("TurnTargetEnd Missing : Target Null");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTurnTargetReturn()));
                    return;
                }

                // オブジェクトの状態変化をチェックするか否か
                if (p_Data.IsCheckObjectStatus)
                {
                    if (p_LookObjectUnderstandInformation != null)
                    {
                        // 注視対象オブジェクトの特徴を取得する
                        ObjectUnderstandInformation lookObjectUnderstandInformation = p_ModeLogicReference
                            .View.SensationsFieldOfVisionAPI
                            .CheckCollectionByGameObject(a_Object: p_Data.TargetObject)?.CurrentFeatures();

                        if (lookObjectUnderstandInformation == null)
                        {
                            // 注視対象オブジェクトを見失っていた場合は即座に目的未達で終了する
                            Debug.Log("TurnTargetEnd Missing : Target Null");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTurnTargetReturn()));
                            return;
                        }

                        // 特徴が一致するか否か
                        if(p_LookObjectUnderstandInformation.ObjectUnderstandType
                            != lookObjectUnderstandInformation.ObjectUnderstandType)
                        {
                            // 注視オブジェクトの情報が変化していた場合は即座に目的達成で終了する
                            Debug.Log("TurnTargetEnd Missing : Target TypeChanged");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTurnTargetReturn()));
                            return;
                        }
                        else
                        {
                            if(p_LookObjectUnderstandInformation.ObjectUnderstandDataInterface.StatusHash()
                                != lookObjectUnderstandInformation.ObjectUnderstandDataInterface.StatusHash())
                            {
                                // 注視オブジェクトの情報が変化していた場合は即座に目的達成で終了する
                                Debug.Log("TurnTargetEnd Missing : Target StatusChanged");
                                StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTurnTargetReturn()));
                                return;
                            }
                        }
                    }
                }

                // 遠距離チェックが有効か否か
                if (p_Data.CheckFarDistance > 0.0f)
                {
                    bool isResult = CheckFarDistance(p_Data.TargetObject, p_Data.CheckFarDistance);
                    if (isResult)
                    {
                        // 注視オブジェクトが遠距離に移動した場合は即座に目的未達で終了する
                        Debug.Log("TurnTargetEnd Missing : Target Far");
                        StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTurnTargetReturn()));
                        return;
                    }
                }

                // 現在の回転角を取得する
                Quaternion currentRotation = p_ModeLogicReference.View.TransformsBodyAPI.WorldRotation;

                // 前回との差分を取得する
                float diffAngle = Quaternion.Angle(p_BeforeRotation, currentRotation);

                // Update間隔の係数を取得する
                float secCoefficient = 1.0f / Time.deltaTime;

                // 1秒あたりの回転速度を取得する
                float secAngle = diffAngle * secCoefficient;

                // 回転速度をアニメーションに通知する
                p_ModeLogicReference.Control.AnimationsBodyAPI.SetModelRotate(secAngle);

                // 現在のターゲットとの相対角度を取得する
                float diffTargetAngle = p_ModeLogicReference.View.BodyComponentsToBillBoardAPI.CurrentDiffTargetAngle;

                // 正面を向いているか否か
                if (diffTargetAngle < p_Data.CheckFrontAngle)
                {
                    // 正面の完了判定を行うか否か
                    if (p_Data.IsAchievementFront)
                    {
                        // 完了判定を行う場合
                        // 正面を向けば目標を達成したと判定する
                        Debug.Log("Turn Achievement");

                        // 追跡を停止する
                        StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicTurnTargetReturn()));
                    }
                    else
                    {
                        // 完了判定を行う場合
                        // 正面を向けば一時的に追跡は停止する
                        if (CurrentActiveBillboard()) ChangeActiveBillboard(false);
                    }
                }
                else
                {
                    // 正面を向いておらず、追跡が無効化されている場合は追跡を再開する
                    if (!CurrentActiveBillboard()) ChangeActiveBillboard(true);
                }
                
                // 最終的な回転角を記録する
                p_BeforeRotation = p_ModeLogicReference.View.TransformsBodyAPI.WorldRotation;
            }
        }

        /// <summary>
        /// 回転モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションを回転モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartTurnMode();

            // 頭部追従ロジックを設定する
            ActionHeadLookObject(p_Data.TargetObject);

            // ターゲットを指定する
            p_ModeLogicReference.Control.BodyComponentsToBillBoardAPI.SetTargetTransform(p_Data.TargetObject.transform);

            // 回転開始時は方向回転を有効化する
            p_ModeLogicReference.Control.BodyComponentsToBillBoardAPI.SetIsActive(true);
            p_ModeLogicReference.Control.BodyComponentsToBillBoardAPI.SetIsYAxisInverse(true);

            // 現在の回転角を記録する
            p_BeforeRotation = p_ModeLogicReference.View.TransformsBodyAPI.WorldRotation;

            // 注視対象オブジェクトの特徴を取得する
            p_LookObjectUnderstandInformation = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByGameObject(a_Object: p_Data.TargetObject).CurrentFeatures();

            return true;
        }

        /// <summary>
        /// 回転モードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetModelRotate(0.0f);

            // 頭部追従ロジックを解除する
            ActionHeadLookObject(null);

            // 回転終了時は方向回転を無効化する
            p_ModeLogicReference.Control.BodyComponentsToBillBoardAPI.SetIsActive(false);
            p_ModeLogicReference.Control.BodyComponentsToBillBoardAPI.SetIsYAxisInverse(false);

            // 回転角を初期化する
            p_BeforeRotation = new Quaternion();

            return true;
        }

        /// <summary>
        /// Billboardの有効無効を切り替える
        /// </summary>
        private void ChangeActiveBillboard(bool onoff)
        {
            // Billboardの有効無効状態を切り替える
            p_ModeLogicReference.Control.BodyComponentsToBillBoardAPI.SetIsActive(onoff);
        }

        /// <summary>
        /// Billboardの有効無効状態を取得する
        /// </summary>
        private bool CurrentActiveBillboard()
        {
            // Billboardの有効無効状態を取得する
            return p_ModeLogicReference.View.BodyComponentsToBillBoardAPI.IsActive;
        }

        /// <summary>
        /// 注目オブジェクトの遠距離チェックを行う
        /// </summary>
        /// <param name="a_CheckFarDistance"></param>
        /// <returns></returns>
        private bool CheckFarDistance(GameObject a_TargetObject, float a_CheckFarDistance)
        {
            bool isResult = false;

            // 指定オブジェクトとの距離をチェックする
            float lookObjectDistance = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CurrentDistanceForGameObject(a_Object: a_TargetObject);

            // 現在のホロモンの大きさに合わせてチェック距離を調整する
            float checkScaleFarDistance = a_CheckFarDistance + CurrentActualForwardDepthLength();

            if (lookObjectDistance > checkScaleFarDistance)
            {
                isResult = true;
            }

            return isResult;
        }

        /// <summary>
        /// 頭部アクションを行う
        /// </summary>
        private void ActionHeadLookObject(GameObject a_LookObject)
        {
            if (p_LookingObject != null && a_LookObject != null)
            {
                if (p_LookingObject == a_LookObject)
                {
                    // 対象が同一オブジェクトの場合は処理しない
                    return;
                }
            }

            // 頭部追従ロジックを設定する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionLookAtTarget(a_LookObject);
            p_LookingObject = a_LookObject;
        }


        /// <summary>
        /// 現在のホロモンの前方向の体長を取得する
        /// </summary>
        /// <returns></returns>
        private float CurrentActualForwardDepthLength()
        {
            float currentActualForwardDepthLength = p_ModeLogicReference.View.SettingsLengthAPI.HoloMonActualDepthLength / 2.0f;

            return currentActualForwardDepthLength;
        }
    }
}