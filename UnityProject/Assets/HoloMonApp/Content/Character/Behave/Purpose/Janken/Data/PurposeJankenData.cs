using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.Janken
{
    [Serializable]
    public class PurposeJankenData : PurposeDataInterface
    {
        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.Janken;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}