using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.WorldItems.Stand;

namespace HoloMonApp.Content.Character.Control.WorldItems.Stand
{
    /// <summary>
    /// スタンドアイテムのAPI
    /// </summary>
    public class HoloMonControlItemStandAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonItemStandAPI p_ItemStandAPI;

        public void ResetPosition(Vector3 a_ResetWorldPosition)
        {
            p_ItemStandAPI.ResetPosition(a_ResetWorldPosition);
        }
    }
}