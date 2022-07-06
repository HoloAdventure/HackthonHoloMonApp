using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.BodyComponents.ToRigidbody;

namespace HoloMonApp.Content.Character.Control.BodyComponents.ToRigidbody
{
    /// <summary>
    /// リジッドボディAPI
    /// </summary>
    public class HoloMonControlBodyComponentsToRigidbodyAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToRigidbodyAPI p_BodyComponentsToRigidbodyAPI;

        public void SetUseGravity(bool a_onoff)
        {
            // 重力の有無を切り替える（同時に慣性はリセットする）
            p_BodyComponentsToRigidbodyAPI.Rigidbody.useGravity = a_onoff;
            p_BodyComponentsToRigidbodyAPI.Rigidbody.velocity = Vector3.zero;
            p_BodyComponentsToRigidbodyAPI.Rigidbody.angularVelocity = Vector3.zero;
        }

        public void ResetVelocity()
        {
            p_BodyComponentsToRigidbodyAPI.Rigidbody.velocity = Vector3.zero;
            p_BodyComponentsToRigidbodyAPI.Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}