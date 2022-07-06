using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.None
{
    [Serializable]
    public class ModeLogicNoneReturn : ModeLogicReturnInterface
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
