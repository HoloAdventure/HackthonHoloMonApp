using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.MealFood
{
    [Serializable]
    public class ModeLogicMealFoodData : ModeLogicDataInterface
    {
        [SerializeField, Tooltip("フードオブジェクト")]
        private GameObject p_FoodObject;

        /// <summary>
        /// フードオブジェクト
        /// </summary>
        public GameObject FoodObject => p_FoodObject;

        public ModeLogicMealFoodData(GameObject a_FoodObject)
        {
            p_FoodObject = a_FoodObject;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.MealFood;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
