using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose.RunFrom
{
    [Serializable]
    public class PurposeRunFromData : PurposeDataInterface
    {
        [SerializeField, Tooltip("逃亡対象種別")]
        private ObjectUnderstandType p_RunFromUnderstandType;

        /// <summary>
        /// 逃亡対象種別
        /// </summary>
        public ObjectUnderstandType RunFromUnderstandType => p_RunFromUnderstandType;

        public PurposeRunFromData(ObjectUnderstandType a_RunFromUnderstandType)
        {
            p_RunFromUnderstandType = a_RunFromUnderstandType;
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.RunFrom;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}