using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx;

using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.Player
{
    public class HandTracker : MonoBehaviour
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

        [SerializeField, Tooltip("ハンドオブジェクトの参照")]
        private GameObject p_HeadObject;

        [SerializeField, Tooltip("現在の手のソースID")]
        private uint p_CurrentHandSourceId;

        /// <summary>
        /// ハンドトラッキングハンドラ
        /// </summary>
        private HandTrackingHandler p_HandTrackingHandler => HoloMonXRPlatform.Instance.HandTracking.Handler;

        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
            // 初期化時はハンドオブジェクトを無効化しておく
            HandLost();

            // 手の検出時オブザーバを作成する
            IDisposable OnSourceDetectedObserver = Observable
                .FromEvent<HandTrackingSourceStateEventData>(
                    action => p_HandTrackingHandler.OnSourceDetectedAction += action,
                    action => p_HandTrackingHandler.OnSourceDetectedAction -= action
                )
                .Subscribe(eventData =>
                {
                    // 右手/左手判定が必要なため、検出処理はHandJointUpdatedで行う
                })
                .AddTo(this);

            // 手のロスト時オブザーバを作成する
            IDisposable OnSourceLostObserver = Observable
                .FromEvent<HandTrackingSourceStateEventData>(
                    action => p_HandTrackingHandler.OnSourceLostAction += action,
                    action => p_HandTrackingHandler.OnSourceLostAction -= action
                )
                .Where(eventData => eventData.SourceId == p_CurrentHandSourceId) // 現在のソースIDがロストした場合
                .Subscribe(eventData =>
                {
                    // ハンドオブジェクトのロスト処理を行う
                    HandLost();
                })
                .AddTo(this);

            // 手の更新時オブザーバを作成する
            IDisposable OnHandJointsUpdatedObserver = Observable
                .FromEvent<HandTrackingInputEventData>(
                    action => p_HandTrackingHandler.OnHandJointsUpdatedAction += action,
                    action => p_HandTrackingHandler.OnHandJointsUpdatedAction -= action
                )
                .Where(eventData => eventData.Handedness == GetCurrentHandednessType()) // チェック対象(右手/左手)である場合
                .Subscribe(eventData =>
                {
                    // ハンドオブジェクトの追跡処理を行う
                    HandTracking(eventData);
                })
                .AddTo(this);
        }


        /// <summary>
        /// チェックすべき Handedness タイプを返す
        /// </summary>
        /// <returns></returns>
        private HandTrackingHandedness GetCurrentHandednessType()
        {
            HandTrackingHandedness handedness = HandTrackingHandedness.None;
            switch (p_SettingHandType)
            {
                case HandType.Right:
                    handedness = HandTrackingHandedness.Right;
                    break;
                case HandType.Left:
                    handedness = HandTrackingHandedness.Left;
                    break;
                default:
                    break;
            }
            return handedness;
        }


        /// <summary>
        /// ハンドオブジェクトの追跡処理を行う
        /// </summary>
        private void HandTracking(HandTrackingInputEventData eventData)
        {
            // 中指の第一関節部分の座標と回転を取得する
            IDictionary<HandTrackingHandJoint, HandTrackingJointPose> joint = eventData.InputData;
            Vector3 handPosition = joint[HandTrackingHandJoint.MiddleKnuckle].Position;
            Quaternion handRotation = joint[HandTrackingHandJoint.MiddleKnuckle].Rotation;

            if (!p_HeadObject.activeSelf)
            {
                // ハンドオブジェクトが無効化されている場合は有効化してIDを記録する
                p_HeadObject.SetActive(true);
                p_CurrentHandSourceId = eventData.SourceId;

                // 座標と回転を設定する
                p_HeadObject.transform.position = handPosition;
                p_HeadObject.transform.rotation = handRotation;
            }
            else
            {
                // ハンドオブジェクトが既に有効化されている場合は
                // Lerp を使ってスムーズに座標と回転を変化させる
                p_HeadObject.transform.position = Vector3.Lerp(p_HeadObject.transform.position, handPosition, 0.5f);
                p_HeadObject.transform.rotation = Quaternion.Lerp(p_HeadObject.transform.rotation, handRotation, 0.5f);
            }
        }

        /// <summary>
        /// ハンドオブジェクトのロスト処理を行う
        /// </summary>
        private void HandLost()
        {
            // ハンドオブジェクトを無効化する
            p_HeadObject.SetActive(false);
            p_CurrentHandSourceId = 0;
        }
    }
}

