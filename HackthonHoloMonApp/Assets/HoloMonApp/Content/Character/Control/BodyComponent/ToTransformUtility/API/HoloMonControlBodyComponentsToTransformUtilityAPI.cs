using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.BodyComponents.ToTransformUtility;

namespace HoloMonApp.Content.Character.Control.BodyComponents.ToTransformUtility
{
    /// <summary>
    /// トランスフォーム特殊操作API
    /// </summary>
    public class HoloMonControlBodyComponentsToTransformUtilityAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToTransformUtilityAPI p_BodyComponentsToTransformUtilityAPI;

        public void SetPosition(Vector3 a_Position)
        {
            p_BodyComponentsToTransformUtilityAPI.TransformUtility.SetPosition(a_Position);
        }

        public void SetRotation(Quaternion a_Rotation)
        {
            p_BodyComponentsToTransformUtilityAPI.TransformUtility.SetRotation(a_Rotation);
        }

        public void SetScaleSmooth(float a_Scale)
        {
            p_BodyComponentsToTransformUtilityAPI.TransformUtility.SetScaleSmooth(a_Scale);
        }
    }
}