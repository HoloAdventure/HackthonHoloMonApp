using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.Dance
{
    [Serializable]
    public class PurposeDanceData : PurposeDataInterface
    {
        public PurposeDanceData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.Dance;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}