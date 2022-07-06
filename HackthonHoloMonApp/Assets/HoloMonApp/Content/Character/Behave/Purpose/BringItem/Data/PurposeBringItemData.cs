using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose.BringItem
{
    [Serializable]
    public struct PurposeBringItemData : PurposeDataInterface
    {
        [SerializeField, Tooltip("捜索対象種別")]
        private ObjectUnderstandType p_BringUnderstandType;

        /// <summary>
        /// 捜索対象種別
        /// </summary>
        public ObjectUnderstandType BringUnderstandType => p_BringUnderstandType;

        public PurposeBringItemData(ObjectUnderstandType a_BringUnderstandType)
        {
            p_BringUnderstandType = a_BringUnderstandType;
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.BringItem;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}