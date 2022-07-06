using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Control.Parameters.Conditions;
using HoloMonApp.Content.Character.Control.Parameters.Settings;

namespace HoloMonApp.Content.Character.Control.Parameters
{
    /// <summary>
    /// コンディションデータAPI
    /// </summary>
    public class HoloMonControlParametersAPI : MonoBehaviour
    {
        /// <summary>
        /// コンディションの参照
        /// </summary>
        [SerializeField, Tooltip("コンディションの参照")]
        private HoloMonControlConditionsAPI p_Conditions;
        public HoloMonControlConditionsAPI Conditions => p_Conditions;

        /// <summary>
        /// セッティングの参照
        /// </summary>
        [SerializeField, Tooltip("セッティングの参照")]
        private HoloMonControlSettingsAPI p_Settings;
        public HoloMonControlSettingsAPI Settings => p_Settings;
    }
}