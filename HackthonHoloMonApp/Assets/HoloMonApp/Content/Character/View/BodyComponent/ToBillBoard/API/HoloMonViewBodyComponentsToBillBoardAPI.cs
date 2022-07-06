using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.BodyComponents.ToBillBoard;

namespace HoloMonApp.Content.Character.View.BodyComponents.ToBillBoard
{
    /// <summary>
    /// ビルボードAPI
    /// </summary>
    public class HoloMonViewBodyComponentsToBillBoardAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToBillBoardAPI p_BodyComponentsToBillBoardAPI;

        public bool IsActive => p_BodyComponentsToBillBoardAPI.Billboard.isActive;

        public float CurrentDiffTargetAngle => p_BodyComponentsToBillBoardAPI.Billboard.CurrentDiffTargetAngle();
    }
}