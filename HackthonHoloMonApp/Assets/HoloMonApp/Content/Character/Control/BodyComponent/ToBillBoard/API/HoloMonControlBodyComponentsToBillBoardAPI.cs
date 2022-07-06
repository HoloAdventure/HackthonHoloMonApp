using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.BodyComponents.ToBillBoard;

namespace HoloMonApp.Content.Character.Control.BodyComponents.ToBillBoard
{
    /// <summary>
    /// ビルボードAPI
    /// </summary>
    public class HoloMonControlBodyComponentsToBillBoardAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToBillBoardAPI p_BodyComponentsToBillBoardAPI;

        public void SetTargetTransform(Transform a_TargetTransform)
        {
            p_BodyComponentsToBillBoardAPI.Billboard.TargetTransform = a_TargetTransform;
        }

        public void SetIsActive(bool a_onoff)
        {
            p_BodyComponentsToBillBoardAPI.Billboard.isActive = a_onoff;
        }

        public void SetIsYAxisInverse(bool a_onoff)
        {
            p_BodyComponentsToBillBoardAPI.Billboard.isYAxisInverse = a_onoff;
        }
    }
}