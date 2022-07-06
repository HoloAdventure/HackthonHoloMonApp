using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.RunFromTarget
{
    /// <summary>
    /// 逃走設定
    /// </summary>
    [Serializable]
    public class RunSetting
    {
        [SerializeField, Tooltip("移動速度")]
        private float p_MoveSpeed;

        /// <summary>
        /// 速度
        /// </summary>
        public float MoveSpeed => p_MoveSpeed;

        [SerializeField, Tooltip("回転速度")]
        private float p_RotateSpeed;

        /// <summary>
        /// 回転速度
        /// </summary>
        public float RotateSpeed => p_RotateSpeed;

        [SerializeField, Tooltip("停止距離")]
        private float p_StoppingDistance;

        /// <summary>
        /// 停止距離
        /// </summary>
        public float StoppingDistance => p_StoppingDistance;

        [SerializeField, Tooltip("ターゲット判定位置種別")]
        private RunFromTargetCheckPosition p_RunCheckPosition;

        /// <summary>
        /// ターゲット判定位置種別
        /// </summary>
        public RunFromTargetCheckPosition RunCheckPosition => p_RunCheckPosition;

        public RunSetting(
            float a_Speed, float a_AngleSpeed,
            float a_StoppingDistance,
            RunFromTargetCheckPosition a_RunCheckPosition)
        {
            p_MoveSpeed = a_Speed;
            p_RotateSpeed = a_AngleSpeed;
            p_StoppingDistance = a_StoppingDistance;
            p_RunCheckPosition = a_RunCheckPosition;
        }
    }
}
