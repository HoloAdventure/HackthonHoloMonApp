using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToObjectManipulator
{
    /// <summary>
    /// オブジェクトマニピュレータ操作API
    /// </summary>
    public class HoloMonBodyComponentsToObjectManipulatorAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonObjectManipulatorControl p_ObjectManipulatorControl;
        public HoloMonObjectManipulatorControl ObjectManipulatorControl => p_ObjectManipulatorControl;
    }
}