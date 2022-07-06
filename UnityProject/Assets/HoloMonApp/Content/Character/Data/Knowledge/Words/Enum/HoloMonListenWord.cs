using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Data.Knowledge.Words
{
    /// <summary>
    /// ホロモンが認識可能な言葉
    /// </summary>
    public enum HoloMonListenWord
    {
        /// <summary>
        /// デフォルト値
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// ホロモンの名前呼び
        /// </summary>
        HoloMonName,
        /// <summary>
        /// プレイヤー名
        /// </summary>
        PlayerName,
        /// <summary>
        /// 肯定(はい, 良し, etc...)
        /// </summary>
        Yes,
        /// <summary>
        /// 否定(いいえ, ダメ, etc...)
        /// </summary>
        No,
        /// <summary>
        /// キャンセル(なんでもない, etc...)
        /// </summary>
        Cancel,
        /// <summary>
        /// こっちおいで
        /// </summary>
        ComeHere,
        /// <summary>
        /// こっちみて
        /// </summary>
        LookMe,
        /// <summary>
        /// こっちむいて
        /// </summary>
        TurnMe,
        /// <summary>
        ///  待て
        /// </summary>
        Wait,
        /// <summary>
        /// どっち
        /// </summary>
        Which,
        /// <summary>
        /// じゃんけん
        /// </summary>
        Janken,
        /// <summary>
        /// ぽん or あいこでしょ
        /// </summary>
        PonSyo,
        /// <summary>
        /// 踊って
        /// </summary>
        StartDance,
        /// <summary>
        /// うんち
        /// </summary>
        ShitPutout,
        /// <summary>
        /// ピース
        /// </summary>
        Peace,
        /// <summary>
        /// ちょうし
        /// </summary>
        Status,
        /// <summary>
        /// おにく
        /// </summary>
        Food,
        /// <summary>
        /// ボール
        /// </summary>
        Ball,
        /// <summary>
        /// シャワー
        /// </summary>
        Shower,
        /// <summary>
        /// ベッド
        /// </summary>
        Bed,
    }

}
