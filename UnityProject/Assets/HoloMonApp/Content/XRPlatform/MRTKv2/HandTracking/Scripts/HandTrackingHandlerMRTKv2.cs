#if XRPLATFORM_MRTKV2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace HoloMonApp.Content.XRPlatform.MRTKv2
{
    public class HandTrackingHandlerMRTKv2 : IMixedRealitySourceStateHandler, IMixedRealityHandJointHandler
    {
        public HandTrackingHandlerMRTKv2()
        {
            // ハンドラ登録
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityHandJointHandler>(this);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealitySourceStateHandler>(this);
        }


        // 手の検出時に呼び出すアクション
        public Action<HandTrackingSourceStateEventData> OnSourceDetectedAction;

        // 手のロスト時に呼び出すアクション
        public Action<HandTrackingSourceStateEventData> OnSourceLostAction;

        // 手の更新時に呼び出すアクション
        public Action<HandTrackingInputEventData> OnHandJointsUpdatedAction;

        /// <summary>
        /// 手の検出時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceDetected(SourceStateEventData eventData)
        {
            HandTrackingSourceStateEventData data = new HandTrackingSourceStateEventData(eventData.EventTime, eventData.SourceId);

            // アクション呼び出し
            OnSourceDetectedAction?.Invoke(data);
        }

        /// <summary>
        /// 手のロスト時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(SourceStateEventData eventData)
        {
            HandTrackingSourceStateEventData data = new HandTrackingSourceStateEventData(eventData.EventTime, eventData.SourceId);

            // アクション呼び出し
            OnSourceLostAction?.Invoke(data);
        }

        /// <summary>
        /// 手の更新時に発生するイベント(IMixedRealityHandJointHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)

        {
            uint sourceId = eventData.SourceId;
            HandTrackingHandedness handedness = ConvertHandnessMRTKv2(eventData.Handedness);
            IDictionary<HandTrackingHandJoint, HandTrackingJointPose> handJointPoses = ConvertHandJointPosesMRTKv2(eventData.InputData);

            HandTrackingInputEventData data = new HandTrackingInputEventData(sourceId, handedness, handJointPoses);

            // アクション呼び出し
            OnHandJointsUpdatedAction?.Invoke(data);
        }

        #region "MRTKv2 private"
        private HandTrackingHandedness ConvertHandnessMRTKv2(Handedness a_Handedness)
        {
            HandTrackingHandedness convertHandedness = HandTrackingHandedness.None;
            switch (a_Handedness)
            {
                case Handedness.None:
                    break;
                case Handedness.Left:
                    convertHandedness = HandTrackingHandedness.Left;
                    break;
                case Handedness.Right:
                    convertHandedness = HandTrackingHandedness.Right;
                    break;
                case Handedness.Both:
                    convertHandedness = HandTrackingHandedness.Both;
                    break;
                case Handedness.Other:
                    convertHandedness = HandTrackingHandedness.Other;
                    break;
                case Handedness.Any:
                    convertHandedness = HandTrackingHandedness.Any;
                    break;
            }
            return convertHandedness;
        }

        private IDictionary<HandTrackingHandJoint, HandTrackingJointPose> ConvertHandJointPosesMRTKv2
            (IDictionary<TrackedHandJoint, MixedRealityPose> a_HandJointPoses)
        {
            IDictionary<HandTrackingHandJoint, HandTrackingJointPose> convertHandJointPoses =
                new Dictionary<HandTrackingHandJoint, HandTrackingJointPose>();

            foreach (TrackedHandJoint trackedHandJoint in a_HandJointPoses.Keys)
            {
                HandTrackingHandJoint convertTrackedHandJoint = ConvertTrackedHandJointMRTKv2(trackedHandJoint);
                HandTrackingJointPose convertMixedRealityPose = ConvertMixedRealityPoseMRTKv2(a_HandJointPoses[trackedHandJoint]);
                convertHandJointPoses.Add(convertTrackedHandJoint, convertMixedRealityPose);
            }

            return convertHandJointPoses;
        }

        private HandTrackingHandJoint ConvertTrackedHandJointMRTKv2(TrackedHandJoint a_TrackedHandJoint)
        {
            HandTrackingHandJoint convertTrackedHandJoint = HandTrackingHandJoint.None;
            switch (a_TrackedHandJoint)
            {
                case TrackedHandJoint.None:
                    break;
                case TrackedHandJoint.Wrist:
                    convertTrackedHandJoint = HandTrackingHandJoint.Wrist;
                    break;
                case TrackedHandJoint.Palm:
                    convertTrackedHandJoint = HandTrackingHandJoint.Palm;
                    break;
                case TrackedHandJoint.ThumbMetacarpalJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.ThumbMetacarpalJoint;
                    break;
                case TrackedHandJoint.ThumbProximalJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.ThumbProximalJoint;
                    break;
                case TrackedHandJoint.ThumbDistalJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.ThumbDistalJoint;
                    break;
                case TrackedHandJoint.ThumbTip:
                    convertTrackedHandJoint = HandTrackingHandJoint.ThumbTip;
                    break;
                case TrackedHandJoint.IndexMetacarpal:
                    convertTrackedHandJoint = HandTrackingHandJoint.IndexMetacarpal;
                    break;
                case TrackedHandJoint.IndexKnuckle:
                    convertTrackedHandJoint = HandTrackingHandJoint.IndexKnuckle;
                    break;
                case TrackedHandJoint.IndexMiddleJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.IndexMiddleJoint;
                    break;
                case TrackedHandJoint.IndexDistalJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.IndexDistalJoint;
                    break;
                case TrackedHandJoint.IndexTip:
                    convertTrackedHandJoint = HandTrackingHandJoint.IndexTip;
                    break;
                case TrackedHandJoint.MiddleMetacarpal:
                    convertTrackedHandJoint = HandTrackingHandJoint.MiddleMetacarpal;
                    break;
                case TrackedHandJoint.MiddleKnuckle:
                    convertTrackedHandJoint = HandTrackingHandJoint.MiddleKnuckle;
                    break;
                case TrackedHandJoint.MiddleMiddleJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.MiddleMiddleJoint;
                    break;
                case TrackedHandJoint.MiddleDistalJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.MiddleDistalJoint;
                    break;
                case TrackedHandJoint.MiddleTip:
                    convertTrackedHandJoint = HandTrackingHandJoint.MiddleTip;
                    break;
                case TrackedHandJoint.RingMetacarpal:
                    convertTrackedHandJoint = HandTrackingHandJoint.RingMetacarpal;
                    break;
                case TrackedHandJoint.RingKnuckle:
                    convertTrackedHandJoint = HandTrackingHandJoint.RingKnuckle;
                    break;
                case TrackedHandJoint.RingMiddleJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.RingMiddleJoint;
                    break;
                case TrackedHandJoint.RingDistalJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.RingDistalJoint;
                    break;
                case TrackedHandJoint.RingTip:
                    convertTrackedHandJoint = HandTrackingHandJoint.RingTip;
                    break;
                case TrackedHandJoint.PinkyMetacarpal:
                    convertTrackedHandJoint = HandTrackingHandJoint.PinkyMetacarpal;
                    break;
                case TrackedHandJoint.PinkyKnuckle:
                    convertTrackedHandJoint = HandTrackingHandJoint.PinkyKnuckle;
                    break;
                case TrackedHandJoint.PinkyMiddleJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.PinkyMiddleJoint;
                    break;
                case TrackedHandJoint.PinkyDistalJoint:
                    convertTrackedHandJoint = HandTrackingHandJoint.PinkyDistalJoint;
                    break;
                case TrackedHandJoint.PinkyTip:
                    convertTrackedHandJoint = HandTrackingHandJoint.PinkyTip;
                    break;
            }

            return convertTrackedHandJoint;
        }

        private HandTrackingJointPose ConvertMixedRealityPoseMRTKv2(MixedRealityPose a_MixedRealityPose)
        {
            HandTrackingJointPose convertMixedRealityPose = new HandTrackingJointPose(a_MixedRealityPose.Position, a_MixedRealityPose.Rotation);
            return convertMixedRealityPose;
        }
        #endregion
    }
}
#endif