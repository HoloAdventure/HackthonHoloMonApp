using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose.MoveTracking
{
    [Serializable]
    public class PurposeMoveTrackingData : PurposeDataInterface
    {
        [SerializeField, Tooltip("追跡対象種別")]
        private ObjectUnderstandType p_TrackingUnderstandType;

        /// <summary>
        /// 追跡対象種別
        /// </summary>
        public ObjectUnderstandType TrackingUnderstandType => p_TrackingUnderstandType;

        [SerializeField, Tooltip("追跡の諦めフラグ")]
        private bool p_NeverGiveUp;

        /// <summary>
        /// 追跡の諦めないフラグ
        /// </summary>
        public bool NeverGiveUp => p_NeverGiveUp;

        public PurposeMoveTrackingData(ObjectUnderstandType a_TrackingUnderstandType, bool a_NeverGiveUp)
        {
            p_TrackingUnderstandType = a_TrackingUnderstandType;
            p_NeverGiveUp = a_NeverGiveUp;
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.MoveTracking;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}