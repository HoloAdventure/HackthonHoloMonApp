using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToProblemEvent
{
    /// <summary>
    /// 問題検知API
    /// </summary>
    public class HoloMonBodyComponentsToProblemEventAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonFallDownReset p_FallDownReset;
        public HoloMonFallDownReset FallDownReset => p_FallDownReset;
    }
}