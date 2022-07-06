using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.View.Parameters.Conditions;
using HoloMonApp.Content.Character.View.Parameters.Settings;

namespace HoloMonApp.Content.Character.View.Parameters
{
    public class HoloMonViewParametersAPI : MonoBehaviour
    {
        /// <summary>
        /// コンディションデータの参照
        /// </summary>
        [SerializeField, Tooltip("コンディションデータの参照")]
        private HoloMonViewConditionsAPI p_Conditions;
        public HoloMonViewConditionsAPI Conditions => p_Conditions;

        /// <summary>
        /// 設定データの参照
        /// </summary>
        [SerializeField, Tooltip("設定データの参照")]
        private HoloMonViewSettingsAPI p_Settings;
        public HoloMonViewSettingsAPI Settings => p_Settings;
    }
}