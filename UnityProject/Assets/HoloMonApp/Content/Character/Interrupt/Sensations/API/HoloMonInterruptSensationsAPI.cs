using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Interrupt.Sensations.FeelEyeGaze;
using HoloMonApp.Content.Character.Interrupt.Sensations.FeelHandGrab;
using HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Interrupt.Sensations.ListenVoice;
using HoloMonApp.Content.Character.Interrupt.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Interrupt.Sensations
{
    public class HoloMonInterruptSensationsAPI : MonoBehaviour, HoloMonInterruptIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonInterruptReference p_Reference;

        /// <summary>
        /// 初期化
        /// </summary>
        public void AwakeInit(HoloMonInterruptReference reference)
        {
            p_Reference = reference;
            foreach (HoloMonInterruptIF instance in p_ListIF)
            {
                instance.AwakeInit(p_Reference);
            }
        }

        /// <summary>
        /// 視線検知APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptFeelEyeGazeAPI p_FeelEyeGaze;

        /// <summary>
        /// 掴み検知APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptFeelHandGrabAPI p_FeelHandGrab;

        /// <summary>
        /// 視界検知APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptFieldOfVisionAPI p_FieldOfVision;

        /// <summary>
        /// 言葉検知APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptListenVoiceAPI p_ListenVoice;

        /// <summary>
        /// 触覚検知APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptTactileBodyAPI p_TactileBody;


        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        private List<HoloMonInterruptIF> p_ListIF
            => p_ListIFInstance
            ?? (p_ListIFInstance = new List<HoloMonInterruptIF>()
            {
                p_FeelEyeGaze,
                p_FeelHandGrab,
                p_FieldOfVision,
                p_ListenVoice,
                p_TactileBody,
            });

        /// <summary>
        /// インタフェースの参照リスト(実体)
        /// </summary>
        private List<HoloMonInterruptIF> p_ListIFInstance;
    }
}