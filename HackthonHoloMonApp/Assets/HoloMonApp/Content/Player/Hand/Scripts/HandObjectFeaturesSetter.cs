using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.XRPlatform;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Player
{
    [RequireComponent(typeof(ObjectFeatures))]
    public class HandObjectFeaturesSetter : MonoBehaviour
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

        /// <summary>
        /// 手の状態識別
        /// </summary>
        [Serializable]
        public enum HandStatus
        {
            Normal,           // 通常
            Gur,              // グー
            Choki,            // チョキ
            Par,              // パー
            Pistol,           // ピストル
            ThumbsUp,         // サムズアップ
        }

        [SerializeField, Tooltip("ハンドタイプ(左右)の設定")]
        private HandType p_SettingHandType;


        [SerializeField, Tooltip("親指の折れ曲がり判定角")]
        private float p_HoldThumbAngle = 30.0f;

        [SerializeField, Tooltip("四指の折れ曲がり判定角")]
        private float p_HoldFingerAngle = 60.0f;

        [SerializeField, Tooltip("手の状態を丸め込み判定するバッファサイズ")]
        private int p_HandStatusBufferCount = 10;


        [SerializeField, Tooltip("現在の手の状態")]
        private HandStatus p_CurrentHandStatus;

        [SerializeField, Tooltip("現在の手のソースID")]
        private uint p_CurrentHandSourceId;


        // オブジェクトの特徴の設定参照
        private ObjectFeatures p_ObjectFeatures;

        // 丸め込み用カウンタリスト
        private int[] p_RoundingCountList;

        /// <summary>
        /// ハンドトラッキングハンドラ
        /// </summary>
        private HandTrackingHandler p_HandTrackingHandler => HoloMonXRPlatform.Instance.HandTracking.Handler;

        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
            // オブジェクトの特徴の参照を取得する
            p_ObjectFeatures = this.gameObject.GetComponent<ObjectFeatures>();

            // 丸め込み用カウンタリストを作成しておく
            p_RoundingCountList = new int[Enum.GetNames(typeof(HandStatus)).Length];

            // 初期状態は通常状態
            SettingNormal();


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
                .Subscribe(eventData =>
                {
                    // ロスト時は設定をノーマルに初期化する
                    SettingNormal();
                })
                .AddTo(this);

            // 手の更新時オブザーバを作成する
            IDisposable OnHandJointsUpdatedObserver = Observable
                .FromEvent<HandTrackingInputEventData>(
                    action => p_HandTrackingHandler.OnHandJointsUpdatedAction += action,
                    action => p_HandTrackingHandler.OnHandJointsUpdatedAction -= action
                )
                .Where(eventData => eventData.Handedness == GetCurrentHandednessType()) // チェック対象(右手/左手)である場合
                .Select(eventData => CheckCurrentHandStatus(eventData))                 // 手の状態を判定する
                .Buffer(p_HandStatusBufferCount)                                        // 判定結果をバッファする
                .Select(eventDataList => this.RoundingHandStatus(eventDataList))        // バッファした結果から最頻出の状態を取得する
                .Subscribe(status =>
                {
                    // 状態を設定する
                    SettingHandStatus(status);
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
        /// 現在の手の状態を判定する
        /// </summary>
        /// <returns></returns>
        private HandStatus CheckCurrentHandStatus(HandTrackingInputEventData eventData)
        {
            // ソースIDを取得する
            p_CurrentHandSourceId = eventData.SourceId;

            // 各指のジョイント位置を取得する
            IDictionary<HandTrackingHandJoint, HandTrackingJointPose> joint = eventData.InputData;
            Vector3 wrist_0 = joint[HandTrackingHandJoint.Wrist].Position;                // 手首
            Vector3 thumb_0 = joint[HandTrackingHandJoint.ThumbMetacarpalJoint].Position; // 親指手首
            Vector3 thumb_1 = joint[HandTrackingHandJoint.ThumbProximalJoint].Position;   // 親指第一関節
            Vector3 thumb_2 = joint[HandTrackingHandJoint.ThumbTip].Position;             // 親指先端
            Vector3 index_0 = joint[HandTrackingHandJoint.IndexMetacarpal].Position;      // 人指し指手首
            Vector3 index_1 = joint[HandTrackingHandJoint.IndexKnuckle].Position;         // 人指し指第一関節
            Vector3 index_2 = joint[HandTrackingHandJoint.IndexTip].Position;             // 人指し指先端
            Vector3 middle_0 = joint[HandTrackingHandJoint.MiddleMetacarpal].Position;    // 中指手首
            Vector3 middle_1 = joint[HandTrackingHandJoint.MiddleKnuckle].Position;       // 中指第一関節
            Vector3 middle_2 = joint[HandTrackingHandJoint.MiddleTip].Position;           // 中指先端
            Vector3 ring_0 = joint[HandTrackingHandJoint.RingMetacarpal].Position;        // 薬指手首
            Vector3 ring_1 = joint[HandTrackingHandJoint.RingKnuckle].Position;           // 薬指第一関節
            Vector3 ring_2 = joint[HandTrackingHandJoint.RingTip].Position;               // 薬指先端
            Vector3 pinky_0 = joint[HandTrackingHandJoint.PinkyMetacarpal].Position;      // 小指手首
            Vector3 pinky_1 = joint[HandTrackingHandJoint.PinkyKnuckle].Position;         // 小指第一関節
            Vector3 pinky_2 = joint[HandTrackingHandJoint.PinkyTip].Position;             // 小指先端


            // 各指の折れ曲がり角
            float angleThumb = Vector3.Angle((thumb_2 - thumb_1).normalized, (thumb_1 - thumb_0).normalized);
            float angleIndex = Vector3.Angle((index_2 - index_1).normalized, (index_1 - index_0).normalized);
            float angleMiddle = Vector3.Angle((middle_2 - middle_1).normalized, (middle_1 - middle_0).normalized);
            float angleRing = Vector3.Angle((ring_2 - ring_1).normalized, (ring_1 - ring_0).normalized);
            float anglePinky = Vector3.Angle((pinky_2 - pinky_1).normalized, (pinky_1 - pinky_0).normalized);

            // 各指の折れ曲がり状態の判定
            bool isHoldThumb = (angleThumb > p_HoldThumbAngle);
            bool isHoldIndex = (angleIndex > p_HoldFingerAngle);
            bool isHoldMiddle = (angleMiddle > p_HoldFingerAngle);
            bool isHoldRing = (angleRing > p_HoldFingerAngle);
            bool isHoldPinky = (anglePinky > p_HoldFingerAngle);


            // 開いている指の数を数える
            int openFingerCount = 0;
            if (!isHoldThumb) openFingerCount++;
            if (!isHoldIndex) openFingerCount++;
            if (!isHoldMiddle) openFingerCount++;
            if (!isHoldRing) openFingerCount++;
            if (!isHoldPinky) openFingerCount++;


            // 手の状態を判定する
            HandStatus status = HandStatus.Normal;

            // 開いている指が 0 本の場合
            if (openFingerCount == 0)
            {
                // グーと判定する
                status = HandStatus.Gur;
            }

            // 開いている指が 1 本の場合
            if (openFingerCount == 1)
            {
                // デフォルトではグーと判定する
                status = HandStatus.Gur;
                if (!isHoldThumb)
                {
                    // 親指が立っていればサムズアップと再判定する
                    status = HandStatus.ThumbsUp;
                }
                if (!isHoldIndex)
                {
                    // 人指し指が立っていればピストルと再判定する
                    status = HandStatus.Pistol;
                }
            }

            // 開いている指が 2 本の場合
            if (openFingerCount == 2)
            {
                // デフォルトではチョキと判定する
                status = HandStatus.Choki;
                if (!isHoldThumb && !isHoldIndex)
                {
                    // 親指と人指し指が立っていればピストルと再判定する
                    status = HandStatus.Pistol;
                }
            }

            // 開いている指が 3 本の場合
            if (openFingerCount == 3)
            {
                // デフォルトではパーと判定する
                status = HandStatus.Par;
                if (!isHoldIndex && !isHoldMiddle)
                {
                    // 人差し指と中指が立っていればチョキと再判定する
                    status = HandStatus.Choki;
                }
            }

            // 開いている指が 4 本の場合
            if (openFingerCount == 4)
            {
                // パーと判定する
                status = HandStatus.Par;
            }

            // 開いている指が 5 本の場合
            if (openFingerCount == 5)
            {
                // パーと判定する
                status = HandStatus.Par;
            }

            return status;
        }


        /// <summary>
        /// 手の状態を最も多区発生した状態で丸め込みを行う
        /// </summary>
        /// <param name="a_HandStatusList"></param>
        /// <returns></returns>
        private HandStatus RoundingHandStatus(IList<HandStatus> a_HandStatusList)
        {
            // 丸め込み結果
            HandStatus roundingResult = HandStatus.Normal;

            // 丸め込み用カウントリストを初期化する
            for (int index = 0; index < p_RoundingCountList.Length; index++)
            {
                p_RoundingCountList[index] = 0;
            }

            // ステータスの最大検出数
            int maxCount = 0;

            foreach (HandStatus checkHandStatus in a_HandStatusList)
            {
                // 各ステータスの判定回数をインクリメントする
                int statusCount = p_RoundingCountList[(int)checkHandStatus]++;

                // 最大検出数のステータスを記録する
                if (statusCount > maxCount)
                {
                    // 最大検出数であればステータスを保持しておく
                    roundingResult = checkHandStatus;
                    maxCount = statusCount;
                }
            }

            return roundingResult;
        }


        /// <summary>
        /// 手の状態を設定する
        /// </summary>
        /// <param name="a_HandStatus"></param>
        private void SettingHandStatus(HandStatus a_HandStatus)
        {
            switch (a_HandStatus)
            {
                // 通常状態
                case HandStatus.Normal:
                    SettingNormal();
                    break;
                // グー状態
                case HandStatus.Gur:
                    SettingGur();
                    break;
                // チョキ状態
                case HandStatus.Choki:
                    SettingChoki();
                    break;
                // パー状態
                case HandStatus.Par:
                    SettingPar();
                    break;
                // ピストル状態
                case HandStatus.Pistol:
                    SettingPistol();
                    break;
                // サムズアップ状態
                case HandStatus.ThumbsUp:
                    SettingThumbsUp();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 手の状態を通常状態に設定する
        /// </summary>
        private void SettingNormal()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Nothing);
            p_CurrentHandStatus = HandStatus.Normal;
        }

        /// <summary>
        /// 手の状態をグー状態に設定する
        /// </summary>
        private void SettingGur()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Gur);
            p_CurrentHandStatus = HandStatus.Gur;
        }

        /// <summary>
        /// 手の状態をチョキ状態に設定する
        /// </summary>
        private void SettingChoki()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Choki);
            p_CurrentHandStatus = HandStatus.Choki;
        }

        /// <summary>
        /// 手の状態をパー状態に設定する
        /// </summary>
        private void SettingPar()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Par);
            p_CurrentHandStatus = HandStatus.Par;
        }


        /// <summary>
        /// 手の状態をピストル状態に設定する
        /// </summary>
        private void SettingPistol()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Pistol);
            p_CurrentHandStatus = HandStatus.Pistol;
        }


        /// <summary>
        /// 手の状態をサムズアップ状態に設定する
        /// </summary>
        private void SettingThumbsUp()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_ThumbsUp);
            p_CurrentHandStatus = HandStatus.ThumbsUp;
        }
    }
}
/*
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

using HoloMonApp.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Player
{
    [RequireComponent(typeof(ObjectFeatures))]
    public class HandObjectFeaturesSetter : MonoBehaviour, IMixedRealityHandJointHandler, IMixedRealitySourceStateHandler
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

        /// <summary>
        /// 手の状態識別
        /// </summary>
        [Serializable]
        public enum HandStatus
        {
            Normal,           // 通常
            Gur,              // グー
            Choki,            // チョキ
            Par,              // パー
            Pistol,           // ピストル
            ThumbsUp,         // サムズアップ
        }

        [SerializeField, Tooltip("ハンドタイプ(左右)の設定")]
        private HandType p_SettingHandType;


        [SerializeField, Tooltip("親指の折れ曲がり判定角")]
        private float p_HoldThumbAngle = 30.0f;

        [SerializeField, Tooltip("四指の折れ曲がり判定角")]
        private float p_HoldFingerAngle = 60.0f;

        [SerializeField, Tooltip("手の状態を丸め込み判定するバッファサイズ")]
        private int p_HandStatusBufferCount = 10;


        [SerializeField, Tooltip("現在の手の状態")]
        private HandStatus p_CurrentHandStatus;

        [SerializeField, Tooltip("現在の手のソースID")]
        private uint p_CurrentHandSourceId;


        // オブジェクトの特徴の設定参照
        private ObjectFeatures p_ObjectFeatures;

        // 丸め込み用カウンタリスト
        private int[] p_RoundingCountList;


        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
            // オブジェクトの特徴の参照を取得する
            p_ObjectFeatures = this.gameObject.GetComponent<ObjectFeatures>();

            // 丸め込み用カウンタリストを作成しておく
            p_RoundingCountList = new int[Enum.GetNames(typeof(HandStatus)).Length];

            // 初期状態は通常状態
             SettingNormal();


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
                .Subscribe(eventData =>
                {
                    // ロスト時は設定をノーマルに初期化する
                    SettingNormal();
                })
                .AddTo(this);

            // 手の更新時オブザーバを作成する
            IDisposable OnHandJointsUpdatedObserver = Observable
                .FromEvent<InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>>>(
                    action => OnHandJointsUpdatedAction += action,
                    action => OnHandJointsUpdatedAction -= action
                )
                .Where(eventData => eventData.Handedness == GetCurrentHandednessType()) // チェック対象(右手/左手)である場合
                .Select(eventData => CheckCurrentHandStatus(eventData))                 // 手の状態を判定する
                .Buffer(p_HandStatusBufferCount)                                        // 判定結果をバッファする
                .Select(eventDataList => this.RoundingHandStatus(eventDataList))        // バッファした結果から最頻出の状態を取得する
                .Subscribe(status =>
                {
                    // 状態を設定する
                    SettingHandStatus(status);
                })
                .AddTo(this);
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
        /// 現在の手の状態を判定する
        /// </summary>
        /// <returns></returns>
        private HandStatus CheckCurrentHandStatus(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
        {
            // ソースIDを取得する
            p_CurrentHandSourceId = eventData.SourceId;

            // 各指のジョイント位置を取得する
            IDictionary<TrackedHandJoint, MixedRealityPose> joint = eventData.InputData;
            Vector3 wrist_0 = joint[TrackedHandJoint.Wrist].Position;                // 手首
            Vector3 thumb_0 = joint[TrackedHandJoint.ThumbMetacarpalJoint].Position; // 親指手首
            Vector3 thumb_1 = joint[TrackedHandJoint.ThumbProximalJoint].Position;   // 親指第一関節
            Vector3 thumb_2 = joint[TrackedHandJoint.ThumbTip].Position;             // 親指先端
            Vector3 index_0 = joint[TrackedHandJoint.IndexMetacarpal].Position;      // 人指し指手首
            Vector3 index_1 = joint[TrackedHandJoint.IndexKnuckle].Position;         // 人指し指第一関節
            Vector3 index_2 = joint[TrackedHandJoint.IndexTip].Position;             // 人指し指先端
            Vector3 middle_0 = joint[TrackedHandJoint.MiddleMetacarpal].Position;    // 中指手首
            Vector3 middle_1 = joint[TrackedHandJoint.MiddleKnuckle].Position;       // 中指第一関節
            Vector3 middle_2 = joint[TrackedHandJoint.MiddleTip].Position;           // 中指先端
            Vector3 ring_0 = joint[TrackedHandJoint.RingMetacarpal].Position;        // 薬指手首
            Vector3 ring_1 = joint[TrackedHandJoint.RingKnuckle].Position;           // 薬指第一関節
            Vector3 ring_2 = joint[TrackedHandJoint.RingTip].Position;               // 薬指先端
            Vector3 pinky_0 = joint[TrackedHandJoint.PinkyMetacarpal].Position;      // 小指手首
            Vector3 pinky_1 = joint[TrackedHandJoint.PinkyKnuckle].Position;         // 小指第一関節
            Vector3 pinky_2 = joint[TrackedHandJoint.PinkyTip].Position;             // 小指先端


            // 各指の折れ曲がり角
            float angleThumb = Vector3.Angle((thumb_2 - thumb_1).normalized, (thumb_1 - thumb_0).normalized);
            float angleIndex = Vector3.Angle((index_2 - index_1).normalized, (index_1 - index_0).normalized);
            float angleMiddle = Vector3.Angle((middle_2 - middle_1).normalized, (middle_1 - middle_0).normalized);
            float angleRing = Vector3.Angle((ring_2 - ring_1).normalized, (ring_1 - ring_0).normalized);
            float anglePinky = Vector3.Angle((pinky_2 - pinky_1).normalized, (pinky_1 - pinky_0).normalized);

            // 各指の折れ曲がり状態の判定
            bool isHoldThumb = (angleThumb > p_HoldThumbAngle);
            bool isHoldIndex = (angleIndex > p_HoldFingerAngle);
            bool isHoldMiddle = (angleMiddle > p_HoldFingerAngle);
            bool isHoldRing = (angleRing > p_HoldFingerAngle);
            bool isHoldPinky = (anglePinky > p_HoldFingerAngle);


            // 開いている指の数を数える
            int openFingerCount = 0;
            if (!isHoldThumb) openFingerCount++;
            if (!isHoldIndex) openFingerCount++;
            if (!isHoldMiddle) openFingerCount++;
            if (!isHoldRing) openFingerCount++;
            if (!isHoldPinky) openFingerCount++;


            // 手の状態を判定する
            HandStatus status = HandStatus.Normal;

            // 開いている指が 0 本の場合
            if (openFingerCount == 0)
            {
                // グーと判定する
                status = HandStatus.Gur;
            }

            // 開いている指が 1 本の場合
            if (openFingerCount == 1)
            {
                // デフォルトではグーと判定する
                status = HandStatus.Gur;
                if (!isHoldThumb)
                {
                    // 親指が立っていればサムズアップと再判定する
                    status = HandStatus.ThumbsUp;
                }
                if (!isHoldIndex)
                {
                    // 人指し指が立っていればピストルと再判定する
                    status = HandStatus.Pistol;
                }
            }

            // 開いている指が 2 本の場合
            if (openFingerCount == 2)
            {
                // デフォルトではチョキと判定する
                status = HandStatus.Choki;
                if (!isHoldThumb && !isHoldIndex)
                {
                    // 親指と人指し指が立っていればピストルと再判定する
                    status = HandStatus.Pistol;
                }
            }

            // 開いている指が 3 本の場合
            if (openFingerCount == 3)
            {
                // デフォルトではパーと判定する
                status = HandStatus.Par;
                if (!isHoldIndex && !isHoldMiddle)
                {
                    // 人差し指と中指が立っていればチョキと再判定する
                    status = HandStatus.Choki;
                }
            }

            // 開いている指が 4 本の場合
            if (openFingerCount == 4)
            {
                // パーと判定する
                status = HandStatus.Par;
            }

            // 開いている指が 5 本の場合
            if (openFingerCount == 5)
            {
                // パーと判定する
                status = HandStatus.Par;
            }

            return status;
        }


        /// <summary>
        /// 手の状態を最も多区発生した状態で丸め込みを行う
        /// </summary>
        /// <param name="a_HandStatusList"></param>
        /// <returns></returns>
        private HandStatus RoundingHandStatus(IList<HandStatus> a_HandStatusList)
        {
            // 丸め込み結果
            HandStatus roundingResult = HandStatus.Normal;

            // 丸め込み用カウントリストを初期化する
            for(int index = 0; index < p_RoundingCountList.Length; index++)
            {
                p_RoundingCountList[index] = 0;
            }

            // ステータスの最大検出数
            int maxCount = 0;

            foreach (HandStatus checkHandStatus in a_HandStatusList)
            {
                // 各ステータスの判定回数をインクリメントする
                int statusCount = p_RoundingCountList[(int)checkHandStatus]++;

                // 最大検出数のステータスを記録する
                if (statusCount > maxCount)
                {
                    // 最大検出数であればステータスを保持しておく
                    roundingResult = checkHandStatus;
                    maxCount = statusCount;
                }
            }

            return roundingResult;
        }


        /// <summary>
        /// 手の状態を設定する
        /// </summary>
        /// <param name="a_HandStatus"></param>
        private void SettingHandStatus(HandStatus a_HandStatus)
        {
            switch (a_HandStatus)
            {
                // 通常状態
                case HandStatus.Normal:
                    SettingNormal();
                    break;
                // グー状態
                case HandStatus.Gur:
                    SettingGur();
                    break;
                // チョキ状態
                case HandStatus.Choki:
                    SettingChoki();
                    break;
                // パー状態
                case HandStatus.Par:
                    SettingPar();
                    break;
                // ピストル状態
                case HandStatus.Pistol:
                    SettingPistol();
                    break;
                // サムズアップ状態
                case HandStatus.ThumbsUp:
                    SettingThumbsUp();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 手の状態を通常状態に設定する
        /// </summary>
        private void SettingNormal()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Nothing);
            p_CurrentHandStatus = HandStatus.Normal;
        }

        /// <summary>
        /// 手の状態をグー状態に設定する
        /// </summary>
        private void SettingGur()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Gur);
            p_CurrentHandStatus = HandStatus.Gur;
        }

        /// <summary>
        /// 手の状態をチョキ状態に設定する
        /// </summary>
        private void SettingChoki()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Choki);
            p_CurrentHandStatus = HandStatus.Choki;
        }

        /// <summary>
        /// 手の状態をパー状態に設定する
        /// </summary>
        private void SettingPar()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Par);
            p_CurrentHandStatus = HandStatus.Par;
        }


        /// <summary>
        /// 手の状態をピストル状態に設定する
        /// </summary>
        private void SettingPistol()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_Pistol);
            p_CurrentHandStatus = HandStatus.Pistol;
        }


        /// <summary>
        /// 手の状態をサムズアップ状態に設定する
        /// </summary>
        private void SettingThumbsUp()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHand(ObjectStatusHand.Hand_ThumbsUp);
            p_CurrentHandStatus = HandStatus.ThumbsUp;
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
            if (OnSourceDetectedAction != null) OnSourceDetectedAction(eventData);
        }

        /// <summary>
        /// 手のロスト時に発生するイベント(IMixedRealitySourceStateHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSourceLost(SourceStateEventData eventData)
        {
            // アクション呼び出し
            if (OnSourceLostAction != null) OnSourceLostAction(eventData);
        }

        /// <summary>
        /// 手の更新時に発生するイベント(IMixedRealityHandJointHandler)
        /// </summary>
        /// <param name="eventData"></param>
        public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
        {
            // アクション呼び出し
            if (OnHandJointsUpdatedAction != null) OnHandJointsUpdatedAction(eventData);
        }
    }
}
*/