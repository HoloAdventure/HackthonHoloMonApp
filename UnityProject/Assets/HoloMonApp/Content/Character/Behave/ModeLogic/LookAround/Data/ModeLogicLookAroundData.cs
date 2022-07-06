using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.LookAround
{
    [Serializable]
    public class ModeLogicLookAroundData : ModeLogicDataInterface
    {
        [SerializeField, Tooltip("捜索オブジェクト種別")]
        private ObjectUnderstandType p_SearchObjectUnderstandType;

        /// <summary>
        /// 捜索オブジェクト種別
        /// </summary>
        public ObjectUnderstandType SearchObjectUnderstandType => p_SearchObjectUnderstandType;

        public ModeLogicLookAroundData(ObjectUnderstandType a_SearchObjectUnderstandType)
        {
            p_SearchObjectUnderstandType = a_SearchObjectUnderstandType;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.LookAround;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
