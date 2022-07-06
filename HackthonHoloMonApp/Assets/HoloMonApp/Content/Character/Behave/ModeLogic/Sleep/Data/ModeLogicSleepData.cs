using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Sleep
{
    [Serializable]
    public class ModeLogicSleepData : ModeLogicDataInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.Sleep;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
