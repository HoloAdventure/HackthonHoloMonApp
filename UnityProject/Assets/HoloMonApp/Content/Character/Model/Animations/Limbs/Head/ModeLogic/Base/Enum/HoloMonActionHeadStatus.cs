using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    /// <summary>
    /// ホロモン頭部アクション状態
    /// </summary>
    public enum HoloMonActionHeadStatus
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 実行中
        /// </summary>
        Runtime,
        /// <summary>
        /// 停止中
        /// </summary>
        Stopping,
    }
}
