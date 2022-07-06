using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Conditions.Body;

namespace HoloMonApp.Content.Character.Interrupt.Conditions.Body
{
    [Serializable]
    public class InterruptConditionBodyData : InterruptDataInterface
    {
        /// <summary>
        /// 身体コンディション情報
        /// </summary>
        [SerializeField, Tooltip("身体コンディション情報")]
        private HoloMonBodyStatus p_BodyStatus;

        public HoloMonBodyStatus BodyStatus => p_BodyStatus;

        public InterruptConditionBodyData(HoloMonBodyStatus a_BodyStatus)
        {
            p_BodyStatus = a_BodyStatus;
        }
    }
}