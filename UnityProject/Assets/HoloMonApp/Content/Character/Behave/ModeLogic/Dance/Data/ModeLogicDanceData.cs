using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Dance
{
    [Serializable]
    public class ModeLogicDanceData : ModeLogicDataInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.Dance;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
