using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.FeelHandGrab
{
    [Serializable]
    public class InterruptFeelHandGrabData : InterruptDataInterface
    {
        /// <summary>
        /// 掴まれているか否か
        /// </summary>
        [SerializeField, Tooltip("掴まれているか否か")]
        private bool p_IsGrabbed;

        public bool IsGrabbed => p_IsGrabbed;

        public InterruptFeelHandGrabData(bool a_IsGrabbed)
        {
            p_IsGrabbed = a_IsGrabbed;
        }
    }
}