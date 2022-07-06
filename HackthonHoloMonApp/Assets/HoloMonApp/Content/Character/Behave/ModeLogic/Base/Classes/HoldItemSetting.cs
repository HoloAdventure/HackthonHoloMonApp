using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    /// <summary>
    /// アイテム保持状態設定
    /// </summary>
    [Serializable]
    public class HoldItemSetting
    {
        /// <summary>
        /// 保持状態
        /// </summary>
        [SerializeField, Tooltip("保持状態")]
        private bool p_IsHold;

        public bool IsHold => p_IsHold;

        /// <summary>
        /// 保持アイテムの参照
        /// </summary>
        [SerializeField, Tooltip("保持状態")]
        private GameObject p_HoldItemObject;

        public GameObject HoldItemObject => p_HoldItemObject;

        public HoldItemSetting(GameObject a_HoldItemObject)
        {
            p_IsHold = (a_HoldItemObject != null);
            p_HoldItemObject = a_HoldItemObject;
        }
    }
}
