using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose.MealFood
{
    [Serializable]
    public class PurposeMealFoodData : PurposeDataInterface
    {
        [SerializeField, Tooltip("フードオブジェクト種別情報")]
        private ObjectFeatureWrap p_FoodObjectData;

        /// <summary>
        /// フードオブジェクト
        /// </summary>
        public GameObject FoodObject => p_FoodObjectData?.GameObject;

        public PurposeMealFoodData(ObjectFeatureWrap a_FoodObjectData)
        {
            p_FoodObjectData = a_FoodObjectData;
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.MealFood;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}