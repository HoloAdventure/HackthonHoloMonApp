using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Interrupt
{
    /// <summary>
    /// ホロモンコマンド
    /// </summary>
    public enum HoloMonCommand
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// データの書き込み
        /// </summary>
        SaveData,
        /// <summary>
        /// データの読み込み
        /// </summary>
        LoadData,
        /// <summary>
        /// AIの有効無効を切り替える
        /// </summary>
        ChangeEnableAI,
        /// <summary>
        /// 位置をリセットする
        /// </summary>
        ResetPosition,
        /// <summary>
        /// スケールを変更する
        /// </summary>
        ChangeScale,
        /// <summary>
        /// ステータスを変更する
        /// </summary>
        ChnageStatus,
        /// <summary>
        /// スタンバイ状態になる
        /// </summary>
        Standby,
        /// <summary>
        /// プレイヤーを追跡する
        /// </summary>
        ComeHere,
        /// <summary>
        /// プレイヤーを見る
        /// </summary>
        LookMe,
        /// <summary>
        /// ターゲットを見る
        /// </summary>
        LookTarget,
        /// <summary>
        /// じゃんけん開始
        /// </summary>
        StartJanken,
        /// <summary>
        /// ホロモンを眠らせる
        /// </summary>
        StartSleep,
        /// <summary>
        /// ホロモンにダンスさせる
        /// </summary>
        StartDance,
        /// <summary>
        /// ホロモンに待てさせる
        /// </summary>
        StartWait,
        /// <summary>
        /// ホロモンに食事を与える
        /// </summary>
        GiveFood,
        /// <summary>
        /// ホロモンにボールを与える
        /// </summary>
        GiveBall,
    }
}
