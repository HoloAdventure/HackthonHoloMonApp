using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision
{
    [Serializable]
    public class InterruptFieldOfVisionData : InterruptDataInterface
    {
        [SerializeField, Tooltip("視覚のイベント種別")]
        private HoloMonFieldOfVisionEvent p_FieldOfVisionEvent;

        /// <summary>
        /// 触覚のイベント種別
        /// </summary>
        public HoloMonFieldOfVisionEvent FieldOfVisionEvent => p_FieldOfVisionEvent;

        [SerializeField, Tooltip("検出オブジェクト")]
        private VisionObjectWrap p_VisionObjectWrap;

        /// <summary>
        /// 検出オブジェクト(ロスト時Null)
        /// </summary>
        public VisionObjectWrap VisionObjectWrap => p_VisionObjectWrap;

        [SerializeField, Tooltip("オブジェクトID")]
        private int p_ObjectInstanceID;

        /// <summary>
        /// オブジェクトID
        /// </summary>
        public int ObjectInstanceID => p_ObjectInstanceID;

        public InterruptFieldOfVisionData(
            HoloMonFieldOfVisionEvent a_FieldOfVisionEvent,
            VisionObjectWrap a_VisionObjectWrap,
            int a_ObjectInstanceID)
        {
            p_FieldOfVisionEvent = a_FieldOfVisionEvent;
            p_VisionObjectWrap = a_VisionObjectWrap;
            p_ObjectInstanceID = a_ObjectInstanceID;
        }
    }
}