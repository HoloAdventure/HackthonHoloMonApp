using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    [Serializable]
    public class HeadLogicLookAtTargetData : HeadLogicDataInterface
    {
        /// <summary>
        /// 追跡オブジェクト
        /// </summary>
        [SerializeField, Tooltip("追跡オブジェクト")]
        private GameObject p_TargetObject;

        public GameObject TargetObject => p_TargetObject;

        public HeadLogicLookAtTargetData(GameObject a_TargetObject)
        {
            p_TargetObject = a_TargetObject;
        }
    }
}