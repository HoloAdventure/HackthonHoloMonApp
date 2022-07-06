using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Model.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.Data.Conditions.Body;

namespace HoloMonApp.Content.Character.Control.Parameters.Conditions.Body
{
    /// <summary>
    /// ボディコンディションのAPI
    /// </summary>
    public class HoloMonControlConditionsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonConditionsBodyAPI p_ConditionsBodyAPI;

        /// <summary>
        /// ボディコンディションデータの反映
        /// </summary>
        public void ApplyBodyStatus(HoloMonBodyStatus a_BodyStatus)
        {
            p_ConditionsBodyAPI.ApplyBodyStatus(a_BodyStatus);
        }

        /// <summary>
        /// 身長を設定する
        /// </summary>
        /// <param name="a_Height"></param>
        public void SetHeight(float a_Height)
        {
            // 身長を設定する
            p_ConditionsBodyAPI.SetHeight(a_Height);
        }

        /// <summary>
        /// ちからを設定する
        /// </summary>
        /// <param name="a_Power"></param>
        public void SetPower(float a_Power)
        {
            // ちからを設定する
            p_ConditionsBodyAPI.SetPower(a_Power);
        }

        /// <summary>
        /// 身長を加算する
        /// </summary>
        /// <param name="a_Value"></param>
        public void AddHeight(float a_Value)
        {
            // 現在の身長を取得する
            float currentHeight = p_ConditionsBodyAPI.BodyHeight;

            // 強さを加算する
            float applyHeight = currentHeight + a_Value;

            // 0 ～ の範囲で設定する
            if (applyHeight < 0) applyHeight = 0;

            // 身長を設定する
            p_ConditionsBodyAPI.SetHeight(applyHeight);
        }

        /// <summary>
        /// 強さを加算する
        /// </summary>
        /// <param name="a_Value"></param>
        public void AddPower(float a_Value)
        {
            // 現在の強さを取得する
            float currentPower = p_ConditionsBodyAPI.BodyPower;

            // 強さを加算する
            float applyPower = currentPower + a_Value;

            // 0 ～ の範囲で設定する
            if (applyPower < 0) applyPower = 0;

            // ちからを設定する
            p_ConditionsBodyAPI.SetPower(applyPower);
        }
    }
}