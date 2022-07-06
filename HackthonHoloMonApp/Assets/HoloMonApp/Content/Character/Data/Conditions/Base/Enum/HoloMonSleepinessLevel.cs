using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Data.Conditions
{
    /// <summary>
    /// ホロモンの眠り度を表す定義
    /// </summary>
    public enum HoloMonSleepinessLevel
    {
        /// <summary>
        /// デフォルト(眠気無し)
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 眠い
        /// </summary>
        Sleepy,
        /// <summary>
        /// うとうと
        /// </summary>
        Drowsy,
    }
}
