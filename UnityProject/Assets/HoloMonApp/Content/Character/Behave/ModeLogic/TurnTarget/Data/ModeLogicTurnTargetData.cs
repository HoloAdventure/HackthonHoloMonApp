using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TurnTarget
{
    [Serializable]
    public class ModeLogicTurnTargetData : ModeLogicDataInterface
    {
        [SerializeField, Tooltip("追跡オブジェクト")]
        private GameObject p_TargetObject;

        [SerializeField, Tooltip("正面を向いたときに達成と判定するか")]
        private bool p_IsAchievementFront;

        [SerializeField, Tooltip("正面判定角度")]
        private float p_CheckFrontAngle;

        [SerializeField, Tooltip("オブジェクトの状態変化で追跡をやめるか")]
        private bool p_IsCheckObjectStatus;

        [SerializeField, Tooltip("待機の合図解除のチェック有無")]
        private bool p_CheckReleaseSignal;

        [SerializeField, Tooltip("回転の遠距離解除の距離設定（-1 時チェックしない）")]
        private float p_CheckFarDistance;

        /// <summary>
        /// 追跡オブジェクト
        /// </summary>
        public GameObject TargetObject => p_TargetObject;

        /// <summary>
        /// 正面を向いたときに達成と判定するか
        /// </summary>
        public bool IsAchievementFront => p_IsAchievementFront;

        /// <summary>
        /// 正面判定角度
        /// </summary>
        public float CheckFrontAngle => p_CheckFrontAngle;

        /// <summary>
        /// オブジェクトの状態変化で追跡をやめるか
        /// </summary>
        public bool IsCheckObjectStatus => p_IsCheckObjectStatus;

        /// <summary>
        /// 回転の遠距離解除の距離設定（-1 時チェックしない）
        /// </summary>
        public float CheckFarDistance => p_CheckFarDistance;

        public ModeLogicTurnTargetData(
            GameObject a_TargetObject,
            bool a_IsAchievementFront,
            float a_CheckFrontAngle,
            bool a_IsCheckObjectStatus,
            float a_CheckFarDistance)
        {
            p_TargetObject = a_TargetObject;
            p_IsAchievementFront = a_IsAchievementFront;
            p_CheckFrontAngle = a_CheckFrontAngle;
            p_IsCheckObjectStatus = a_IsCheckObjectStatus;
            p_CheckFarDistance = a_CheckFarDistance;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.TurnTarget;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
