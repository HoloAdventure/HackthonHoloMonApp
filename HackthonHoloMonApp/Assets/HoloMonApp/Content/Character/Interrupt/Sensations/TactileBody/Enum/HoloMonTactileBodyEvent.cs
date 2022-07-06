using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.TactileBody
{
    /// <summary>
    /// ホロモンが認識する触覚イベント種別
    /// </summary>
    public enum HoloMonTactileBodyEvent
    {
        /// <summary>
        /// デフォルト値
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 発見イベント
        /// </summary>
        Add,
        /// <summary>
        /// ロストイベント
        /// </summary>
        Lost,
        /// <summary>
        /// 状態変化検出イベント
        /// </summary>
        UpdateStatus,
    }

}
