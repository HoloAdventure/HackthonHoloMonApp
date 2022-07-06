using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Data.Settings.Personal;

namespace HoloMonApp.Content.Character.Model.Parameters.Settings.Personal
{
    /// <summary>
    /// 個性設定API
    /// </summary>
    public class HoloMonSettingsPersonalAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンの個別設定データの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンの個別設定データの参照")]
        private HoloMonSettingPersonalData p_HoloMonSettingPersonalData;

        /// <summary>
        /// ホロモンのアイデンティティのReadOnlyReactivePropertyの参照用変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonPersonalIdentity> ReactivePropertyStatus
            => p_HoloMonSettingPersonalData.IReadOnlyReactivePropertyIdentity;

        /// <summary>
        /// ホロモンの名前の短縮参照用変数
        /// </summary>
        public string HoloMonName => p_HoloMonSettingPersonalData.Identity.HoloMonName;


        /// <summary>
        /// 設定の初期化
        /// </summary>
        public void InitializeSetting()
        {
            p_HoloMonSettingPersonalData.InitializeSetting();
        }
    }
}