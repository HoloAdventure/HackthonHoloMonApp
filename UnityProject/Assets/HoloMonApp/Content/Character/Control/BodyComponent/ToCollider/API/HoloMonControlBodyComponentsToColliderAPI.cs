using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.BodyComponents.ToCollider;

namespace HoloMonApp.Content.Character.Control.BodyComponents.ToCollider
{
    /// <summary>
    /// コライダーAPI
    /// </summary>
    public class HoloMonControlBodyComponentsToColliderAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToColliderAPI p_BodyComponentsToColliderAPI;

        public void SetEnabled(bool a_onoff)
        {
            p_BodyComponentsToColliderAPI.Collider.enabled = a_onoff;
        }

    }
}