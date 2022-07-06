using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTarget
{
    public class HoloMonModeLogicTrackingTarget : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.TrackingTarget;
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
        private ModeLogicTrackingTargetData p_Data =>
            (ModeLogicTrackingTargetData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 現在追跡中のオブジェクト
        /// </summary>
        [SerializeField, Tooltip("現在追跡中のオブジェクト")]
        private GameObject p_TrackingObject;

        [SerializeField, Tooltip("追跡対象を確認する時間間隔(秒)")]
        float p_TargetCheckTimeSpan = 1.0f;
        float p_TargetCheckTimeElapsed;

        [SerializeField, Tooltip("追跡対象が見当たらない場合に追跡を解除する上限回数")]
        int p_TargetNotFoundThreshold = 3;

        [SerializeField, Tooltip("追跡対象が見当たらなかった回数")]
        int p_TargetNotFoundCount;


        /// <summary>
        /// スタック確認定期処理
        /// </summary>
        private bool p_StackCheckFlg = false;
        private float p_StackCheckTime = 1.0f;
        private float p_StackTimeElapsed;

        /// <summary>
        /// スタック判定距離
        /// </summary>
        float p_StackDistance = 0.1f;

        /// <summary>
        /// スタック判定用ポジション
        /// </summary>
        Vector3 p_StackCheckPostion;

        /// <summary>
        /// 前回フレームポジション
        /// </summary>
        Vector3 p_PrefPosition;


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {

        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            // モードが実行中かチェックする
            if (p_ModeLogicCommon.ModeLogicStatus == HoloMonActionModeStatus.Runtime)
            {
                // 一定時間ごとに追跡対象を確認する
                p_TargetCheckTimeElapsed += Time.deltaTime;
                if (p_TargetCheckTimeElapsed >= p_TargetCheckTimeSpan)
                {
                    // 視界内に追跡オブジェクトが見えているかチェックする
                    VisionObjectWrap friendObjectWrap = p_ModeLogicReference
                        .View.SensationsFieldOfVisionAPI
                        .CheckCollectionByGameObject(p_TrackingObject);

                    p_TargetCheckTimeElapsed = 0.0f;

                    if (friendObjectWrap == null)
                    {
                        // 追跡対象が見当たらなかった場合はカウントアップ
                        p_TargetNotFoundCount++;

                        if (p_TargetNotFoundCount > p_TargetNotFoundThreshold)
                        {
                            // 指定回数見つからなければ失敗として追跡を解除する
                            Debug.Log("TrackingTarget Missing : Target Not Found");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTrackingTargetReturn()));
                            return;
                        }
                    }
                    else
                    {
                        // 追跡対象が見つかった場合はカウントリセット
                        p_TargetNotFoundCount = 0;
                    }
                }


                // 自身の座標
                Vector3 selfPostion = p_ModeLogicReference.View.TransformsBodyAPI.WorldPosition;

                // ターゲットの座標
                Vector3 targetPosition = p_TrackingObject.transform.position;

                // ターゲットまでの距離が停止距離か判定する
                if (CheckStoppingDistance(targetPosition, selfPostion))
                {
                    // ターゲット位置に到達していれば完了とする
                    Debug.Log("Tracking Achievement");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicTrackingTargetReturn()));
                    return;
                }

                // 現在位置と過去位置からスタックを判定する
                if (CheckImmobility(selfPostion))
                {
                    // 到達場所が停止距離以遠の状態でスタックしていれば追跡失敗と判定する
                    Debug.Log("Tracking Missing");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicTrackingTargetReturn()));
                    return;
                }

                // ターゲットの足元座標 (Y軸方向の追跡はしないので軸の座標は自身と同じとする)
                Vector3 targetFootPosition = new Vector3(
                    targetPosition.x,
                    selfPostion.y,
                    targetPosition.z);

                // アニメーションパラメータに速度設定を反映する
                // (実速度では変動が大きいため、設定値をそのまま利用する)
                ApplyAnimationSpeed(p_Data.TrackingSetting.MoveSpeed);


                // 自身の向き
                Vector3 selfRotateVector = p_ModeLogicReference.View.TransformsBodyAPI.WorldForward;

                // ターゲットへの方向ベクトル
                Vector3 toTargetVector = (targetFootPosition - selfPostion).normalized;

                // フレーム間隔に合わせて回転速度と移動速度を計算する
                float deltaRotateSpeed = p_Data.TrackingSetting.RotateSpeed * Time.deltaTime;
                float deltaMoveSpeed = p_Data.TrackingSetting.MoveSpeed * Time.deltaTime;

                // 回転を反映する
                Vector3 newRotateVector = Vector3.RotateTowards(selfRotateVector, toTargetVector, deltaRotateSpeed, 0f);
                p_ModeLogicReference.Control.BodyComponentsToTransformUtilityAPI.SetRotation(Quaternion.LookRotation(newRotateVector));

                // 移動ベクトルを位置に反映する
                Vector3 nextPosition = Vector3.MoveTowards(selfPostion, targetFootPosition, deltaMoveSpeed);
                p_ModeLogicReference.Control.BodyComponentsToTransformUtilityAPI.SetPosition(nextPosition);

                // 前回フレーム位置を記録する
                p_PrefPosition = selfPostion;
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

            // 追跡オブジェクトを設定する
            p_TrackingObject = p_Data.TargetObject;

            // スタックチェックを初期化する
            p_StackCheckFlg = false;
            p_StackTimeElapsed = 0.0f;
            p_StackCheckPostion = Vector3.zero;

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

            // 回転慣性を切る
            p_ModeLogicReference.Control.BodyComponentsToRigidbodyAPI.ResetVelocity();

            // 追跡オブジェクトを解除する
            p_TrackingObject = null;

            // スタックチェックを初期化する
            p_StackCheckFlg = false;
            p_StackTimeElapsed = 0.0f;
            p_StackCheckPostion = Vector3.zero;

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
        /// 到達距離のチェック
        /// </summary>
        private bool CheckStoppingDistance(Vector3 a_TargetPosition, Vector3 a_SelfPostion)
        {
            bool isReached = false;

            // 判定位置の指定に合わせて到達距離のチェックを行う
            Vector3 arrivalCheckPosition = Vector3.zero;
            switch (p_Data.TrackingSetting.ArrivalCheckPosition)
            {
                case TrackingTargetCheckPosition.ObjectOrigin:
                    // オリジナルの座標で判定
                    arrivalCheckPosition = a_TargetPosition;
                    break;
                case TrackingTargetCheckPosition.FootPosition:
                    // ターゲットの足元座標で判定 (Y軸方向の追跡はしないので軸の座標は自身と同じとする)
                    arrivalCheckPosition = new Vector3(
                            a_TargetPosition.x,
                            a_SelfPostion.y,
                            a_TargetPosition.z);
                    break;
                default:
                    break;
            }

            // ターゲットまでの距離
            float toTargetDistance = Vector3.Distance(arrivalCheckPosition, a_SelfPostion);

            // 現在のホロモンの大きさに合わせて停止距離を調整する
            float checkDistance = p_Data.TrackingSetting.StoppingDistance + CurrentActualHandReachableLength();

            if (toTargetDistance < checkDistance)
            {
                isReached = true;
            }

            return isReached;
        }

        /// <summary>
        /// 移動を継続できているか(スタックしていないか)のチェック
        /// </summary>
        private bool CheckImmobility(Vector3 a_CurrentPostion)
        {
            bool isImmovility = false;

            // 一定時間ごとに移動距離を判定する
            p_StackTimeElapsed += Time.deltaTime;
            if (p_StackTimeElapsed > p_StackCheckTime)
            {
                // チェック座標を保存済みか否か
                if (p_StackCheckFlg)
                {
                    // スタック範囲内でしか動けていないのであれば移動していないと判定する
                    float moveDistance = Vector3.Distance(a_CurrentPostion, p_StackCheckPostion);
                    if (moveDistance < p_StackDistance)
                    {
                        isImmovility = true;
                    }
                }

                // 前回位置を記録する
                p_StackCheckFlg = true;
                p_StackCheckPostion = a_CurrentPostion;
                p_StackTimeElapsed = 0.0f;
            }

            return isImmovility;
        }

        /// <summary>
        /// 現在の速度をアニメーションパラメータに反映する
        /// </summary>
        private void ApplyAnimationSpeed(float a_AgentSpeed)
        {
            // 速度をアニメーション調整用にスケール値で補正する
            // 同じ速度でもスケールが小さければ低い敷居値で走りになる
            float scaleRatio = CurrentHeightScaleRatio();
            float scaleAgentSpeed = (a_AgentSpeed / scaleRatio);

            // 速度をアニメーションに通知する
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetModelSpeed(scaleAgentSpeed);
        }


        /// <summary>
        /// 現在のホロモンの身体スケール比率を取得する
        /// </summary>
        /// <returns></returns>
        private float CurrentHeightScaleRatio()
        {
            float currentHeightRatio = p_ModeLogicReference.View.ConditionsBodyAPI.DefaultBodyHeightRatio;

            return currentHeightRatio;
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

        /// <summary>
        /// 現在のホロモンの手が届く距離を取得する
        /// </summary>
        /// <returns></returns>
        private float CurrentActualHandReachableLength()
        {
            float currentActualHandReachableLength = p_ModeLogicReference.View.SettingsLengthAPI.HoloMonActualHandReachableLength;

            return currentActualHandReachableLength;
        }
    }
}