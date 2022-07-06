using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.ItemSpace
{
    /// <summary>
    /// 注目物のモード
    /// </summary>
    [Serializable]
    public enum AttentionMode
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
