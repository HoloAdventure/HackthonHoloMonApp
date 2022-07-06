using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Data.Animations.Body
{
    /// <summary>
    /// リアクションポーズ定義
    /// </summary>
    public enum ReactionPose
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 肯定
        /// </summary>
        HeadNod = 1,
        /// <summary>
        /// 否定
        /// </summary>
        HeadShake = 2,
        /// <summary>
        /// 首傾げ(左)
        /// </summary>
        HeadTiltLeft = 3,
        /// <summary>
        /// 首傾げ(右)
        /// </summary>
        HeadTiltRight = 4,
        /// <summary>
        /// 頭撫でられ
        /// </summary>
        StrokingHead = 11,
        /// <summary>
        /// 首撫でられ
        /// </summary>
        ScratchNeck = 12,
        /// <summary>
        /// ピースサイン
        /// </summary>
        PeaceSign = 21,
        /// <summary>
        /// サムズアップサイン
        /// </summary>
        ThumbupSign = 24,
        /// <summary>
        /// 両手サムズアップサイン
        /// </summary>
        DoubleThumbupSign = 25,
        /// <summary>
        /// 空腹ポーズ
        /// </summary>
        HungryStatus = 31,
        /// <summary>
        /// 退屈ポーズ
        /// </summary>
        BoardStatus = 32,
        /// <summary>
        /// 疲労ポーズ
        /// </summary>
        TiredStatus = 33,
        /// <summary>
        /// 通常ポーズ
        /// </summary>
        GoodStatus = 34,
        /// <summary>
        /// 最高ポーズ
        /// </summary>
        BestStatus = 35,
        /// <summary>
        /// 寝そべりポーズ
        /// </summary>
        LyingPose = 41,
        /// <summary>
        /// 腹見せポーズ
        /// </summary>
        ShowBellyPose = 42,
        /// <summary>
        /// Yes意思表示
        /// </summary>
        YesIntention = 51,
        /// <summary>
        /// No意思表示
        /// </summary>
        NoIntention = 51,
    }
}