/*
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

using UniRx;

namespace HoloMonApp.Player
{
    public class HandTracker : MonoBehaviour, IMixedRealitySourceStateHandler, IMixedRealityHandJointHandler
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

        [SerializeField, Tooltip("ハンドオブジェクトの参照")]
        private GameObject p_HeadObject;

        [SerializeField, Tooltip("現在の手のソースID")]
        private uint p_CurrentHandSourceId;


        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
            // 初期化時はハンドオブジェクトを無効化しておく
            HandLost();

            // 手の検出時オブザーバを作成する
            IDisposable OnSourceDetectedObserver = Observable
                .FromEvent<SourceStateEventData>(
                    action => OnSourceDetectedAction += action,
                    action => OnSourceDetectedAction -= action
                )
                .Subscribe(eventData =>
                {
                    // 右手/左手判定が必要なため、検出処理はHandJointUpdatedで行う
                })
                .AddTo(this);

            // 手のロスト時オブザーバを作成する
            IDisposable OnSourceLostObserver = Observable
                .FromEvent<SourceStateEventData>(
                    action => OnSourceLostAction += action,
                    action => OnSourceLostAction -= action
                )
                .Where(eventData => eventData.SourceId == p_CurrentHandSourceId) // 現在のソースIDがロストした場合
                .Subscribe(eventData =>
                {
                    // ハンドオブジェクトのロスト処理を行う
                    HandLost();
                })
                .AddTo(this);

            // 手の更新時オブザーバを作成する
            IDisposable OnHandJointsUpdatedObserver = Observable
                .FromEvent<InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>>>(
                    action => OnHandJointsUpdatedAction += action,
                    action => OnHandJointsUpdatedAction -= action
                )
                .Where(eventData => eventData.Handedness == GetCurrentHandednessType()) // チェック対象(右手/左手)である場合
                .Subscribe(eventData =>
                {
                    // ハンドオブジェクトの追跡処理を行う
                    HandTracking(eventData);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
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


        /// <summary>
        /// チェックすべき Handedness タイプを返す
        /// </summary>
        /// <returns></returns>
        private Handedness GetCurrentHandednessType()
        {
            Handedness handedness = Handedness.None;
            switch (p_SettingHandType)
            {
                case HandType.Right:
                    handedness = Handedness.Right;
                    break;
                case HandType.Left:
                    handedness = Handedness.Left;
                    break;
                default:
                    break;
            }
            return handedness;
        }


        /// <summary>
        /// ハンドオブジェクトの追跡処理を行う
        /// </summary>
        private void HandTracking(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
        {
            // 中指の第一関節部分の座標と回転を取得する
            IDictionary<TrackedHandJoint, MixedRealityPose> joint = eventData.InputData;
            Vector3 handPosition = joint[TrackedHandJoint.MiddleKnuckle].Position;
            Quaternion handRotation = joint[TrackedHandJoint.MiddleKnuckle].Rotation;

            if (!p_HeadObject.activeSelf)
            {
                // ハンドオブジェクトが無効化されている場合は有効化してIDを記録する
                p_HeadObject.SetActive(true);
                p_CurrentHandSourceId = eventData.SourceId;

                // 座標と回転を設定する
                p_HeadObject.transform.position = handPosition;
                p_HeadObject.transform.rotation = handRotation;
            }
            else
            {
                // ハンドオブジェクトが既に有効化されている場合は
                // Lerp を使ってスムーズに座標と回転を変化させる
                p_HeadObject.transform.position = Vector3.Lerp(p_HeadObject.transform.position, handPosition, 0.5f);
                p_HeadObject.transform.rotation = Quaternion.Lerp(p_HeadObject.transform.rotation, handRotation, 0.5f);
            }
        }

        /// <summary>
        /// ハンドオブジェクトのロスト処理を行う
        /// </summary>
        private void HandLost()
        {
            // ハンドオブジェクトを無効化する
            p_HeadObject.SetActive(false);
            p_CurrentHandSourceId = 0;
        }



        // 手の検出時に呼び出すアクション
        private Action<SourceStateEventData> OnSourceDetectedAction;

        // 手のロスト時に呼び出すアクション
        private Action<SourceStateEventData> OnSourceLostAction;

        // 手の更新時に呼び出すアクション
        private Action<InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>>> OnHandJointsUpdatedAction;

        /// <summary>
        /// 手の検出時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceDetected(SourceStateEventData eventData)
        {
            // アクション呼び出し
            OnSourceDetectedAction?.Invoke(eventData);
        }

        /// <summary>
        /// 手のロスト時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(SourceStateEventData eventData)
        {
            // アクション呼び出し
            OnSourceLostAction?.Invoke(eventData);
        }

        /// <summary>
        /// 手の更新時に発生するイベント(IMixedRealityHandJointHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)

        {
            // アクション呼び出し
            OnHandJointsUpdatedAction?.Invoke(eventData);
        }
    }
}
*/