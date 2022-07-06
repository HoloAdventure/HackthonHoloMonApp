using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.Standby
{
    [Serializable]
    public class PurposeStandbyData : PurposeDataInterface
    {
        public PurposeStandbyData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.Standby;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}