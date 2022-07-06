using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Parameters.Conditions.Life
{
    /// <summary>
    /// ホロモンの空腹度設定
    /// </summary>
    [Serializable]
    public class HoloMonShitSetting
    {
        [SerializeField, Tooltip("減少間隔")]
        private int p_IncreaseFactor;

        /// <summary>
        /// 自動増加係数
        /// </summary>
        public int IncreaseFactor => p_IncreaseFactor;

        public HoloMonShitSetting()
        {
        }
    }
}
