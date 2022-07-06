using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx;

using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Janken
{
    /// <summary>
    /// ジャンケンの状態種別
    /// </summary>
    public enum HandRockPaperScissorsStatus
    {
        Paper,
        Scissor,
        Rock,
        Nothing
    }

    // 現在、手の形状検知は HandObjectFeaturesSetter に集約しているため未使用
    public class HandRockPaperScissorsGesture : MonoBehaviour
    {
        /// <summary>
        /// 握り敷居値
        /// 0.35 ：エディター上のクリック動作での限界敷居値
        /// </summary>
        [SerializeField, Range(-1, 1)]
        private float grabThreshold = 0.35f;

        // Inspectorで内容を確認するため、
        // DictionaryではなくListで保持する
        /// <summary>
        /// ジェスチャー状態
        /// </summary>
        [SerializeField, Tooltip("ジェスチャー状態")]
        private List<GestureState> p_GestureStateList = new List<GestureState>();

        /// <summary>
        /// ジェスチャー構造体
        /// </summary>
        [System.Serializable]
        private struct GestureState
        {
            public uint keyId;
            public HandTrackingHandedness handedness;
            public float index;
            public float middle;
            public float ring;
            public float pinky;

            public Vector3 palmPos;
            public Quaternion palmRot;

            public bool isHandOpen;
        }

        /// <summary>
        /// ハンドトラッキングハンドラ
        /// </summary>
        private HandTrackingHandler p_HandTrackingHandler => HoloMonXRPlatform.Instance.HandTracking.Handler;

        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
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
        /// 手の更新イベント
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(HandTrackingInputEventData eventData)

        {
            // Noneイベントは処理しない
            if (eventData.Handedness == HandTrackingHandedness.None) return;

            var handedness = eventData.Handedness;

            var joint = eventData.InputData;
            var id = eventData.SourceId;

            var w0 = joint[HandTrackingHandJoint.Wrist].Position;
            var i1 = joint[HandTrackingHandJoint.IndexKnuckle].Position;
            var i2 = joint[HandTrackingHandJoint.IndexTip].Position;
            var m1 = joint[HandTrackingHandJoint.MiddleKnuckle].Position;
            var m2 = joint[HandTrackingHandJoint.MiddleTip].Position;
            var r1 = joint[HandTrackingHandJoint.RingKnuckle].Position;
            var r2 = joint[HandTrackingHandJoint.RingTip].Position;
            var p1 = joint[HandTrackingHandJoint.PinkyKnuckle].Position;
            var p2 = joint[HandTrackingHandJoint.PinkyTip].Position;

            var di = Vector3.Dot((i2 - i1).normalized, (i1 - w0).normalized);
            var dm = Vector3.Dot((m2 - m1).normalized, (m1 - w0).normalized);
            var dr = Vector3.Dot((r2 - r1).normalized, (r1 - w0).normalized);
            var dp = Vector3.Dot((p2 - p1).normalized, (p1 - w0).normalized);


            GestureState gs;
            Vector3 lastPalmPos;
            Quaternion lastPalmRot;

            int gestureIndex = this.GetGestureStatusIndex(id);
            if (gestureIndex >= 0)
            {
                gs = p_GestureStateList[gestureIndex];
                gs.handedness = handedness;
                gs.index = Mathf.Lerp(gs.index, di, 0.3f);
                gs.middle = Mathf.Lerp(gs.middle, dm, 0.3f);
                gs.ring = Mathf.Lerp(gs.ring, dr, 0.3f);
                gs.pinky = Mathf.Lerp(gs.pinky, dp, 0.3f);

                // Check
                lastPalmPos = gs.palmPos;
                lastPalmRot = gs.palmRot;

                gs.palmPos = Vector3.Lerp(gs.palmPos, joint[HandTrackingHandJoint.Palm].Position, 0.5f);
                gs.palmRot = Quaternion.Lerp(gs.palmRot, joint[HandTrackingHandJoint.Palm].Rotation, 0.5f);

                // ハンドオープンジェスチャ判定
                gs.isHandOpen = isHandOpen(gs);

                p_GestureStateList[gestureIndex] = gs;
            }
            else
            {
                gs = new GestureState()
                {
                    keyId = id,
                    handedness = handedness,
                    index = di,
                    middle = dm,
                    ring = dr,
                    pinky = dp,
                    palmPos = joint[HandTrackingHandJoint.Palm].Position,
                    palmRot = joint[HandTrackingHandJoint.Palm].Rotation,
                    isHandOpen = false
                };

                lastPalmPos = gs.palmPos;
                lastPalmRot = gs.palmRot;

                // ハンドオープンジェスチャ判定
                gs.isHandOpen = isHandOpen(gs);

                p_GestureStateList.Add(gs);
            }
        }

        /// <summary>
        /// 手の検出時呼び出し関数
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceDetected(HandTrackingSourceStateEventData eventData)
        {
            // DetectedイベントはHandJointUpdated内で判定する。
        }

        /// <summary>
        /// 手のロスト時呼び出し関数
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(HandTrackingSourceStateEventData eventData)
        {
            var id = eventData.SourceId;

            int gestureIndex = this.GetGestureStatusIndex(id);
            if (gestureIndex >= 0)
            {
                p_GestureStateList.RemoveAt(gestureIndex);
            }
        }


        /// <summary>
        /// 指定キーのジェスチャー情報のインデックスを取得する
        /// キーが一致しない場合は -1 を返す
        /// </summary>
        /// <param name="a_KeyId"></param>
        /// <returns></returns>
        private int GetGestureStatusIndex(uint a_KeyId)
        {
            int index = -1;
            for (int loop = 0; loop < p_GestureStateList.Count; loop++)
            {
                if (p_GestureStateList[loop].keyId == a_KeyId)
                {
                    index = loop;
                    break;
                }
            }
            return index;
        }


        /// <summary>
        /// ハンドトラッキング中か否かの取得
        /// </summary>
        /// <returns></returns>
        public bool isHandTracking()
        {
            bool isHandTracking = false;
            foreach (GestureState gs in p_GestureStateList)
            {
                isHandTracking = true;
            }
            return isHandTracking;
        }

        /// <summary>
        /// 手を開いている（パー）か否か
        /// </summary>
        /// <returns></returns>
        public bool IsHandOpen()
        {
            bool isHandOpen = false;
            foreach (GestureState gs in p_GestureStateList)
            {
                if (gs.isHandOpen)
                {
                    isHandOpen = true;
                    break;
                }
            }
            return isHandOpen;
        }

        /// <summary>
        /// 手がグーチョキパーのいずれかの状態か
        /// </summary>
        /// <returns></returns>
        public HandRockPaperScissorsStatus IsRockPaperScissorsStatus()
        {
            // グーチョキパーの判定結果
            HandRockPaperScissorsStatus handRockPaperScissorsStatus = HandRockPaperScissorsStatus.Nothing;

            foreach (GestureState gs in p_GestureStateList)
            {
                handRockPaperScissorsStatus = isRockPaperScissorsStatus(gs);
                // 右手優先
                if (gs.handedness == HandTrackingHandedness.Right)
                {
                    break;
                }
            }
            return handRockPaperScissorsStatus;
        }

        /// <summary>
        /// 現在の手の位置を取得する
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCurrentHandPosition()
        {
            Vector3 retPos = new Vector3();
            foreach (GestureState gs in p_GestureStateList)
            {
                if (gs.isHandOpen)
                {
                    retPos = gs.palmPos;
                    break;
                }
            }
            return retPos;
        }

        /// <summary>
        /// 現在の手の回転を取得する
        /// </summary>
        /// <returns></returns>
        public Quaternion GetCurrentHandRotation()
        {
            Quaternion retRot = new Quaternion();
            foreach (GestureState gs in p_GestureStateList)
            {
                if (gs.isHandOpen)
                {
                    retRot = gs.palmRot;
                    break;
                }
            }
            return retRot;
        }

        /// <summary>
        /// 手を開いている（パー）か判定する
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        private bool isHandOpen(GestureState gs)
        {
            // 人差し指が開いているか
            bool isIndexOpen = (gs.index > grabThreshold);
            // 中指が開いているか
            bool isMiddleOpen = (gs.middle > grabThreshold);
            // 薬指が開いているか
            bool isRingOpen = (gs.ring > grabThreshold);
            // 小指が開いているか
            bool isPinkyOpen = (gs.pinky > grabThreshold);

            // 開いている指の数を数える
            int count = 0;
            if (isIndexOpen) count++;
            if (isMiddleOpen) count++;
            if (isRingOpen) count++;
            if (isPinkyOpen) count++;

            // 3本以上の指が開いていれば手を開いている（パー）と判定する
            bool isHandOpen = (count >= 3);
#if UNITY_EDITOR
            // エディターの場合は人差し指が伸びていれば手を開いている（パー）と判定する
            isHandOpen = isIndexOpen;
#endif
            return isHandOpen;
        }

        /// <summary>
        /// 手の状態がグーチョキパーのいずれかを判定する
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        private HandRockPaperScissorsStatus isRockPaperScissorsStatus(GestureState gs)
        {
            // グーチョキパーの判定結果
            HandRockPaperScissorsStatus status = HandRockPaperScissorsStatus.Nothing;

            // 人差し指が開いているか
            bool isIndexOpen = (gs.index > grabThreshold);
            // 中指が開いているか
            bool isMiddleOpen = (gs.middle > grabThreshold);
            // 薬指が開いているか
            bool isRingOpen = (gs.ring > grabThreshold);
            // 小指が開いているか
            bool isPinkyOpen = (gs.pinky > grabThreshold);

            // 開いている指の数を数える
            int count = 0;
            if (isIndexOpen) count++;
            if (isMiddleOpen) count++;
            if (isRingOpen) count++;
            if (isPinkyOpen) count++;

            if (isIndexOpen && isMiddleOpen)
            {
                if (count <= 3)
                {
                    // 人差し指と中指が開いていて、かつ、開いている指が3本以下ならチョキと判定する
                    status = HandRockPaperScissorsStatus.Scissor;
                }
                else
                {
                    // 開いている指が4本ならパーと判定する
                    status = HandRockPaperScissorsStatus.Paper;
                }
            }
            else
            {
                if (count <= 2)
                {
                    // 人差し指か中指がいずれか閉じていて、かつ、開いている指が2本以下ならグーと判定する
                    status = HandRockPaperScissorsStatus.Rock;
                }
                else
                {
                    // 人差し指か中指がいずれか閉じていて、かつ、開いている指が3本ならパーと判定する
                    status = HandRockPaperScissorsStatus.Paper;
                }
            }

            return status;
        }

    }
}
/*
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace HoloMonApp.Character.Behave.ModeLogic.Janken
{
    /// <summary>
    /// ジャンケンの状態種別
    /// </summary>
    public enum HandRockPaperScissorsStatus
    {
        Paper,
        Scissor,
        Rock,
        Nothing
    }

    public class HandRockPaperScissorsGesture : MonoBehaviour, IMixedRealityHandJointHandler, IMixedRealitySourceStateHandler
    {
        /// <summary>
        /// 握り敷居値
        /// 0.35 ：エディター上のクリック動作での限界敷居値
        /// </summary>
        [SerializeField, Range(-1, 1)]
        private float grabThreshold = 0.35f;

        // Inspectorで内容を確認するため、
        // DictionaryではなくListで保持する
        /// <summary>
        /// ジェスチャー状態
        /// </summary>
        [SerializeField, Tooltip("ジェスチャー状態")]
        private List<GestureState> p_GestureStateList;

        /// <summary>
        /// ジェスチャー構造体
        /// </summary>
        [System.Serializable]
        private struct GestureState
        {
            public uint keyId;
            public Handedness handedness;
            public float index;
            public float middle;
            public float ring;
            public float pinky;
            
            public Vector3 palmPos;
            public Quaternion palmRot;

            public bool isHandOpen;
        }

        /// <summary>
        /// 指定キーのジェスチャー情報のインデックスを取得する
        /// キーが一致しない場合は -1 を返す
        /// </summary>
        /// <param name="a_KeyId"></param>
        /// <returns></returns>
        private int GetGestureStatusIndex(uint a_KeyId)
        {
            int index = -1;
            for (int loop = 0; loop < p_GestureStateList.Count; loop++)
            {
                if (p_GestureStateList[loop].keyId == a_KeyId)
                {
                    index = loop;
                    break;
                }
            }
            return index;
        }


        /// <summary>
        /// 起動時処理
        /// </summary>
        private void Awake()
        {
            p_GestureStateList = new List<GestureState>();
        }

        /// <summary>
        /// 起動時処理
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 有効時処理
        /// </summary>
        private void OnEnable()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityHandJointHandler>(this);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealitySourceStateHandler>(this);
        }

        /// <summary>
        /// 無効時処理
        /// </summary>
        private void OnDisable()
        {
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityHandJointHandler>(this);
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealitySourceStateHandler>(this);
        }


        /// <summary>
        /// ハンドトラッキング中か否かの取得
        /// </summary>
        /// <returns></returns>
        public bool isHandTracking()
        {
            bool isHandTracking = false;
            foreach (GestureState gs in p_GestureStateList)
            {
                isHandTracking = true;
            }
            return isHandTracking;
        }

        /// <summary>
        /// 手を開いている（パー）か否か
        /// </summary>
        /// <returns></returns>
        public bool IsHandOpen()
        {
            bool isHandOpen = false;
            foreach (GestureState gs in p_GestureStateList)
            {
                if (gs.isHandOpen)
                {
                    isHandOpen = true;
                    break;
                }
            }
            return isHandOpen;
        }

        /// <summary>
        /// 手がグーチョキパーのいずれかの状態か
        /// </summary>
        /// <returns></returns>
        public HandRockPaperScissorsStatus IsRockPaperScissorsStatus()
        {
            // グーチョキパーの判定結果
            HandRockPaperScissorsStatus handRockPaperScissorsStatus = HandRockPaperScissorsStatus.Nothing;

            foreach (GestureState gs in p_GestureStateList)
            {
                handRockPaperScissorsStatus = isRockPaperScissorsStatus(gs);
                // 右手優先
                if (gs.handedness == Handedness.Right)
                {
                    break;
                }
            }
            return handRockPaperScissorsStatus;
        }

        /// <summary>
        /// 現在の手の位置を取得する
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCurrentHandPosition()
        {
            Vector3 retPos = new Vector3();
            foreach (GestureState gs in p_GestureStateList)
            {
                if(gs.isHandOpen)
                {
                    retPos = gs.palmPos;
                    break;
                }
            }
            return retPos;
        }

        /// <summary>
        /// 現在の手の回転を取得する
        /// </summary>
        /// <returns></returns>
        public Quaternion GetCurrentHandRotation()
        {
            Quaternion retRot = new Quaternion();
            foreach (GestureState gs in p_GestureStateList)
            {
                if (gs.isHandOpen)
                {
                    retRot = gs.palmRot;
                    break;
                }
            }
            return retRot;
        }

        /// <summary>
        /// 手の更新イベント
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)

        {
            // Noneイベントは処理しない
            if (eventData.Handedness == Handedness.None) return;

            var handedness = eventData.Handedness;

            var joint = eventData.InputData;
            var id = eventData.SourceId;

            var w0 = joint[TrackedHandJoint.Wrist].Position;
            var i1 = joint[TrackedHandJoint.IndexKnuckle].Position;
            var i2 = joint[TrackedHandJoint.IndexTip].Position;
            var m1 = joint[TrackedHandJoint.MiddleKnuckle].Position;
            var m2 = joint[TrackedHandJoint.MiddleTip].Position;
            var r1 = joint[TrackedHandJoint.RingKnuckle].Position;
            var r2 = joint[TrackedHandJoint.RingTip].Position;
            var p1 = joint[TrackedHandJoint.PinkyKnuckle].Position;
            var p2 = joint[TrackedHandJoint.PinkyTip].Position;

            var di = Vector3.Dot((i2 - i1).normalized, (i1 - w0).normalized);
            var dm = Vector3.Dot((m2 - m1).normalized, (m1 - w0).normalized);
            var dr = Vector3.Dot((r2 - r1).normalized, (r1 - w0).normalized);
            var dp = Vector3.Dot((p2 - p1).normalized, (p1 - w0).normalized);


            GestureState gs;
            Vector3 lastPalmPos;
            Quaternion lastPalmRot;

            int gestureIndex = this.GetGestureStatusIndex(id);
            if (gestureIndex >= 0)
            {
                gs = p_GestureStateList[gestureIndex];
                gs.handedness = handedness;
                gs.index = Mathf.Lerp(gs.index, di, 0.3f);
                gs.middle = Mathf.Lerp(gs.middle, dm, 0.3f);
                gs.ring = Mathf.Lerp(gs.ring, dr, 0.3f);
                gs.pinky = Mathf.Lerp(gs.pinky, dp, 0.3f);

                // Check
                lastPalmPos = gs.palmPos;
                lastPalmRot = gs.palmRot;

                gs.palmPos = Vector3.Lerp(gs.palmPos, joint[TrackedHandJoint.Palm].Position, 0.5f);
                gs.palmRot = Quaternion.Lerp(gs.palmRot, joint[TrackedHandJoint.Palm].Rotation, 0.5f);

                // ハンドオープンジェスチャ判定
                gs.isHandOpen = isHandOpen(gs);

                p_GestureStateList[gestureIndex] = gs;
            }
            else
            {
                gs = new GestureState()
                {
                    keyId = id,
                    handedness = handedness,
                    index = di,
                    middle = dm,
                    ring = dr,
                    pinky = dp,
                    palmPos = joint[TrackedHandJoint.Palm].Position,
                    palmRot = joint[TrackedHandJoint.Palm].Rotation,
                    isHandOpen = false
                };

                lastPalmPos = gs.palmPos;
                lastPalmRot = gs.palmRot;

                // ハンドオープンジェスチャ判定
                gs.isHandOpen = isHandOpen(gs);

                p_GestureStateList.Add(gs);
            }
        }

        /// <summary>
        /// 手の検出時呼び出し関数
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceDetected(SourceStateEventData eventData)
        {
            // DetectedイベントはHandJointUpdated内で判定する。
        }

        /// <summary>
        /// 手のロスト時呼び出し関数
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(SourceStateEventData eventData)
        {
            var id = eventData.SourceId;

            int gestureIndex = this.GetGestureStatusIndex(id);
            if (gestureIndex >= 0)
            {
                p_GestureStateList.RemoveAt(gestureIndex);
            }
        }

        /// <summary>
        /// 手を開いている（パー）か判定する
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        private bool isHandOpen(GestureState gs)
        {
            // 人差し指が開いているか
            bool isIndexOpen = (gs.index > grabThreshold);
            // 中指が開いているか
            bool isMiddleOpen = (gs.middle > grabThreshold);
            // 薬指が開いているか
            bool isRingOpen = (gs.ring > grabThreshold);
            // 小指が開いているか
            bool isPinkyOpen = (gs.pinky > grabThreshold);

            // 開いている指の数を数える
            int count = 0;
            if (isIndexOpen) count++;
            if (isMiddleOpen) count++;
            if (isRingOpen) count++;
            if (isPinkyOpen) count++;

            // 3本以上の指が開いていれば手を開いている（パー）と判定する
            bool isHandOpen = (count >= 3);
#if UNITY_EDITOR
            // エディターの場合は人差し指が伸びていれば手を開いている（パー）と判定する
            isHandOpen = isIndexOpen;
#endif
            return isHandOpen;
        }

        /// <summary>
        /// 手の状態がグーチョキパーのいずれかを判定する
        /// </summary>
        /// <param name="gs"></param>
        /// <returns></returns>
        private HandRockPaperScissorsStatus isRockPaperScissorsStatus(GestureState gs)
        {
            // グーチョキパーの判定結果
            HandRockPaperScissorsStatus status = HandRockPaperScissorsStatus.Nothing;

            // 人差し指が開いているか
            bool isIndexOpen = (gs.index > grabThreshold);
            // 中指が開いているか
            bool isMiddleOpen = (gs.middle > grabThreshold);
            // 薬指が開いているか
            bool isRingOpen = (gs.ring > grabThreshold);
            // 小指が開いているか
            bool isPinkyOpen = (gs.pinky > grabThreshold);

            // 開いている指の数を数える
            int count = 0;
            if (isIndexOpen) count++;
            if (isMiddleOpen) count++;
            if (isRingOpen) count++;
            if (isPinkyOpen) count++;

            if (isIndexOpen && isMiddleOpen)
            {
                if (count <= 3)
                {
                    // 人差し指と中指が開いていて、かつ、開いている指が3本以下ならチョキと判定する
                    status = HandRockPaperScissorsStatus.Scissor;
                }
                else
                {
                    // 開いている指が4本ならパーと判定する
                    status = HandRockPaperScissorsStatus.Paper;
                }
            }
            else
            {
                if (count <= 2)
                {
                    // 人差し指か中指がいずれか閉じていて、かつ、開いている指が2本以下ならグーと判定する
                    status = HandRockPaperScissorsStatus.Rock;
                }
                else
                {
                    // 人差し指か中指がいずれか閉じていて、かつ、開いている指が3本ならパーと判定する
                    status = HandRockPaperScissorsStatus.Paper;
                }
            }

            return status;
        }

    }
}
*/