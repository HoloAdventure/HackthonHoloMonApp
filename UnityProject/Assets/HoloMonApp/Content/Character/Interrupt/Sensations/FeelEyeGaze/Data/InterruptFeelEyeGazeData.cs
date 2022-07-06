using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.FeelEyeGaze
{
    [Serializable]
    public class InterruptFeelEyeGazeData : InterruptDataInterface
    {
        /// <summary>
        /// 視線がこちらを向いているか否か
        /// </summary>
        [SerializeField, Tooltip("視線がこちらを向いているか否か")]
        private bool p_IsSeen;

        public bool IsSeen => p_IsSeen;

        public InterruptFeelEyeGazeData(bool a_IsSeen)
        {
            p_IsSeen = a_IsSeen;
        }
    }
}