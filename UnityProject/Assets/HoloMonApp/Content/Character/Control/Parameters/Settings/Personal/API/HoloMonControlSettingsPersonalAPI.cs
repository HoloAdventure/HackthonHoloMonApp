using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Model;
using HoloMonApp.Content.Character.Model.Parameters.Settings.Personal;

namespace HoloMonApp.Content.Character.Control.Parameters.Settings.Personal
{
    /// <summary>
    /// 個性設定API
    /// </summary>
    public class HoloMonControlSettingsPersonalAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonSettingsPersonalAPI p_SettingsPersonalAPI;

        /// <summary>
        /// 設定の初期化
        /// </summary>
        public void InitializeSetting()
        {
            p_SettingsPersonalAPI.InitializeSetting();
        }
    }
}