using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Interrupt.Conditions.Body;
using HoloMonApp.Content.Character.Interrupt.Conditions.Life;
using HoloMonApp.Content.Character.Interrupt.Sensations.FeelEyeGaze;
using HoloMonApp.Content.Character.Interrupt.Sensations.FeelHandGrab;
using HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Interrupt.Sensations.ListenVoice;
using HoloMonApp.Content.Character.Interrupt.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Interrupt
{
    [Serializable]
    public class InterruptInformation
    {
        [SerializeField, Tooltip("ホロモンインタラプト種別")]
        private HoloMonInterruptType p_HoloMonInterruptType;

        public HoloMonInterruptType HoloMonInterruptType => p_HoloMonInterruptType;

        [SerializeField]
        private InterruptDataInterface p_InterruptData;

        // キャスト参照
        public InterruptConditionBodyData InterruptConditionBodyData
            => (InterruptConditionBodyData)p_InterruptData;

        public InterruptConditionLifeData InterruptConditionLifeData
            => (InterruptConditionLifeData)p_InterruptData;

        public InterruptListenVoiceData InterruptListenWordData
            => (InterruptListenVoiceData)p_InterruptData;

        public InterruptFieldOfVisionData InterruptFieldOfVisionData
            => (InterruptFieldOfVisionData)p_InterruptData;

        public InterruptTactileBodyData InterruptTactileBodyData
            => (InterruptTactileBodyData)p_InterruptData;

        public InterruptFeelEyeGazeData InterruptGazeBodyData
            => (InterruptFeelEyeGazeData)p_InterruptData;

        public InterruptFeelHandGrabData InterruptHandGrabBodyData
            => (InterruptFeelHandGrabData)p_InterruptData;


        public InterruptInformation(InterruptConditionBodyData a_Data)
        {
            p_HoloMonInterruptType = HoloMonInterruptType.ConditionBody;
            p_InterruptData = a_Data;
        }

        public InterruptInformation(InterruptConditionLifeData a_Data)
        {
            p_HoloMonInterruptType = HoloMonInterruptType.ConditionLife;
            p_InterruptData = a_Data;
        }

        public InterruptInformation(InterruptListenVoiceData a_Data)
        {
            p_HoloMonInterruptType = HoloMonInterruptType.ListenWord;
            p_InterruptData = a_Data;
        }

        public InterruptInformation(InterruptFieldOfVisionData a_Data)
        {
            p_HoloMonInterruptType = HoloMonInterruptType.FieldOfVision;
            p_InterruptData = a_Data;
        }

        public InterruptInformation(InterruptTactileBodyData a_Data)
        {
            p_HoloMonInterruptType = HoloMonInterruptType.TactileBody;
            p_InterruptData = a_Data;
        }

        public InterruptInformation(InterruptFeelEyeGazeData a_Data)
        {
            p_HoloMonInterruptType = HoloMonInterruptType.EyeGazeBody;
            p_InterruptData = a_Data;
        }

        public InterruptInformation(InterruptFeelHandGrabData a_Data)
        {
            p_HoloMonInterruptType = HoloMonInterruptType.HandGrabBody;
            p_InterruptData = a_Data;
        }
    }
}