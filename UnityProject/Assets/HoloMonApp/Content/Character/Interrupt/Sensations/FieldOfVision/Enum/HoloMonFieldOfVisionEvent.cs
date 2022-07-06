using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision
{
    /// <summary>
    /// ホロモンが認識する視界イベント種別
    /// </summary>
    public enum HoloMonFieldOfVisionEvent
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
        /// <summary>
        /// 距離変化検出イベント
        /// </summary>
        UpdateRange,
    }

}
