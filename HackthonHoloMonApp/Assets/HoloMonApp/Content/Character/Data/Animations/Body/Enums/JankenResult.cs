using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Data.Animations.Body
{
    /// <summary>
    /// じゃんけん結果定義
    /// </summary>
    public enum JankenResult
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// ホロモンの勝ち
        /// </summary>
        HoloMonWin = 1,
        /// <summary>
        /// ホロモンの負け
        /// </summary>
        HoloMonLose = 2,
        /// <summary>
        /// ホロモンの引き分け
        /// </summary>
        HoloMonDraw = 3,
    }
}
