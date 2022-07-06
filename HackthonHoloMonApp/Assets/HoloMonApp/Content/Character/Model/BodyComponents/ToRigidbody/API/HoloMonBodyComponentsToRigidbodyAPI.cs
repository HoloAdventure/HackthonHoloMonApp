using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToRigidbody
{
    /// <summary>
    /// リジッドボディAPI
    /// </summary>
    public class HoloMonBodyComponentsToRigidbodyAPI : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody p_Rigidbody;
        public Rigidbody Rigidbody => p_Rigidbody;
    }
}