using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.StayWait
{
    [Serializable]
    public class PurposeStayWaitData : PurposeDataInterface
    {
        public PurposeStayWaitData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.StayWait;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}