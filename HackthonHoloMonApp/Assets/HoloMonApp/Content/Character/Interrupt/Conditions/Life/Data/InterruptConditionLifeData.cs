using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using HoloMonApp.Content.Character.Data.Conditions.Life;

namespace HoloMonApp.Content.Character.Interrupt.Conditions.Life
{
    [Serializable]
    public class InterruptConditionLifeData : InterruptDataInterface
    {
        /// <summary>
        /// 生活コンディション情報
        /// </summary>
        [SerializeField, Tooltip("生活コンディション情報")]
        private HoloMonLifeStatus p_LifeStatus;

        public HoloMonLifeStatus LifeStatus => p_LifeStatus;

        public InterruptConditionLifeData(HoloMonLifeStatus a_LifeStatus)
        {
            p_LifeStatus = a_LifeStatus;
        }
    }
}