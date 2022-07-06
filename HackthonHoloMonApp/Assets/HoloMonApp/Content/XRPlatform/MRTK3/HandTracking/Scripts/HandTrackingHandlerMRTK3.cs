#if XRPLATFORM_MRTK3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx;
using UniRx.Triggers;

using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;
using Microsoft.MixedReality.Toolkit.Input;

namespace HoloMonApp.Content.XRPlatform.MRTK3
{
    public class HandTrackingHandlerMRTK3
    {
        private HandsAggregatorSubsystem p_Aggregator;

        private IDictionary<XRNode, bool> prevAllJointsAreValid;

        private IDictionary<HandTrackingHandJoint, HandTrackingJointPose> p_ConvertHandJointPoseDictionary;

        public HandTrackingHandlerMRTK3(Component a_Component)
        {
            p_ConvertHandJointPoseDictionary = CreateHandJointPoseDictionary();

            p_Aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();

            prevAllJointsAreValid = new Dictionary<XRNode, bool>();

            a_Component.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    CheckUpdate();
                })
                .AddTo(a_Component);
        }


        // 手の検出時に呼び出すアクション
        public Action<HandTrackingSourceStateEventData> OnSourceDetectedAction;

        // 手のロスト時に呼び出すアクション
        public Action<HandTrackingSourceStateEventData> OnSourceLostAction;

        // 手の更新時に呼び出すアクション
        public Action<HandTrackingInputEventData> OnHandJointsUpdatedAction;


        #region "privete"
        private void CheckUpdate()
        {
            if (p_Aggregator == null) return;
            HandTrackingEventHandle(XRNode.RightHand);
            HandTrackingEventHandle(XRNode.LeftHand);
        }

        private void HandTrackingEventHandle(XRNode a_XRNode)
        {
            if (!prevAllJointsAreValid.ContainsKey(a_XRNode)) prevAllJointsAreValid[a_XRNode] = false;

            // 指定の手のハンドトラッキングを調べる
            bool allJointsAreValid = p_Aggregator.TryGetEntireHand(a_XRNode, out IReadOnlyList<HandJointPose> jointList);

            // IDを取得する
            uint sourceId = (uint)(XRNodeExtensions.ToHandedness(a_XRNode));

            // ハンド種別を取得する
            HandTrackingHandedness handedness = ConvertHandnessMRTK3(XRNodeExtensions.ToHandedness(a_XRNode));

            // ジョイントリストをDictionary型に変換する
            p_ConvertHandJointPoseDictionary = ConvertHandJointPosesMRTK3(jointList, p_ConvertHandJointPoseDictionary);

            // イベント実行
            if (allJointsAreValid && !prevAllJointsAreValid[a_XRNode])
            {
                // 検出
                HandTrackingSourceStateEventData data = new HandTrackingSourceStateEventData(DateTime.Now, sourceId);

                // アクション呼び出し
                OnSourceDetectedAction?.Invoke(data);
            }
            else if (!allJointsAreValid && prevAllJointsAreValid[a_XRNode])
            {
                // ロスト
                HandTrackingSourceStateEventData data = new HandTrackingSourceStateEventData(DateTime.Now, sourceId);

                // アクション呼び出し
                OnSourceLostAction?.Invoke(data);
            }

            if (allJointsAreValid)
            {
                // 検出中処理
                HandTrackingInputEventData data = new HandTrackingInputEventData(sourceId, handedness, p_ConvertHandJointPoseDictionary);

                // アクション呼び出し
                OnHandJointsUpdatedAction?.Invoke(data);
            }

            prevAllJointsAreValid[a_XRNode] = allJointsAreValid;
        }
        #endregion


        #region "MRTK3 private"
        private IDictionary<HandTrackingHandJoint, HandTrackingJointPose> CreateHandJointPoseDictionary()
        {
            IDictionary<HandTrackingHandJoint, HandTrackingJointPose> handJointPosesDictionary
                = new Dictionary<HandTrackingHandJoint, HandTrackingJointPose>();
            HandTrackingJointPose handJointPose = null;

            // ジョイントに対応するキー名を付与する
            for (int index = 0; index < (int)Enum.GetNames(typeof(HandTrackingHandJoint)).Length; index++)
            {
                HandTrackingHandJoint trackedHandJointKey = (HandTrackingHandJoint)index;
                handJointPosesDictionary.Add(trackedHandJointKey, handJointPose);
            }

            return handJointPosesDictionary;
        }


        private HandTrackingHandedness ConvertHandnessMRTK3(Handedness a_Handedness)
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
            }
            return convertHandedness;
        }

        private IDictionary<HandTrackingHandJoint, HandTrackingJointPose> ConvertHandJointPosesMRTK3
            (IReadOnlyList<HandJointPose> a_JointPoseList,
            IDictionary<HandTrackingHandJoint, HandTrackingJointPose> a_HandJointPosesDictionary)
        {
            // ジョイントに対応するキー名を付与する
            for (int index = 0; index < (int)TrackedHandJoint.TotalJoints; index++)
            {
                // None 識別子を無視するためインデックスより 1 インクリメントする
                // (最初に Wrist の情報が代入されている)
                TrackedHandJoint trackedHandJointKey = (TrackedHandJoint)index + 1;
                HandTrackingHandJoint convertTrackedHandJoint = ConvertTrackedHandJointMRTK3(trackedHandJointKey);
                HandTrackingJointPose convertMixedRealityPose = ConvertMixedRealityPoseMRTK3(a_JointPoseList[index]);
                a_HandJointPosesDictionary[convertTrackedHandJoint] = convertMixedRealityPose;
            }

            return a_HandJointPosesDictionary;
        }

        private HandTrackingHandJoint ConvertTrackedHandJointMRTK3(TrackedHandJoint a_TrackedHandJoint)
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

        private HandTrackingJointPose ConvertMixedRealityPoseMRTK3(HandJointPose a_HandJointPose)
        {
            HandTrackingJointPose convertMixedRealityPose = new HandTrackingJointPose(a_HandJointPose.Position, a_HandJointPose.Rotation);
            return convertMixedRealityPose;
        }
#endregion
    }
}
#endif
