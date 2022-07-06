using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.HungUp
{
    [Serializable]
    public class PurposeHungUpData : PurposeDataInterface
    {
        public PurposeHungUpData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.HungUp;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}