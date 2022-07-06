using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.BodyComponents.ToCollider;

namespace HoloMonApp.Content.Character.View.BodyComponents.ToCollider
{
    /// <summary>
    /// コライダーAPI
    /// </summary>
    public class HoloMonViewBodyComponentsToColliderAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToColliderAPI p_BodyComponentsToColliderAPI;
    }
}