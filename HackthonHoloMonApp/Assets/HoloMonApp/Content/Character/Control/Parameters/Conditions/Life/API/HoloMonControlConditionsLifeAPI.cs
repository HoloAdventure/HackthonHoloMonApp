using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Model.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.Data.Conditions.Life;

namespace HoloMonApp.Content.Character.Control.Parameters.Conditions.Life
{
    /// <summary>
    /// ライフコンディションのAPI
    /// </summary>
    public class HoloMonControlConditionsLifeAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonConditionsLifeAPI p_ConditionsLifeAPI;

        /// <summary>
        /// ライフコンディションデータの反映
        /// </summary>
        public void ApplyLifeStatus(HoloMonLifeStatus a_LifeStatus)
        {
            p_ConditionsLifeAPI.ApplyLifeStatus(a_LifeStatus);
        }

        /// <summary>
        /// 空腹度を加算する
        /// </summary>
        /// <param name="a_Value"></param>
        public void AddHungry(int a_Value)
        {
            // 現在の空腹度を取得する
            int currentHungry = p_ConditionsLifeAPI.HungryPercent;

            // 空腹度を加算する
            int applyHungry = currentHungry + a_Value;

            // 空腹度を設定する
            p_ConditionsLifeAPI.SetHungry(applyHungry);
        }

        /// <summary>
        /// 機嫌度を加算する
        /// </summary>
        /// <param name="a_Value"></param>
        public void AddHumor(int a_Value)
        {
            // 現在の機嫌度を取得する
            int currentHumor = p_ConditionsLifeAPI.HumorPercent;

            // 機嫌度を加算する
            int applyHumor = currentHumor + a_Value;

            // 機嫌度を設定する
            p_ConditionsLifeAPI.SetHumor(applyHumor);
        }

        /// <summary>
        /// 元気度を加算する
        /// </summary>
        /// <param name="a_Value"></param>
        public void AddStamina(int a_Value)
        {
            // 現在の元気度を取得する
            int currentStamina = p_ConditionsLifeAPI.StaminaPercent;

            // 元気度を加算する
            int applyStamina = currentStamina + a_Value;

            // 元気度を設定する
            p_ConditionsLifeAPI.SetStamina(applyStamina);
        }

        /// <summary>
        /// うんち度を加算する
        /// </summary>
        /// <param name="a_Value"></param>
        public void AddPoop(int a_Value)
        {
            // 現在のうんち度を取得する
            int currentPoop = p_ConditionsLifeAPI.PoopPercent;

            // うんち度を加算する
            int applyPoop = currentPoop + a_Value;

            // うんち度を設定する
            p_ConditionsLifeAPI.SetPoop(applyPoop);
        }
    }
}