using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Parameters.Conditions.Life
{
    /// <summary>
    /// ホロモンの機嫌度設定
    /// </summary>
    [Serializable]
    public class HoloMonHumorSetting
    {
        [SerializeField, Tooltip("減少値")]
        private int p_DecreasePoint;

        /// <summary>
        /// 減少値
        /// </summary>
        public int DecreasePoint => p_DecreasePoint;

        [SerializeField, Tooltip("減少間隔")]
        private int p_DecreaseMarginMinute;

        /// <summary>
        /// 減少間隔(分)
        /// </summary>
        public int DecreaseMarginMinute => p_DecreaseMarginMinute;

        public HoloMonHumorSetting()
        {
        }
    }
}
