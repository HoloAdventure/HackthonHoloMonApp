using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Janken
{
    [Serializable]
    public class ModeLogicJankenData : ModeLogicDataInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.Janken;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
