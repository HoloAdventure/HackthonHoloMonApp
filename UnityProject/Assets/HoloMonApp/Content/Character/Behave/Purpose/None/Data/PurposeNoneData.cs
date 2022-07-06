using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.None
{
    [Serializable]
    public class PurposeNoneData : PurposeDataInterface
    {
        public PurposeNoneData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.None;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}