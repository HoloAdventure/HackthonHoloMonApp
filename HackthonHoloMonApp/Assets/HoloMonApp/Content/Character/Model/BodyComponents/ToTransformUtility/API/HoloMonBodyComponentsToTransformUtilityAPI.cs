using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToTransformUtility
{
    /// <summary>
    /// トランスフォーム特殊操作API
    /// </summary>
    public class HoloMonBodyComponentsToTransformUtilityAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonTransformUtility p_TransformUtility;
        public HoloMonTransformUtility TransformUtility => p_TransformUtility;
    }
}