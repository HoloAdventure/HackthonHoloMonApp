using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.HungUp
{
    [Serializable]
    public class ModeLogicHungUpData : ModeLogicDataInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.HungUp;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
