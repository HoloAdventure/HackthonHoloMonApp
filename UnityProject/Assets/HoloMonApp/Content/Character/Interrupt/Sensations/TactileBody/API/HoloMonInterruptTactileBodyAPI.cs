using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.AI;

using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.TactileBody
{
    /// <summary>
    /// 触覚情報によって行動目的を決定する
    /// </summary>
    public class HoloMonInterruptTactileBodyAPI : MonoBehaviour, HoloMonInterruptIF
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
            p_Reference.View.SensationsTactileBodyAPI
                .ReactivePropertyFindObjectWrap
                .ObserveOnMainThread()
                .Subscribe(objectWrap =>
                {
                    if (objectWrap.Object == null) return;
                    //Debug.Log("TactileBody FindObjectWrap : objectname = " + objectWrap.CurrentName()
                    //    + ", objecttype = " + objectWrap.CurrentFeatures().ObjectUnderstandType.ToString() + ".");
                    FindTactileObjectWrap(objectWrap);
                })
                .AddTo(this);

            // 見失ったものの変化時の処理を設定する
            p_Reference.View.SensationsTactileBodyAPI
                .ReactivePropertyLostObjectInstanceID
                .ObserveOnMainThread()
                .Subscribe(objectInstanceID =>
                {
                    //Debug.Log("TactileBody LostObjectName : objectInstanceID = " + objectInstanceID + ".");
                    LostTactileObjectWrap(objectInstanceID);
                })
                .AddTo(this);

            // 状態変化に気づいたものの変化時の処理を設定する
            p_Reference.View.SensationsTactileBodyAPI
                .ReactivePropertyUpdateStatusObjectWrap
                .ObserveOnMainThread()
                .Subscribe(objectWrap =>
                {
                    if (objectWrap.Object == null) return;
                    //Debug.Log("TactileBody UpdateStatusObjectWrap : objectname = " + objectWrap.CurrentName() + ".");
                    UpdateStatusTactileObjectWrap(objectWrap);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 発見時の割込み情報データを作成する
        /// </summary>
        private InterruptInformation MakeInterruptFineTactileInfo(TactileObjectWrap a_FindObjectWrap)
        {
            return new InterruptInformation(new InterruptTactileBodyData(
                HoloMonTactileBodyEvent.Add,
                HoloMonTactileBodyLimb.Head,
                a_FindObjectWrap,
                a_FindObjectWrap.ObjectFeatureData.ObjectID
                ));
        }

        /// <summary>
        /// ロスト時の割込み情報データを作成する
        /// </summary>
        private InterruptInformation MakeInterruptLostTactileInfo(int a_ObjectInstanceID)
        {
            return new InterruptInformation(new InterruptTactileBodyData(
                HoloMonTactileBodyEvent.Lost,
                HoloMonTactileBodyLimb.Head,
                null,
                a_ObjectInstanceID
                ));
        }

        /// <summary>
        /// 状態変化検出時の割込み情報データを作成する
        /// </summary>
        private InterruptInformation MakeInterruptUpdateStatusTactileInfo(TactileObjectWrap a_UpdateStatusObjectWrap)
        {
            return new InterruptInformation(new InterruptTactileBodyData(
                HoloMonTactileBodyEvent.UpdateStatus,
                HoloMonTactileBodyLimb.Head,
                a_UpdateStatusObjectWrap,
                a_UpdateStatusObjectWrap.ObjectFeatureData.ObjectID
                ));
        }


        /// <summary>
        /// 触覚内オブジェクト発見状態の変化時のアクションを実行する
        /// </summary>
        private void FindTactileObjectWrap(TactileObjectWrap a_FindObjectWrap)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptFineTactileInfo(a_FindObjectWrap);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("TactileBody Processed !!");
                return;
            }

            // 触覚オブジェクトに対するリアクション
            ReactionForTactileObject(a_FindObjectWrap);
        }

        /// <summary>
        /// 触覚内オブジェクトロスト状態の変化時のアクションを実行する
        /// </summary>
        private void LostTactileObjectWrap(int a_ObjectInstanceID)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptLostTactileInfo(a_ObjectInstanceID);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("TactileBody Processed !!");
                return;
            }
        }

        /// <summary>
        /// 触覚内オブジェクト状態変化検出状態の変化時のアクションを実行する
        /// </summary>
        private void UpdateStatusTactileObjectWrap(TactileObjectWrap a_UpdateStatusObjectWrap)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptUpdateStatusTactileInfo(a_UpdateStatusObjectWrap);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("TactileBody Processed !!");
                return;
            }

            // 触覚オブジェクトに対するリアクション
            ReactionForTactileObject(a_UpdateStatusObjectWrap);
        }

        /// <summary>
        /// 触覚オブジェクトに対するリアクション処理
        /// </summary>
        private void ReactionForTactileObject(TactileObjectWrap a_TactileObjectWrap)
        {
            // 触れたオブジェクトに対するリアクション
            switch (a_TactileObjectWrap.CurrentFeatures().ObjectUnderstandType)
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
                    break;
                case ObjectUnderstandType.Ball:
                    break;
                case ObjectUnderstandType.Poop:
                    break;
                case ObjectUnderstandType.Jewel:
                    break;
                default:
                    break;
            }
        }
    }
}