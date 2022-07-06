using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Interrupt.Conditions.Body;
using HoloMonApp.Content.Character.Interrupt.Conditions.Life;

namespace HoloMonApp.Content.Character.Interrupt.Conditions
{
    public class HoloMonInterruptConditionsAPI : MonoBehaviour, HoloMonInterruptIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonInterruptReference p_Reference;

        /// <summary>
        /// 初期化
        /// </summary>
        public void AwakeInit(HoloMonInterruptReference reference)
        {
            p_Reference = reference;
            foreach (HoloMonInterruptIF instance in p_ListIF)
            {
                instance.AwakeInit(p_Reference);
            }
        }

        /// <summary>
        /// ホロモン身体コンディションAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptConditionsBodyAPI p_ConditionsBody;

        /// <summary>
        /// ホロモンライフコンディションAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptConditionsLifeAPI p_ConditionsLife;


        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        private List<HoloMonInterruptIF> p_ListIF
            => p_ListIFInstance
            ?? (p_ListIFInstance = new List<HoloMonInterruptIF>()
            {
                p_ConditionsBody,
                p_ConditionsLife,
            });

        /// <summary>
        /// インタフェースの参照リスト(実体)
        /// </summary>
        private List<HoloMonInterruptIF> p_ListIFInstance;
    }
}