using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using HoloMonApp.Content.Character.Behave.ModeLogic;

namespace HoloMonApp.Content.Character.Behave.Purpose
{
    /// <summary>
    /// ホロモンの目的行動API
    /// </summary>
    public class HoloMonPurposeBehaveReference : MonoBehaviour
    {
        /// <summary>
        /// アクションモードAPIの参照
        /// </summary>
        [SerializeField, Tooltip("アクションモードAPIの参照")]
        private HoloMonActionModeLogicAPI p_ActionModeLogic;
        public HoloMonActionModeLogicAPI ActionModeLogic => p_ActionModeLogic;
    }
}