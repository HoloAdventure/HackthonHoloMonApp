using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    [Serializable]
    public class HeadLogicSetting
    {
        [SerializeField, Tooltip("ホロモン頭部アクション種別")]
        private HoloMonActionHead p_HoloMonActionHead;

        public HoloMonActionHead HoloMonActionHead => p_HoloMonActionHead;

        [SerializeField]
        private HeadLogicDataInterface p_HeadLogicData;

        // キャスト参照
        public HeadLogicNoOverrideData HeadLogicNoOverrideData
            => (HeadLogicNoOverrideData)p_HeadLogicData;

        public HeadLogicLookAtTargetData HeadLogicLookAtTargetData
            => (HeadLogicLookAtTargetData)p_HeadLogicData;

        public HeadLogicDontLookAtTargetData HeadLogicDontLookAtTargetData
            => (HeadLogicDontLookAtTargetData)p_HeadLogicData;


        public HeadLogicSetting(HeadLogicNoOverrideData a_Data)
        {
            p_HoloMonActionHead = HoloMonActionHead.NoOverride;
            p_HeadLogicData = a_Data;
        }

        public HeadLogicSetting(HeadLogicLookAtTargetData a_Data)
        {
            p_HoloMonActionHead = HoloMonActionHead.LookAtTarget;
            p_HeadLogicData = a_Data;
        }

        public HeadLogicSetting(HeadLogicDontLookAtTargetData a_Data)
        {
            p_HoloMonActionHead = HoloMonActionHead.DontLookAtTarget;
            p_HeadLogicData = a_Data;
        }
    }
}