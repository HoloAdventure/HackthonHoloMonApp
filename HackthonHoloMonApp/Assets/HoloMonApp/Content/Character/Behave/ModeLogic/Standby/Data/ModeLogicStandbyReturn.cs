using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Standby
{
    [Serializable]
    public class ModeLogicStandbyReturn : ModeLogicReturnInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.Standby;
        }

        public ModeLogicReturnInterface GetModeLogicReturn()
        {
            return this;
        }
    }
}
