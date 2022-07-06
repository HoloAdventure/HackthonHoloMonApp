using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace HoloMonApp.Content.Character.Model.Sensations.FeelEyeGaze
{
    /// <summary>
    /// 視線感知API
    /// </summary>
    public class HoloMonFeelEyeGazeAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンに向けられた視線の状態
        /// </summary>
        [SerializeField, Tooltip("ホロモンに向けられた視線の状態")]
        private BoolReactiveProperty p_HoloMonEyeGazed = new BoolReactiveProperty(false);

        /// <summary>
        /// ホロモンに向けられた視線の状態のReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<bool> p_IReadOnlyReactivePropertyHoloMonEyeGazed;

        /// <summary>
        /// ホロモンに向けられた視線の状態のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IReadOnlyReactivePropertyHoloMonEyeGazed
            => p_IReadOnlyReactivePropertyHoloMonEyeGazed
            ?? (p_IReadOnlyReactivePropertyHoloMonEyeGazed
            = p_HoloMonEyeGazed.ToSequentialReadOnlyReactiveProperty());

        /// <summary>
        /// ホロモンに向けられた視線の状態の短縮参照用変数
        /// </summary>
        public bool EyeGazed => IReadOnlyReactivePropertyHoloMonEyeGazed.Value;


        /// <summary>
        /// 視線の変化
        /// </summary>
        /// <param name="a_IsSeen"></param>
        private void ReceptionEyeGazed(bool a_IsSeen)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            p_HoloMonEyeGazed.Value = a_IsSeen;
        }

        /// <summary>
        /// ホロモンが視線を受けたかどうか
        /// </summary>
        /// <param name="a_IsSeen"></param>
        public void EyeGazeFeelHoloMon(bool a_IsSeen)
        {
            ReceptionEyeGazed(a_IsSeen);
        }
    }
}