using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    [Serializable]
    public class ModeLogicResult
    {
        /// <summary>
        /// モード最終状態
        /// </summary>
        [SerializeField, Tooltip("モード最終状態")]
        private HoloMonActionModeStatus p_FinishModeStatus;
        public HoloMonActionModeStatus FinishModeStatus => p_FinishModeStatus;

        /// <summary>
        /// ホロモンアクションモード種別
        /// </summary>
        [SerializeField, Tooltip("ホロモンアクションモード種別(確認種別)")]
        public HoloMonActionMode p_HoloMonActionMode;
        public HoloMonActionMode HoloMonActionMode => p_ModeLogicReturn.GetActionMode();

        /// <summary>
        /// ホロモン種別固有返却データ(dynamic)
        /// </summary>
        [SerializeField, Tooltip("種別固有返却")]
        private ModeLogicReturnInterface p_ModeLogicReturn;
        public ModeLogicReturnInterface ModeLogicReturn => p_ModeLogicReturn.GetModeLogicReturn();

        public ModeLogicResult(HoloMonActionModeStatus a_FinishModeStatus, ModeLogicReturnInterface a_Return)
        {
            p_ModeLogicReturn = a_Return;
            p_HoloMonActionMode = a_Return.GetActionMode();
            p_FinishModeStatus = a_FinishModeStatus;
        }
    }
}