using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Data.Animations.Body
{
    /// <summary>
    /// アクションモード定義
    /// </summary>
    public enum AnimationMode
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// スタンバイ
        /// </summary>
        Standby = 1,
        /// <summary>
        /// 追跡
        /// </summary>
        Tracking = 2,
        /// <summary>
        /// 見回し
        /// </summary>
        LookAround = 3,
        /// <summary>
        /// 振り返り
        /// </summary>
        Turn = 4,
        /// <summary>
        /// お座り
        /// </summary>
        SitDown = 5,
        /// <summary>
        /// 睡眠
        /// </summary>
        Sleep = 11,
        /// <summary>
        /// うんち
        /// </summary>
        PoopPutout = 12,
        /// <summary>
        /// じゃんけん
        /// </summary>
        Janken = 21,
        /// <summary>
        /// 食事
        /// </summary>
        FoodMeal = 22,
        /// <summary>
        /// ダンス
        /// </summary>
        Dance = 23,
        /// <summary>
        /// 掴まれ
        /// </summary>
        HungUp = 24,
    }
}
