using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloMonApp.Content.Character.AI;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Tail.ModeLogic
{
    [Serializable]
    public class TailLogicSetting
    {
        [SerializeField, Tooltip("ホロモン尻尾種別")]
        private HoloMonActionTail p_HoloMonActionTail;

        public HoloMonActionTail HoloMonActionTail => p_HoloMonActionTail;

        [SerializeField]
        private TailLogicDataInterface p_TailLogicData;

        public TailLogicDataInterface TailLogicData => p_TailLogicData;

        public TailLogicSetting(TailLogicNoOverrideData a_Data)
        {
            p_HoloMonActionTail = HoloMonActionTail.NoOverride;
            p_TailLogicData = a_Data;
        }
    }
}