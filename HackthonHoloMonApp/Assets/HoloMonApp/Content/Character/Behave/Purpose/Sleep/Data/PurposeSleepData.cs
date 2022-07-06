using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.Sleep
{
    [Serializable]
    public class PurposeSleepData : PurposeDataInterface
    {
        public PurposeSleepData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.Sleep;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}