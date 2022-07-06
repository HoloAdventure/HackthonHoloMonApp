using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.TactileBody
{
    [Serializable]
    public class InterruptTactileBodyData : InterruptDataInterface
    {
        [SerializeField, Tooltip("触覚のイベント種別")]
        private HoloMonTactileBodyEvent p_TactileBodyEvent;

        /// <summary>
        /// 触覚のイベント種別
        /// </summary>
        public HoloMonTactileBodyEvent TactileBodyEvent => p_TactileBodyEvent;

        [SerializeField, Tooltip("触覚の発生パーツ")]
        private HoloMonTactileBodyLimb p_TactileBodyLimb;

        /// <summary>
        /// 触覚の発生パーツ
        /// </summary>
        public HoloMonTactileBodyLimb TactileBodyLimb => p_TactileBodyLimb;

        [SerializeField, Tooltip("検出オブジェクト")]
        private TactileObjectWrap p_TactileObjectWrap;

        /// <summary>
        /// 検出オブジェクト(ロスト時Null)
        /// </summary>
        public TactileObjectWrap TactileObjectWrap => p_TactileObjectWrap;

        [SerializeField, Tooltip("オブジェクトID")]
        private int p_ObjectInstanceID;

        /// <summary>
        /// オブジェクトID
        /// </summary>
        public int ObjectInstanceID => p_ObjectInstanceID;

        public InterruptTactileBodyData(
            HoloMonTactileBodyEvent a_TactileBodyEvent,
            HoloMonTactileBodyLimb a_TactileBodyLimb,
            TactileObjectWrap a_TactileObjectWrap,
            int a_ObjectInstanceID)
        {
            p_TactileBodyEvent = a_TactileBodyEvent;
            p_TactileBodyLimb = a_TactileBodyLimb;
            p_TactileObjectWrap = a_TactileObjectWrap;
            p_ObjectInstanceID = a_ObjectInstanceID;
        }
    }
}