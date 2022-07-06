using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Model.Sensations.FeelHandGrab;

namespace HoloMonApp.Content.Character.View.Sensations.FeelHandGrab
{
    /// <summary>
    /// 掴まれ感知API
    /// </summary>
    public class HoloMonViewFeelHandGrabAPI : MonoBehaviour
    {
        /// <summary>
        /// APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonFeelHandGrabAPI p_FeelHandGrabAPI;

        /// <summary>
        /// ホロモンの掴まれ状態のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<bool> ReactivePropertyHoloMonHandGrabFeel
            => p_FeelHandGrabAPI.IReadOnlyReactivePropertyHoloMonHandGrabFeel;

        /// <summary>
        /// ホロモンの掴まれ状態の短縮参照用変数
        /// </summary>
        public bool HandGrabed => p_FeelHandGrabAPI.HandGrabed;
    }
}