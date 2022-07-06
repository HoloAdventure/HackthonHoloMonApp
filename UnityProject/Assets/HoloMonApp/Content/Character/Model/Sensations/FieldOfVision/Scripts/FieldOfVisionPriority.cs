using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    public class FieldOfVisionPriority : MonoBehaviour
    {
        /// <summary>
        /// オブジェクト理解種別ごとの優先度リスト
        /// </summary>
        [SerializeField, Tooltip("オブジェクト理解種別ごとの優先度リスト")]
        private List<ObjectUnderstandTypePriority> p_ObjectUnderstandTypePriorityList;
        public IEnumerable<ObjectUnderstandTypePriority> PriorityList => p_ObjectUnderstandTypePriorityList;
    }
}