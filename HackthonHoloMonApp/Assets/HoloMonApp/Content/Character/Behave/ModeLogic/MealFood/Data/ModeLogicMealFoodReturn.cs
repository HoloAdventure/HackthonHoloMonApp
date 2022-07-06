using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.MealFood
{
    [Serializable]
    public class ModeLogicMealFoodReturn : ModeLogicReturnInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.MealFood;
        }

        public ModeLogicReturnInterface GetModeLogicReturn()
        {
            return this;
        }
    }
}
