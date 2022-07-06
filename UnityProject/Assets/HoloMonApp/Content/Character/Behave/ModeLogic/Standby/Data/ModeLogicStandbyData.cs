using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Standby
{
    [Serializable]
    public class ModeLogicStandbyData : ModeLogicDataInterface
    {
        [SerializeField, Tooltip("初期注目オブジェクト")]
        private GameObject p_StartLookObject;

        /// <summary>
        /// 初期注目オブジェクト
        /// </summary>
        public GameObject StartLookObject => p_StartLookObject;

        public ModeLogicStandbyData(GameObject a_StartLookObject)
        {
            p_StartLookObject = a_StartLookObject;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.Standby;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
