using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    [Serializable]
    public class HeadLogicDontLookAtTargetData : HeadLogicDataInterface
    {
        /// <summary>
        /// 非追跡オブジェクト
        /// </summary>
        [SerializeField, Tooltip("非追跡オブジェクト")]
        private GameObject p_TargetObject;

        public GameObject TargetObject => p_TargetObject;

        public HeadLogicDontLookAtTargetData(GameObject a_TargetObject)
        {
            p_TargetObject = a_TargetObject;
        }
    }
}