using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.View.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.View.Sensations.TactileBody;
using HoloMonApp.Content.Character.View.Sensations.ListenVoice;
using HoloMonApp.Content.Character.View.Sensations.FeelEyeGaze;
using HoloMonApp.Content.Character.View.Sensations.FeelHandGrab;

namespace HoloMonApp.Content.Character.View.Sensations
{
    /// <summary>
    /// 感覚システムAPI
    /// </summary>
    public class HoloMonViewSensationsAPI : MonoBehaviour
    {
        /// <summary>
        /// 視界システムの参照
        /// </summary>
        [SerializeField, Tooltip("視界システムの参照")]
        private HoloMonViewFieldOfVisionAPI p_FieldOfVision;
        public HoloMonViewFieldOfVisionAPI FieldOfVision => p_FieldOfVision;

        /// <summary>
        /// 触覚システムの参照
        /// </summary>
        [SerializeField, Tooltip("触覚システムの参照")]
        private HoloMonViewTactileBodyAPI p_TactileBody;
        public HoloMonViewTactileBodyAPI TactileBody => p_TactileBody;

        /// <summary>
        /// 声認識システムの参照
        /// </summary>
        [SerializeField, Tooltip("声認識システムの参照")]
        private HoloMonViewListenVoiceAPI p_ListenVoice;
        public HoloMonViewListenVoiceAPI ListenVoice => p_ListenVoice;

        /// <summary>
        /// 視線感知の参照
        /// </summary>
        [SerializeField, Tooltip("視線感知の参照")]
        private HoloMonViewFeelEyeGazeAPI p_FeelEyeGaze;
        public HoloMonViewFeelEyeGazeAPI FeelEyeGaze => p_FeelEyeGaze;

        /// <summary>
        /// 掴まれ感知の参照
        /// </summary>
        [SerializeField, Tooltip("掴まれ感知の参照")]
        private HoloMonViewFeelHandGrabAPI p_FeelHandGrab;
        public HoloMonViewFeelHandGrabAPI FeelHandGrab => p_FeelHandGrab;
    }
}