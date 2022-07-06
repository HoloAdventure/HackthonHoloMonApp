using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToCollider
{
    /// <summary>
    /// コライダーAPI
    /// </summary>
    public class HoloMonBodyComponentsToColliderAPI : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider p_Collider;
        public BoxCollider Collider => p_Collider;
    }
}