using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Model.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.Data.Conditions;
using HoloMonApp.Content.Character.Data.Conditions.Life;

namespace HoloMonApp.Content.Character.View.Parameters.Conditions.Life
{
    /// <summary>
    /// ライフコンディションのAPI
    /// </summary>
    public class HoloMonViewConditionsLifeAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonConditionsLifeAPI p_ConditionsLifeAPI;

        /// <summary>
        /// ホロモンのライフコンディションのReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonLifeStatus> ReactivePropertyStatus
            => p_ConditionsLifeAPI.ReactivePropertyStatus;

        /// <summary>
        /// ホロモンのライフコンディションの短縮参照用変数
        /// </summary>
        public HoloMonLifeStatus LifeStatus => p_ConditionsLifeAPI.ReactivePropertyStatus.Value;

        /// <summary>
        /// ホロモンの空腹度の短縮参照用変数
        /// </summary>
        public int HungryPercent => p_ConditionsLifeAPI.HungryPercent;

        /// <summary>
        /// ホロモンの機嫌度の短縮参照用変数
        /// </summary>
        public int HumorPercent => p_ConditionsLifeAPI.HumorPercent;

        /// <summary>
        /// ホロモンの元気度の短縮参照用変数
        /// </summary>
        public int StaminaPercent => p_ConditionsLifeAPI.StaminaPercent;

        /// <summary>
        /// ホロモンのうんち度の短縮参照用変数
        /// </summary>
        public int PoopPercent => p_ConditionsLifeAPI.PoopPercent;

        /// <summary>
        /// ホロモンの眠気度の短縮参照用変数
        /// </summary>
        public HoloMonSleepinessLevel SleepinessLevel => p_ConditionsLifeAPI.SleepinessLevel;
    }
}