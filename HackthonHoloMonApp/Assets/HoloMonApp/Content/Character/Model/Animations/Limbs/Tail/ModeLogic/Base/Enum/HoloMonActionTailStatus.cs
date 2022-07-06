using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Tail.ModeLogic
{
    /// <summary>
    /// ホロモン尻尾状態
    /// </summary>
    public enum HoloMonActionTailStatus
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
