using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Model.Parameters.Settings.Personal;
using HoloMonApp.Content.Character.Data.Settings.Personal;

namespace HoloMonApp.Content.Character.View.Parameters.Settings.Personal
{
    /// <summary>
    /// 個性設定API
    /// </summary>
    public class HoloMonViewSettingsPersonalAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonSettingsPersonalAPI p_SettingsPersonalAPI;

        /// <summary>
        /// ホロモンのアイデンティティのReadOnlyReactivePropertyの参照用変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonPersonalIdentity> ReactivePropertyStatus
            => p_SettingsPersonalAPI.ReactivePropertyStatus;

        /// <summary>
        /// ホロモンの名前の短縮参照用変数
        /// </summary>
        public string HoloMonName => p_SettingsPersonalAPI.HoloMonName;
    }
}