using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.RunFromTarget
{
    [Serializable]
    public class ModeLogicRunFromTargetData : ModeLogicDataInterface
    {
        [SerializeField, Tooltip("逃走対象オブジェクト")]
        private GameObject p_TargetObject;

        /// <summary>
        /// 逃走対象オブジェクト
        /// </summary>
        public GameObject TargetObject => p_TargetObject;

        [SerializeField, Tooltip("逃走設定")]
        private RunSetting p_RunSetting;

        /// <summary>
        /// 追跡設定
        /// </summary>
        public RunSetting RunSetting => p_RunSetting;

        [SerializeField, Tooltip("アイテム保持設定")]
        private HoldItemSetting p_HoldItemSetting;

        /// <summary>
        /// アイテム保持設定
        /// </summary>
        public HoldItemSetting HoldItemSetting => p_HoldItemSetting;

        public ModeLogicRunFromTargetData(GameObject a_TargetObject,
            RunSetting a_RunSetting, HoldItemSetting a_HoldItemSetting)
        {
            p_TargetObject = a_TargetObject;
            p_RunSetting = a_RunSetting;
            p_HoldItemSetting = a_HoldItemSetting;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.RunFromTarget;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
