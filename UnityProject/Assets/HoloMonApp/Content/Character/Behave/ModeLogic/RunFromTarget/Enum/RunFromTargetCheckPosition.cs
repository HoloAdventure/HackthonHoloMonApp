using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.RunFromTarget
{
    /// <summary>
    /// ターゲットの判定位置
    /// </summary>
    public enum RunFromTargetCheckPosition
    {
        /// <summary>
        /// デフォルト(オプション無し)
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// オブジェクトのオリジナル座標
        /// </summary>
        ObjectOrigin,
        /// <summary>
        /// オブジェクトの足元座標
        /// </summary>
        FootPosition,
    }
}
