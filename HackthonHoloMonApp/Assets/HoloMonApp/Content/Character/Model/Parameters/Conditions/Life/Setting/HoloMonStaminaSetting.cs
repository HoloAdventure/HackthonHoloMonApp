using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Parameters.Conditions.Life
{
    /// <summary>
    /// ホロモンの元気設定
    /// </summary>
    [Serializable]
    public class HoloMonStaminaSetting
    {
        [SerializeField, Tooltip("増加値")]
        private int p_IncreasePoint;

        /// <summary>
        /// 増加値
        /// </summary>
        public int IncreasePoint => p_IncreasePoint;

        [SerializeField, Tooltip("増加間隔(分)")]
        private int p_IncreaseMarginMinute;

        /// <summary>
        /// 増加間隔(分)
        /// </summary>
        public int IncreaseMarginMinute => p_IncreaseMarginMinute;

        public HoloMonStaminaSetting()
        {
        }
    }
}
