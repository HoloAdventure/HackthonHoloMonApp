using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.PutoutPoop
{
    [Serializable]
    public class ModeLogicPutoutPoopData : ModeLogicDataInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.PutoutPoop;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
