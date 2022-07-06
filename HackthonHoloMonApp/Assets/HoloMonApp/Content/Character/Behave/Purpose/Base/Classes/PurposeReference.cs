using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Behave.Purpose
{
    [Serializable]
    public class PurposeReference
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonBehaveReference p_BehaveReference;

        /// <summary>
        /// アクションモードAPIの参照
        /// </summary>
        private HoloMonActionModeLogicAPI p_ActionModeLogicAPI;
        public HoloMonActionModeLogicAPI ActionModeLogic => p_ActionModeLogicAPI;

        // 参照用短縮変数
        public HoloMonControlAPI Control => p_BehaveReference.Control;

        public HoloMonViewAPI View => p_BehaveReference.View;


        public PurposeReference(
            HoloMonBehaveReference behaveReference,
            HoloMonActionModeLogicAPI actionModeLogicAPI)
        {
            p_BehaveReference = behaveReference;
            p_ActionModeLogicAPI = actionModeLogicAPI;
        }
    }
}