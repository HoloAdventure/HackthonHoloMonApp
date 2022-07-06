using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Model.Sensations.FeelEyeGaze;

namespace HoloMonApp.Content.Character.View.Sensations.FeelEyeGaze
{
    /// <summary>
    /// 視線感知API
    /// </summary>
    public class HoloMonViewFeelEyeGazeAPI : MonoBehaviour
    {
        /// <summary>
        /// APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonFeelEyeGazeAPI p_FeelEyeGazeAPI;

        /// <summary>
        /// ホロモンに向けられた視線の状態のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<bool> ReactivePropertyHoloMonEyeGazed
            => p_FeelEyeGazeAPI.IReadOnlyReactivePropertyHoloMonEyeGazed;

        /// <summary>
        /// ホロモンに向けられた視線の状態の短縮参照用変数
        /// </summary>
        public bool EyeGazed => p_FeelEyeGazeAPI.EyeGazed;
    }
}