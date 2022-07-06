using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.WorldItems.Stand
{
    /// <summary>
    /// スタンドアイテムのAPI
    /// </summary>
    public class HoloMonItemStandAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("スタンドコントローラの参照")]
        private HoloMonStandController p_StandController;

        public void ResetPosition(Vector3 a_ResetWorldPosition)
        {
            p_StandController.ResetPosition(a_ResetWorldPosition);
        }
    }
}