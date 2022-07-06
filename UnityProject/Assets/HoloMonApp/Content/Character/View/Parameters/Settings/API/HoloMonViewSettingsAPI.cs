using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.View.Parameters.Settings.Length;
using HoloMonApp.Content.Character.View.Parameters.Settings.Personal;

namespace HoloMonApp.Content.Character.View.Parameters.Settings
{
    /// <summary>
    /// 設定データAPI
    /// </summary>
    public class HoloMonViewSettingsAPI : MonoBehaviour
    {
        /// <summary>
        /// 個性設定の参照
        /// </summary>
        [SerializeField, Tooltip("個性設定の参照")]
        private HoloMonViewSettingsLengthAPI p_Length;
        public HoloMonViewSettingsLengthAPI Length => p_Length;

        /// <summary>
        /// 個性設定の参照
        /// </summary>
        [SerializeField, Tooltip("個性設定の参照")]
        private HoloMonViewSettingsPersonalAPI p_Personal;
        public HoloMonViewSettingsPersonalAPI Personal => p_Personal;
    }
}