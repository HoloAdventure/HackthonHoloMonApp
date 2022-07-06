using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Tail.ModeLogic
{
    /// <summary>
    /// ホロモンの尻尾アクション種別
    /// </summary>
    public enum HoloMonActionTail
    {
        /// <summary>
        /// デフォルト(オプション無し)
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 尻尾上書き無し
        /// </summary>
        NoOverride,
        /// <summary>
        /// 幸せ
        /// </summary>
        Happy,
        /// <summary>
        /// 悲しみ
        /// </summary>
        Sad,
        /// <summary>
        /// 警戒
        /// </summary>
        Vigilant,
    }
}
