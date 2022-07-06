using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.AI;

using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.Control.VisualizeUIs;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision
{
    /// <summary>
    /// 視界情報によって行動目的を決定する
    /// </summary>
    public class HoloMonInterruptFieldOfVisionAPI : MonoBehaviour, HoloMonInterruptIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonInterruptReference p_Reference;

        /// <summary>
        /// 初期化
        /// </summary>
        public void AwakeInit(HoloMonInterruptReference reference)
        {
            p_Reference = reference;
        }


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // 見つけたものの変化時の処理を設定する
            p_Reference.View.SensationsFieldOfVisionAPI
                .ReactivePropertyFindObjectWrap
                .ObserveOnMainThread()
                .Subscribe(objectWrap =>
                {
                    if (objectWrap.Object == null) return;
                    //Debug.Log("FindObjectWrap : objectname = " + objectWrap.CurrentName() + ".");
                    FindFieldOfVisionObjectWrap(objectWrap);
                })
                .AddTo(this);

            // 見失ったものの変化時の処理を設定する
            p_Reference.View.SensationsFieldOfVisionAPI
                .ReactivePropertyLostObjectInstanceID
                .ObserveOnMainThread()
                .Subscribe(objectInstanceID =>
                {
                    //Debug.Log("LostObjectName : objectInstanceID = " + objectInstanceID + ".");
                    LostFieldOfVisionObjectWrap(objectInstanceID);
                })
                .AddTo(this);

            // 状態変化に気づいたものの変化時の処理を設定する
            p_Reference.View.SensationsFieldOfVisionAPI
                .ReactivePropertyUpdateStatusObjectWrap
                .ObserveOnMainThread()
                .Subscribe(objectWrap =>
                {
                    if (objectWrap.Object == null) return;
                    //Debug.Log("UpdateStatusObjectWrap : objectname = " + objectWrap.CurrentName() + ".");
                    UpdateStatusFieldOfVisionObjectWrap(objectWrap);
                })
                .AddTo(this);

            // 距離変化に気づいたものの変化時の処理を設定する
            p_Reference.View.SensationsFieldOfVisionAPI
                .ReactivePropertyUpdateRangeObjectWrap
                .ObserveOnMainThread()
                .Subscribe(objectWrap =>
                {
                    if (objectWrap.Object == null) return;
                    //Debug.Log("UpdateRangeObjectWrap : objectname = " + objectWrap.CurrentName() + ".");
                    UpdateRangeFieldOfVisionObjectWrap(objectWrap);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 発見時の割込み情報データを作成する
        /// </summary>
        private InterruptInformation MakeInterruptFineVisionInfo(VisionObjectWrap a_FindObjectWrap)
        {
            return new InterruptInformation(new InterruptFieldOfVisionData(
                HoloMonFieldOfVisionEvent.Add,
                a_FindObjectWrap,
                a_FindObjectWrap.Object.GetInstanceID()
                ));
        }

        /// <summary>
        /// ロスト時の割込み情報データを作成する
        /// </summary>
        private InterruptInformation MakeInterruptLostVisionInfo(int a_ObjectInstanceID)
        {
            return new InterruptInformation(new InterruptFieldOfVisionData(
                HoloMonFieldOfVisionEvent.Lost,
                null,
                a_ObjectInstanceID
                ));
        }

        /// <summary>
        /// 状態変化検出時の割込み情報データを作成する
        /// </summary>
        private InterruptInformation MakeInterruptUpdateStatusVisionInfo(VisionObjectWrap a_UpdateStatusObjectWrap)
        {
            return new InterruptInformation(new InterruptFieldOfVisionData(
                HoloMonFieldOfVisionEvent.UpdateStatus,
                a_UpdateStatusObjectWrap,
                a_UpdateStatusObjectWrap.ObjectFeatureData.ObjectID
                ));
        }

        /// <summary>
        /// 距離変化検出時の割込み情報データを作成する
        /// </summary>
        private InterruptInformation MakeInterruptUpdateRangeVisionInfo(VisionObjectWrap a_UpdateStatusObjectWrap)
        {
            return new InterruptInformation(new InterruptFieldOfVisionData(
                HoloMonFieldOfVisionEvent.UpdateRange,
                a_UpdateStatusObjectWrap,
                a_UpdateStatusObjectWrap.ObjectFeatureData.ObjectID
                ));
        }


        /// <summary>
        /// 視界内オブジェクト発見状態の変化時のアクションを実行する
        /// </summary>
        private void FindFieldOfVisionObjectWrap(VisionObjectWrap a_FindObjectWrap)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptFineVisionInfo(a_FindObjectWrap);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("Vision Processed !!");
                return;
            }

            // 視認オブジェクトに対するリアクション
            FindReactionForVisionObject(a_FindObjectWrap);
        }

        /// <summary>
        /// 視界内オブジェクトロスト状態の変化時のアクションを実行する
        /// </summary>
        private void LostFieldOfVisionObjectWrap(int a_ObjectInstanceID)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptLostVisionInfo(a_ObjectInstanceID);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("Vision Processed !!");
                return;
            }
        }

        /// <summary>
        /// 視界内オブジェクト状態変化検出状態の変化時のアクションを実行する
        /// </summary>
        private void UpdateStatusFieldOfVisionObjectWrap(VisionObjectWrap a_UpdateStatusObjectWrap)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptUpdateStatusVisionInfo(a_UpdateStatusObjectWrap);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("Vision Processed !!");
                return;
            }

            // 視認オブジェクトに対するリアクション
            UpdateStatusReactionForVisionObject(a_UpdateStatusObjectWrap);
        }

        /// <summary>
        /// 視界内オブジェクト距離変化検出状態の変化時のアクションを実行する
        /// </summary>
        private void UpdateRangeFieldOfVisionObjectWrap(VisionObjectWrap a_UpdateStatusObjectWrap)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptUpdateRangeVisionInfo(a_UpdateStatusObjectWrap);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("Vision Processed !!");
                return;
            }

            // 視認オブジェクトに対するリアクション
            UpdateRangeReactionForVisionObject(a_UpdateStatusObjectWrap);
        }


        /// <summary>
        /// 視認オブジェクトの発見に対するリアクション処理
        /// </summary>
        private bool FindReactionForVisionObject(VisionObjectWrap a_VisionObjectWrap)
        {
            // リアクションを行ったか
            bool isReactioned = false;

            // オブジェクトタイプごとに処理を行う
            switch (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandType)
            {
                case ObjectUnderstandType.Learning:
                    break;
                case ObjectUnderstandType.FriendFace:
                    // プレイヤー発見時リアクション開始の判定を行う
                    isReactioned = RequestFoundPlayer();
                    break;
                case ObjectUnderstandType.FriendRightHand:
                    // 手発見時リアクション開始の判定を行う
                    isReactioned = RequestFoundHand(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.FriendLeftHand:
                    // 手発見時リアクション開始の判定を行う
                    isReactioned = RequestFoundHand(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Food:
                    // 手渡し食事開始の判定を行う
                    isReactioned = RequestGiveHandFood(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Ball:
                    // 手渡しボール遊び開始の判定を行う
                    isReactioned = RequestGiveHandBall(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Poop:
                    // うんちからの逃走の判定を行う
                    isReactioned = RequestRunFromPoop(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Jewel:
                    // ジュエル注目開始の判定を行う
                    isReactioned = RequestLookJewel();
                    break;
                case ObjectUnderstandType.ShowerHead:
                    break;
                case ObjectUnderstandType.ShowerWater:
                    // シャワー水からの逃走の判定を行う
                    isReactioned = RequestRunFromShowerWater(a_VisionObjectWrap);
                    break;
                default:
                    break;
            }

            return isReactioned;
        }

        /// <summary>
        /// 視認オブジェクトの状態変化に対するリアクション処理
        /// </summary>
        private bool UpdateStatusReactionForVisionObject(VisionObjectWrap a_VisionObjectWrap)
        {
            // リアクションを行ったか
            bool isReactioned = false;

            // オブジェクトタイプごとに処理を行う
            switch (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandType)
            {
                case ObjectUnderstandType.Learning:
                    break;
                case ObjectUnderstandType.FriendFace:
                    break;
                case ObjectUnderstandType.FriendRightHand:
                    break;
                case ObjectUnderstandType.FriendLeftHand:
                    break;
                case ObjectUnderstandType.Food:
                    // 手渡し食事開始の判定を行う
                    isReactioned = RequestGiveHandFood(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Ball:
                    // 手渡しボール遊び開始の判定を行う
                    isReactioned = RequestGiveHandBall(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Poop:
                    // うんちからの逃走の判定を行う
                    isReactioned = RequestRunFromPoop(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Jewel:
                    break;
                case ObjectUnderstandType.ShowerHead:
                    break;
                case ObjectUnderstandType.ShowerWater:
                    // シャワー水からの逃走の判定を行う
                    isReactioned = RequestRunFromShowerWater(a_VisionObjectWrap);
                    break;
                default:
                    break;
            }

            return isReactioned;
        }

        /// <summary>
        /// 視認オブジェクトの距離変化に対するリアクション処理
        /// </summary>
        private bool UpdateRangeReactionForVisionObject(VisionObjectWrap a_VisionObjectWrap)
        {
            // リアクションを行ったか
            bool isReactioned = false;

            // オブジェクトタイプごとに処理を行う
            switch (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandType)
            {
                case ObjectUnderstandType.Learning:
                    break;
                case ObjectUnderstandType.FriendFace:
                    // プレイヤー追跡の判定を行う
                    isReactioned = RequestTrackingPlayer(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.FriendRightHand:
                    break;
                case ObjectUnderstandType.FriendLeftHand:
                    break;
                case ObjectUnderstandType.Food:
                    break;
                case ObjectUnderstandType.Ball:
                    break;
                case ObjectUnderstandType.Poop:
                    // うんちからの逃走の判定を行う
                    isReactioned = RequestRunFromPoop(a_VisionObjectWrap);
                    break;
                case ObjectUnderstandType.Jewel:
                    break;
                case ObjectUnderstandType.ShowerHead:
                    break;
                case ObjectUnderstandType.ShowerWater:
                    // シャワー水からの逃走の判定を行う
                    isReactioned = RequestRunFromShowerWater(a_VisionObjectWrap);
                    break;
                default:
                    break;
            }

            return isReactioned;
        }


        /// <summary>
        /// プレイヤー発見時リアクション開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestFoundPlayer()
        {
            return false;
        }

        /// <summary>
        /// 遠距離のプレイヤーへ追跡開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestTrackingPlayer(VisionObjectWrap a_VisionObjectWrap)
        {
            // 友達オブジェクトが遠距離に居る場合はプレイヤー追跡の行動を要求する
            bool isFar = p_Reference.View.SensationsFieldOfVisionAPI
                .CurrentVisionRangeAreaForGameObject(a_VisionObjectWrap.Object, false, false, true);
            if (isFar)
            {
                if (p_Reference.AI.RequestTrackingPlayer())
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ハンド発見時リアクション開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestFoundHand(VisionObjectWrap a_VisionObjectWrap)
        {
            // グーの手を見つけたときはじゃんけん行動を要求する
            if (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandType == ObjectUnderstandType.FriendRightHand)
            {
                if (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandFriendRightHandData.HandStatus == ObjectStatusHand.Hand_Gur)
                {
                    if (p_Reference.AI.RequestJanken())
                    {
                        // 要求が通った場合は感情のびっくりUIを表示する
                        p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                        return true;
                    }
                }
            }
            // グーの手を見つけたときはじゃんけん行動を要求する
            if (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandType == ObjectUnderstandType.FriendLeftHand)
            {
                if (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandFriendLeftHandData.HandStatus == ObjectStatusHand.Hand_Gur)
                {
                    if (p_Reference.AI.RequestJanken())
                    {
                        // 要求が通った場合は感情のびっくりUIを表示する
                        p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 手渡しの食事開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestGiveHandFood(VisionObjectWrap a_VisionObjectWrap)
        {
            // 友達が食べ物を手に持っているときは食事行動を要求する
            if (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandFoodData.ItemStatus == ObjectStatusItem.Item_PlayerHold)
            {
                if (p_Reference.AI.RequestGiveFood(a_VisionObjectWrap.ObjectFeatureData))
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 手渡しのボール遊び開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestGiveHandBall(VisionObjectWrap a_VisionObjectWrap)
        {
            // 友達がボールを手に持っているときはボール遊び行動を要求する
            if (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandBallData.ItemStatus == ObjectStatusItem.Item_PlayerHold)
            {
                if (p_Reference.AI.RequestGiveBall(a_VisionObjectWrap.ObjectFeatureData))
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ジュエル注目開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestLookJewel()
        {
            // 注目オブジェクトが見えている場合はスタンバイ状態で優先度を判定する
            if (p_Reference.AI.RequestStandby())
            {
                // 要求が通った場合は感情のはてなUIを表示する
                p_Reference.Control.VisualizeUIsEmotionPanelAPI.QuestionStartAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// うんちから逃走開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestRunFromPoop(VisionObjectWrap a_VisionObjectWrap)
        {
            bool isReactioned = false;

            // うんちがゼロ距離または近距離にあるときは条件なしに、うんちから逃走の行動を要求する
            bool isNear = p_Reference.View.SensationsFieldOfVisionAPI
                .CurrentVisionRangeAreaForGameObject(a_VisionObjectWrap.Object, true, false, false);
            if (isNear)
            {
                if (p_Reference.AI.RequestRunFromPoop())
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            else
            {
                // うんちが中距離にあるとき
                bool isMiddle = p_Reference.View.SensationsFieldOfVisionAPI
                    .CurrentVisionRangeAreaForGameObject(a_VisionObjectWrap.Object, true, false, false);
                if (isMiddle)
                {
                    // 友達がうんちを手に持っているときは、うんちから逃走の行動を要求する
                    if (a_VisionObjectWrap.CurrentFeatures().ObjectUnderstandPoopData.ItemStatus == ObjectStatusItem.Item_PlayerHold)
                    {
                        if (p_Reference.AI.RequestRunFromPoop())
                        {
                            // 要求が通った場合は感情のびっくりUIを表示する
                            p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// シャワー水から逃走開始の判定を行う
        /// </summary>
        /// <returns></returns>
        private bool RequestRunFromShowerWater(VisionObjectWrap a_VisionObjectWrap)
        {
            // シャワー水が近距離にあるときは条件なしに、シャワー水から逃走の行動を要求する
            bool isNear = p_Reference.View.SensationsFieldOfVisionAPI
                .CurrentVisionRangeAreaForGameObject(a_VisionObjectWrap.Object, true, false, false);
            if (isNear)
            {
                if (p_Reference.AI.RequestRunFromShowerWater())
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
