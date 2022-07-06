using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.PutoutPoop
{
    [Serializable]
    public class PurposePutoutPoopData : PurposeDataInterface
    {
        public PurposePutoutPoopData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.PutoutPoop;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}