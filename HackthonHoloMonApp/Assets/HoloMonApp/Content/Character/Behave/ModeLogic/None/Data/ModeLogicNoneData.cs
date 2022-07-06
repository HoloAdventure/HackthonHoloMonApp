using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.None
{
    [Serializable]
    public class ModeLogicNoneData : ModeLogicDataInterface
    {
        public ModeLogicNoneData()
        {
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.None;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
