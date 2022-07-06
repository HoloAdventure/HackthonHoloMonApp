using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx;

using HoloMonApp.Content.XRPlatform;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Player
{
    public class HandRayTracker : MonoBehaviour
    {
        /// <summary>
        /// ハンドタイプ(左右)
        /// </summary>
        [Serializable]
        private enum HandType
        {
            Right,
            Left,
        }

        [SerializeField, Tooltip("ハンドタイプ(左右)の設定")]
        private HandType p_SettingHandType;

        [SerializeField, Tooltip("参照するハンド状態")]
        private ObjectFeatures p_HandFeatureRef;

        /// <summary>
        /// ポインターオブジェクトの参照
        /// </summary>
        [SerializeField, Tooltip("ポインターオブジェクトの参照")]
        private GameObject p_Pointer;

        /// <summary>
        /// ハンドトラッキングハンドラ
        /// </summary>
        private HandTrackingHandler p_HandTrackingHandler => HoloMonXRPlatform.Instance.HandTracking.Handler;

        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
            // 初期状態はポインターオブジェクトを無効化する
            p_Pointer.SetActive(false);

            // 手の検出時オブザーバを作成する
            IDisposable OnSourceDetectedObserver = Observable
                .FromEvent<HandTrackingSourceStateEventData>(
                    action => p_HandTrackingHandler.OnSourceDetectedAction += action,
                    action => p_HandTrackingHandler.OnSourceDetectedAction -= action
                )
                .Subscribe(eventData =>
                {
                    OnSourceDetected(eventData);
                })
                .AddTo(this);

            // 手のロスト時オブザーバを作成する
            IDisposable OnSourceLostObserver = Observable
                .FromEvent<HandTrackingSourceStateEventData>(
                    action => p_HandTrackingHandler.OnSourceLostAction += action,
                    action => p_HandTrackingHandler.OnSourceLostAction -= action
                )
                .Subscribe(eventData =>
                {
                    OnSourceLost(eventData);
                })
                .AddTo(this);

            // 手の更新時オブザーバを作成する
            IDisposable OnHandJointsUpdatedObserver = Observable
                .FromEvent<HandTrackingInputEventData>(
                    action => p_HandTrackingHandler.OnHandJointsUpdatedAction += action,
                    action => p_HandTrackingHandler.OnHandJointsUpdatedAction -= action
                )
                .Subscribe(eventData =>
                {
                    OnHandJointsUpdated(eventData);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 手の検出時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceDetected(HandTrackingSourceStateEventData eventData)
        {
            // 処理なし
        }

        /// <summary>
        /// 手のロスト時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(HandTrackingSourceStateEventData eventData)
        {
            // 処理なし
        }

        /// <summary>
        /// 手の更新時に発生するイベント(IMixedRealityHandJointHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(HandTrackingInputEventData eventData)

        {
            // ハンドオブジェクトの状態を取得する
            ObjectStatusHand handStatus = ObjectStatusHand.Nothing;
            HandTrackingHandedness handedness = HandTrackingHandedness.None;
            switch (p_SettingHandType)
            {
                case HandType.Right:
                    handStatus = p_HandFeatureRef?.DataWrap.UnderstandInformation?.ObjectUnderstandFriendRightHandData?.HandStatus ?? ObjectStatusHand.Nothing;
                    handedness = HandTrackingHandedness.Right;
                    break;
                case HandType.Left:
                    handStatus = p_HandFeatureRef?.DataWrap.UnderstandInformation?.ObjectUnderstandFriendLeftHandData?.HandStatus ?? ObjectStatusHand.Nothing;
                    handedness = HandTrackingHandedness.Left;
                    break;
                default:
                    break;
            }

            // 手の形がピストルでなければポインター座標は有効化しない
            if (handStatus != ObjectStatusHand.Hand_Pistol)
            {
                // ポインターオブジェクトを無効化する
                p_Pointer.SetActive(false);

                return;
            }

            // ポインターを取得できなければポインター座標は有効化しない
            bool result = HoloMonXRPlatform.Instance.HandTracking.Pointer.GetHandPointerTargetPose(handedness, out Pose targetPose);
            if (result == false)
            {
                // ポインターオブジェクトを無効化する
                p_Pointer.SetActive(false);

                return;
            }

            // ポインターオブジェクトを有効化する
            p_Pointer.SetActive(true);

            // ポインターの座標にポインターオブジェクトを移動する
            Vector3 handRayPosition = targetPose.position;
            p_Pointer.transform.position = handRayPosition;
        }

    }
}
/*
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

using HoloMonApp.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Player
{
    public class HandRayTracker : MonoBehaviour, IMixedRealityHandJointHandler, IMixedRealitySourceStateHandler
    {
        [SerializeField, Tooltip("参照するハンドタイプの指定")]
        private Handedness p_HandednessType;

        [SerializeField, Tooltip("参照するハンド状態")]
        private ObjectFeatures p_HandFeatureRef;

        /// <summary>
        /// ポインターオブジェクトの参照
        /// </summary>
        [SerializeField, Tooltip("ポインターオブジェクトの参照")]
        private GameObject p_Pointer;

        /// <summary>
        /// 現在参照中のハンドレイ
        /// </summary>
        private ShellHandRayPointer p_ShellHandRayPointer;


        /// <summary>
        /// 手の検出時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceDetected(SourceStateEventData eventData)
        {
            // 既に対象を検出済みの場合は処理しない
            if (p_ShellHandRayPointer != null) return;

            // 現在監視対象のポインターが存在するか
            ShellHandRayPointer handRayPointer = DetectionTargetHandRay();
            if (handRayPointer == null) return;

            // 対象が見つかった場合参照を取得しておく
            p_ShellHandRayPointer = handRayPointer;
        }

        /// <summary>
        /// 手のロスト時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(SourceStateEventData eventData)
        {
            // 既に対象を削除済みの場合は処理しない
            if (p_ShellHandRayPointer == null) return;

            // 現在監視対象のポインターが存在するか
            ShellHandRayPointer handRayPointer = DetectionTargetHandRay();
            if (handRayPointer != null) return;

            // 対象が見つからなくなっている場合ロスト処理を行う
            p_ShellHandRayPointer = null;
        }

        /// <summary>
        /// 手の更新時に発生するイベント(IMixedRealityHandJointHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)

        {
            // 監視対象のポインターが取得済みか
            if (p_ShellHandRayPointer == null) return;

            // ハンドオブジェクトの状態を取得する
            ObjectStatusHand handStatus = ObjectStatusHand.Nothing;
            switch (p_HandednessType)
            {
                case Handedness.Right:
                    handStatus = p_HandFeatureRef?.DataWrap.UnderstandInformation?.ObjectUnderstandFriendRightHandData?.HandStatus ?? ObjectStatusHand.Nothing;
                    break;
                case Handedness.Left:
                    handStatus = p_HandFeatureRef?.DataWrap.UnderstandInformation?.ObjectUnderstandFriendLeftHandData?.HandStatus ?? ObjectStatusHand.Nothing;
                    break;
                default:
                    break;
            }

            // 手の形がピストルでなければポインター座標は有効化しない
            if (handStatus != ObjectStatusHand.Hand_Pistol)
            {
                // ポインターオブジェクトを無効化する
                p_Pointer.SetActive(false);

                return;
            }

            // ポインターオブジェクトを有効化する
            p_Pointer.SetActive(true);

            // ポインターのレイキャスト座標にポインターオブジェクトを移動する
            Vector3 handRayPosition = p_ShellHandRayPointer?.BaseCursor?.Position ?? new Vector3();
            p_Pointer.transform.position = handRayPosition;
        }

        /// <summary>
        /// 有効時処理
        /// </summary>
        private void OnEnable()
        {
            // ハンドラ登録
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityHandJointHandler>(this);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealitySourceStateHandler>(this);
        }

        /// <summary>
        /// 無効時処理
        /// </summary>
        private void OnDisable()
        {
            // ハンドラ解除
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityHandJointHandler>(this);
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealitySourceStateHandler>(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            // デフォルトはポインターOFF
            p_Pointer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// MRTKのInputSystemから監視対象のハンドレイポインターを取得する
        /// </summary>
        /// <returns></returns>
        private ShellHandRayPointer DetectionTargetHandRay()
        {
            ShellHandRayPointer handRayPointer = null;

            // 現在のInputSystemに対象が存在するかチェックする
            foreach (IMixedRealityInputSource inputSource in CoreServices.InputSystem.DetectedInputSources)
            {
                foreach (IMixedRealityPointer pointer in inputSource.Pointers)
                {
                    // ハンドレイでなければ対象外
                    if (pointer.GetType() != typeof(ShellHandRayPointer)) continue;
                    // 指定のハンドタイプでなければ対象外
                    if (((ShellHandRayPointer)pointer).Handedness != p_HandednessType) continue;

                    handRayPointer = (ShellHandRayPointer)pointer;
                }
            }

            return handRayPointer;
        }
    }
}
*/