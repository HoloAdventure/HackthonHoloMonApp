using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Model.Parameters.Settings.Length;
using HoloMonApp.Content.Character.Data.Settings.Length;

namespace HoloMonApp.Content.Character.View.Parameters.Settings.Length
{
    /// <summary>
    /// 個性設定API
    /// </summary>
    public class HoloMonViewSettingsLengthAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonSettingsLengthAPI p_SettingsLengthAPI;

        /// <summary>
        /// ホロモンのアイデンティティのReadOnlyReactivePropertyの参照用変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonLengthIdentity> ReactivePropertyStatus
            => p_SettingsLengthAPI.ReactivePropertyStatus;

        /// <summary>
        /// ホロモンの縦幅(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualHeightLength => p_SettingsLengthAPI.HoloMonActualHeightLength;

        /// <summary>
        /// ホロモンの横幅(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualWideLength => p_SettingsLengthAPI.HoloMonActualWideLength;

        /// <summary>
        /// ホロモンの奥幅(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualDepthLength => p_SettingsLengthAPI.HoloMonActualDepthLength;

        /// <summary>
        /// ホロモンの手が届く距離(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualHandReachableLength => p_SettingsLengthAPI.HoloMonActualHandReachableLength;
    }
}