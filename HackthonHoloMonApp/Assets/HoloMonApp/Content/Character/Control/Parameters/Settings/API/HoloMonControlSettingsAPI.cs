using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Control.Parameters.Settings.Personal;

namespace HoloMonApp.Content.Character.Control.Parameters.Settings
{
    /// <summary>
    /// 設定データAPI
    /// </summary>
    public class HoloMonControlSettingsAPI : MonoBehaviour
    {
        /// <summary>
        /// 個性設定の参照
        /// </summary>
        [SerializeField, Tooltip("個性設定の参照")]
        private HoloMonControlSettingsPersonalAPI p_Personal;
        public HoloMonControlSettingsPersonalAPI Personal => p_Personal;
    }
}