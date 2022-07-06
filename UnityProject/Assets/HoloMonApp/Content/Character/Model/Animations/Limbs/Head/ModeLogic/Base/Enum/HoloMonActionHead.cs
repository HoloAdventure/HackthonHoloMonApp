using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    /// <summary>
    /// ホロモン頭部アクション種別
    /// </summary>
    public enum HoloMonActionHead
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// オーバーライド無し
        /// </summary>
        NoOverride,
        /// <summary>
        /// 対象を見る頭部アクション
        /// </summary>
        LookAtTarget,
        /// <summary>
        /// 対象を避ける頭部アクション
        /// </summary>
        DontLookAtTarget,
    }

}
