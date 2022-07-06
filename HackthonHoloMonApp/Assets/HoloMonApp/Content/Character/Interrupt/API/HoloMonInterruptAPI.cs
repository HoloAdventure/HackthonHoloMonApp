using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using HoloMonApp.Content.Character.Interrupt.Commands;
using HoloMonApp.Content.Character.Interrupt.Conditions;
using HoloMonApp.Content.Character.Interrupt.Sensations;

namespace HoloMonApp.Content.Character.Interrupt
{
    /// <summary>
    /// ホロモンの行動API
    /// </summary>
    [RequireComponent(typeof(HoloMonInterruptReference))]
    public class HoloMonInterruptAPI : MonoBehaviour
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonInterruptReference p_Reference;

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            p_Reference = this.GetComponent<HoloMonInterruptReference>();
            foreach (HoloMonInterruptIF instance in p_ListIF)
            {
                instance.AwakeInit(p_Reference);
            }
        }


        /// <summary>
        /// ホロモンのコマンドAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptCommandsAPI p_InterruptCommands;

        /// <summary>
        /// ホロモンのコンディションAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptConditionsAPI p_InterruptConditions;

        /// <summary>
        /// ホロモンの感覚APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonInterruptSensationsAPI p_InterruptSensations;


        // 参照用短縮変数
        public HoloMonInterruptCommandsAPI InterruptCommandsAPI => p_InterruptCommands;


        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        private List<HoloMonInterruptIF> p_ListIF
            => p_ListIFInstance
            ?? (p_ListIFInstance = new List<HoloMonInterruptIF>()
            {
                p_InterruptCommands,
                p_InterruptConditions,
                p_InterruptSensations,
            });

        /// <summary>
        /// インタフェースの参照リスト(実体)
        /// </summary>
        private List<HoloMonInterruptIF> p_ListIFInstance;
    }
}