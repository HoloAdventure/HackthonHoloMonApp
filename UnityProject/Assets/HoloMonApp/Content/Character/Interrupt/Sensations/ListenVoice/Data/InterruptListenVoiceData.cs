using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Words;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.ListenVoice
{
    [Serializable]
    public class InterruptListenVoiceData : InterruptDataInterface
    {
        /// <summary>
        /// 聞き取った言葉
        /// </summary>
        [SerializeField, Tooltip("聞き取った言葉")]
        private HoloMonListenWord p_ListenWord;

        public HoloMonListenWord ListenWord => p_ListenWord;

        public InterruptListenVoiceData(HoloMonListenWord a_ListenWord)
        {
            p_ListenWord = a_ListenWord;
        }
    }
}