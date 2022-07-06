using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Control.ObjectInteraction.HoldItem;
using HoloMonApp.Content.Character.Control.ObjectInteraction.RideItem;

namespace HoloMonApp.Content.Character.Control.ObjectInteraction
{
    /// <summary>
    /// オブジェクト相互作用API
    /// </summary>
    public class HoloMonControlObjectInteractionAPI : MonoBehaviour
    {
        /// <summary>
        /// アイテム保持の参照
        /// </summary>
        [SerializeField, Tooltip("アイテム保持の参照")]
        private HoloMonControlHoldItemAPI p_HoldItem;

        public HoloMonControlHoldItemAPI HoldItem => p_HoldItem;

        /// <summary>
        /// アイテム搭乗の参照
        /// </summary>
        [SerializeField, Tooltip("アイテム搭乗の参照")]
        private HoloMonControlRideItemAPI p_RideItem;
        public HoloMonControlRideItemAPI RideItem => p_RideItem;
    }
}