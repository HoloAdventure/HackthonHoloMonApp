using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Animations.Body
{
    /// <summary>
    /// ホロモンアニメーション情報情報
    /// </summary>
    [Serializable]
    public class HoloMonAnimationsBodyRawData
    {
        /// <summary>
        /// アニメーションモード
        /// </summary>
        public AnimationMode AnimationMode;

        /// <summary>
        /// モデル速度
        /// </summary>
        public float Speed;

        /// <summary>
        /// モデル回転速度
        /// </summary>
        public float Rotate;

        /// <summary>
        /// アイテム保持オプション
        /// </summary>
        public bool HoldItemOption;

        /// <summary>
        /// じゃんけんポーズ
        /// </summary>
        public JankenPose JankenPose;

        /// <summary>
        /// 睡眠ポーズ
        /// </summary>
        public SleepPose SleepPose;

        /// <summary>
        /// じゃんけん結果
        /// </summary>
        public JankenResult JankenResult;

        /// <summary>
        /// ダンス種類
        /// </summary>
        public DanceType DanceTyep;

        /// <summary>
        /// リアクションポーズ
        /// </summary>
        public ReactionPose ReactionPose;
    }
}
