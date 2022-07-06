using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.LookAround
{
    [Serializable]
    public class ModeLogicLookAroundReturn : ModeLogicReturnInterface
    {
        [SerializeField, Tooltip("発見オブジェクト")]
        private GameObject p_FindedObject;

        /// <summary>
        /// 発見オブジェクトの参照用変数
        /// </summary>
        public GameObject FindedObject => p_FindedObject;

        public ModeLogicLookAroundReturn(
            GameObject a_FindedObject = null
            )
        {
            p_FindedObject = a_FindedObject;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.LookAround;
        }

        public ModeLogicReturnInterface GetModeLogicReturn()
        {
            return this;
        }
    }
}
