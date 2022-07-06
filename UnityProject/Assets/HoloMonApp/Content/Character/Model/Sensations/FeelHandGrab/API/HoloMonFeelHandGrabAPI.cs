using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace HoloMonApp.Content.Character.Model.Sensations.FeelHandGrab
{
    /// <summary>
    /// 掴まれ感知API
    /// </summary>
    public class HoloMonFeelHandGrabAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンの掴まれ状態
        /// </summary>
        [SerializeField, Tooltip("ホロモンの掴まれ状態")]
        private BoolReactiveProperty p_HoloMonHandGrabFeel = new BoolReactiveProperty(false);

        /// <summary>
        /// ホロモンの掴まれ状態のReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<bool> p_IReadOnlyReactivePropertyHoloMonHandGrabFeel;

        /// <summary>
        /// ホロモンの掴まれ状態のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IReadOnlyReactivePropertyHoloMonHandGrabFeel
            => p_IReadOnlyReactivePropertyHoloMonHandGrabFeel
            ?? (p_IReadOnlyReactivePropertyHoloMonHandGrabFeel
            = p_HoloMonHandGrabFeel.ToSequentialReadOnlyReactiveProperty());

        /// <summary>
        /// ホロモンの掴まれ状態の短縮参照用変数
        /// </summary>
        public bool HandGrabed => IReadOnlyReactivePropertyHoloMonHandGrabFeel.Value;


        /// <summary>
        /// 掴まれ状態の変化
        /// </summary>
        /// <param name="a_IsHandGrab"></param>
        private void ReceptionHandGrabFeel(bool a_IsHandGrab)
        {
            // 変数の登録を行う
            // 非変更時は通知が発生しない
            p_HoloMonHandGrabFeel.Value = a_IsHandGrab;
        }

        /// <summary>
        /// ホロモンが掴まれたかどうか
        /// </summary>
        /// <param name="a_IsHandGrab"></param>
        public void HandGrabFeelHoloMon(bool a_IsHandGrab)
        {
            ReceptionHandGrabFeel(a_IsHandGrab);
        }
    }
}