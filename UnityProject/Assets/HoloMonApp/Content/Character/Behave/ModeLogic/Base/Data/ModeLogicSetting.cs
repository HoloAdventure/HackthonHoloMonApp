using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Behave.ModeLogic.Dance;
using HoloMonApp.Content.Character.Behave.ModeLogic.HungUp;
using HoloMonApp.Content.Character.Behave.ModeLogic.Janken;
using HoloMonApp.Content.Character.Behave.ModeLogic.LookAround;
using HoloMonApp.Content.Character.Behave.ModeLogic.MealFood;
using HoloMonApp.Content.Character.Behave.ModeLogic.RunFromTarget;
using HoloMonApp.Content.Character.Behave.ModeLogic.PutoutPoop;
using HoloMonApp.Content.Character.Behave.ModeLogic.SitDown;
using HoloMonApp.Content.Character.Behave.ModeLogic.Sleep;
using HoloMonApp.Content.Character.Behave.ModeLogic.Standby;
using HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTarget;
using HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh;
using HoloMonApp.Content.Character.Behave.ModeLogic.TurnTarget;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    [Serializable]
    public class ModeLogicSetting
    {
        /// <summary>
        /// ホロモンアクションモード種別
        /// </summary>
        [SerializeField, Tooltip("ホロモンアクションモード種別(確認種別)")]
        private HoloMonActionMode p_HoloMonActionMode;
        public HoloMonActionMode HoloMonActionMode => p_ModeLogicData.GetActionMode();

        /// <summary>
        /// ホロモン種別固有設定データ(dynamic)
        /// </summary>
        [SerializeField, Tooltip("種別固有設定")]
        private ModeLogicDataInterface p_ModeLogicData;
        public ModeLogicDataInterface ModeLogicData => p_ModeLogicData.GetModeLogicData();

        public ModeLogicSetting(ModeLogicDataInterface a_Data)
        {
            p_ModeLogicData = a_Data;
            p_HoloMonActionMode = a_Data.GetActionMode();
        }
    }
}