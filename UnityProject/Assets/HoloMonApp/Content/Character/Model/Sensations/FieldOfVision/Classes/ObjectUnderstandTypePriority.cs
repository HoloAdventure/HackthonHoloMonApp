using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    [Serializable]
    public class ObjectUnderstandTypePriority
    {
        public ObjectUnderstandTypePriority(ObjectUnderstandType a_UnderstandType, int a_Priority)
        {
            this.p_ObjectUnderstandType = a_UnderstandType;
            this.p_Priority = a_Priority;
        }

        [SerializeField, Tooltip("オブジェクトの理解種別")]
        private ObjectUnderstandType p_ObjectUnderstandType;

        /// <summary>
        /// オブジェクトの理解種別(読み取り専用)
        /// </summary>
        public ObjectUnderstandType ObjectUnderstandType => p_ObjectUnderstandType;

        [SerializeField, Tooltip("優先度")]
        private int p_Priority;

        /// <summary>
        /// 優先度(読み取り専用)
        /// </summary>
        public int Priority => p_Priority;
    }
}