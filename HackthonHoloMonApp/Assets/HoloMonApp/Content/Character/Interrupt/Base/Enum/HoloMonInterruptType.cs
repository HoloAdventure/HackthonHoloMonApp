using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Interrupt
{
    /// <summary>
    /// ホロモンインタラプト種別
    /// </summary>
    public enum HoloMonInterruptType
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 身体コンディション
        /// </summary>
        ConditionBody,
        /// <summary>
        /// 生活コンディション
        /// </summary>
        ConditionLife,
        /// <summary>
        /// 言葉の聞き取り
        /// </summary>
        ListenWord,
        /// <summary>
        /// 視覚検出
        /// </summary>
        FieldOfVision,
        /// <summary>
        /// 触覚検出
        /// </summary>
        TactileBody,
        /// <summary>
        /// 視線検知
        /// </summary>
        EyeGazeBody,
        /// <summary>
        /// 掴まれ検知
        /// </summary>
        HandGrabBody,
    }
}
