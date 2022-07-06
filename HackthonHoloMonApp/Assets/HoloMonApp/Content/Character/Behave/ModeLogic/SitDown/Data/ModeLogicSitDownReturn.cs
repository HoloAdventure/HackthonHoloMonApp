using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.SitDown
{
    [Serializable]
    public class ModeLogicSitDownReturn : ModeLogicReturnInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.SitDown;
        }

        public ModeLogicReturnInterface GetModeLogicReturn()
        {
            return this;
        }
    }
}
