using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Conditions.Life
{
    /// <summary>
    /// ホロモンのライフコンディションの構造定義
    /// </summary>
    [Serializable]
    public class HoloMonLifeStatus
    {
        /// <summary>
        /// 初期化済みフラグ
        /// </summary>
        public bool Initialized = false;

        /// <summary>
        /// ホロモンの空腹度
        /// </summary>
        public StatusElementInt HungryPercent;

        /// <summary>
        /// ホロモンの機嫌度
        /// </summary>
        public StatusElementInt HumorPercent;

        /// <summary>
        /// ホロモンの元気度
        /// </summary>
        public StatusElementInt StaminaPercent;

        /// <summary>
        /// ホロモンのうんち度
        /// </summary>
        public StatusElementInt PoopPercent;

        /// <summary>
        /// ホロモンの眠気度
        /// </summary>
        public StatusElementSleepinessLevel SleepinessLevel;

        public HoloMonLifeStatus(
            int a_HungryPercent,
            int a_HumorPercent,
            int a_StaminaPercent,
            int a_PoopPercent,
            HoloMonSleepinessLevel a_SleepinessLevel
            )
        {
            HungryPercent = new StatusElementInt(a_HungryPercent);
            HumorPercent = new StatusElementInt(a_HumorPercent);
            StaminaPercent = new StatusElementInt(a_StaminaPercent);
            PoopPercent = new StatusElementInt(a_PoopPercent);
            SleepinessLevel = new StatusElementSleepinessLevel(a_SleepinessLevel);
        }

        // XmlSerializeのため、引数無しのコンストラクタを定義する
        private HoloMonLifeStatus() { }
    }
}
