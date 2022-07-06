using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Control.WorldItems.Stand;

namespace HoloMonApp.Content.Character.Control.WorldItems
{
    /// <summary>
    /// スタンドアイテムののAPI
    /// </summary>
    public class HoloMonControlSupportItemAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("スタンドアイテムの参照")]
        private HoloMonControlItemStandAPI p_Stand;

        /// <summary>
        /// スタンドアイテムの参照
        /// </summary>
        public HoloMonControlItemStandAPI Stand => p_Stand;
    }
}