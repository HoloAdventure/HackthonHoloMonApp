using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.ItemSpace
{
    [Serializable]
    public enum StandMode
    {
        /// <summary>
        /// デフォルト(無効)
        /// </summary>
        Disable = 0,
        /// <summary>
        /// 表示
        /// </summary>
        Show = 1,
        /// <summary>
        /// 非表示
        /// </summary>
        Hide = 2,
    }
}
