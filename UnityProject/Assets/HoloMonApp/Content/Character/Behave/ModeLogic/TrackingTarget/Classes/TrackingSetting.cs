using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTarget
{
    /// <summary>
    /// 追跡設定
    /// </summary>
    [Serializable]
    public class TrackingSetting
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

        [SerializeField, Tooltip("トラッキング到達判定位置")]
        private TrackingTargetCheckPosition p_ArrivalCheckPosition;

        /// <summary>
        /// トラッキング到達判定位置
        /// </summary>
        public TrackingTargetCheckPosition ArrivalCheckPosition => p_ArrivalCheckPosition;

        public TrackingSetting()
        {
            p_MoveSpeed = 1.0f;
            p_RotateSpeed = 1.0f;
            p_StoppingDistance = 1.0f;
            p_ArrivalCheckPosition = TrackingTargetCheckPosition.ObjectOrigin;
        }

        public TrackingSetting(
            float a_Speed, float a_AngleSpeed,
            float a_StoppingDistance,
            TrackingTargetCheckPosition a_ArrivalCheckPosition)
        {
            p_MoveSpeed = a_Speed;
            p_RotateSpeed = a_AngleSpeed;
            p_StoppingDistance = a_StoppingDistance;
            p_ArrivalCheckPosition = a_ArrivalCheckPosition;
        }
    }
}
