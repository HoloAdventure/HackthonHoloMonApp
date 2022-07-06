using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToBillBoard
{
    /// <summary>
    /// ビルボードAPI
    /// </summary>
    public class HoloMonBodyComponentsToBillBoardAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBillboard p_Billboard;
        public HoloMonBillboard Billboard => p_Billboard;
    }
}