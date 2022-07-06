using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh
{
    /// <summary>
    /// NavMesh共有追跡設定
    /// </summary>
    [Serializable]
    public class TrackingSettingNavMesh
    {
        [Serializable]
        private class CheckFloat
        {
            public bool IsEnable;
            public float Value;

            public CheckFloat(bool a_IsEnable, float a_Value)
            {
                IsEnable = a_IsEnable;
                Value = a_Value;
            }
        }

        /// <summary>
        /// 速度
        /// </summary>
        [SerializeField, Tooltip("速度")]
        private CheckFloat Speed;

        /// <summary>
        /// 回転速度
        /// </summary>
        [SerializeField, Tooltip("回転速度")]
        private CheckFloat AngleSpeed;

        /// <summary>
        /// 停止距離
        /// </summary>
        [SerializeField, Tooltip("停止距離")]
        private CheckFloat StoppingDistance;

        public TrackingSettingNavMesh()
        {
            // 初期値は全て無効(false)な値として保持する
            Speed = new CheckFloat(false, 0.0f);
            AngleSpeed = new CheckFloat(false, 0.0f);
            StoppingDistance = new CheckFloat(false, 0.0f);
        }

        public TrackingSettingNavMesh(float a_Speed, float a_AngleSpeed, float a_StoppingDistance)
        {
            // 初期値は全て無効(false)な値として保持する
            Speed = new CheckFloat(false, 0.0f);
            AngleSpeed = new CheckFloat(false, 0.0f);
            StoppingDistance = new CheckFloat(false, 0.0f);
            // 有効な設定値のみ設定する
            if (a_Speed > 0.0f) Speed = new CheckFloat(true, a_Speed);
            if (a_AngleSpeed > 0.0f) AngleSpeed = new CheckFloat(true, a_AngleSpeed);
            if (a_StoppingDistance > 0.0f) StoppingDistance = new CheckFloat(true, a_StoppingDistance);
        }

        /// <summary>
        /// NavMeshAgentに有効な設定を反映する
        /// </summary>
        /// <param name="a_Agent"></param>
        public void ApplyNavMeshSetting(float a_Scale, NavMeshAgent a_Agent)
        {
            // 有効(true)な値のみ NavMeshAgent に設定を行う
            if (Speed.IsEnable) a_Agent.speed = Speed.Value;
            if (AngleSpeed.IsEnable) a_Agent.angularSpeed = AngleSpeed.Value;
            if (StoppingDistance.IsEnable) a_Agent.stoppingDistance = StoppingDistance.Value * a_Scale;
        }

        /// <summary>
        /// NavMeshAgentから有効な値を取得する
        /// </summary>
        /// <param name="a_Agent"></param>
        public void SetNavMeshSetting(float a_Scale, NavMeshAgent a_Agent)
        {
            // NavMeshAgent から設定を取得する
            Speed = new CheckFloat(true, a_Agent.speed);
            AngleSpeed = new CheckFloat(true, a_Agent.angularSpeed);
            StoppingDistance = new CheckFloat(true, a_Agent.stoppingDistance / a_Scale);
        }
    }
}
