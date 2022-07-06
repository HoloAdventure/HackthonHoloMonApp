namespace HoloMonApp.Content.Character.Behave.Purpose
{
    /// <summary>
    /// ホロモン目的行動種別
    /// </summary>
    public enum HoloMonPurposeType
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 無行動
        /// </summary>
        None,
        /// <summary>
        /// 待機
        /// </summary>
        Standby,
        /// <summary>
        /// 友人に注目する
        /// </summary>
        LookFriend,
        /// <summary>
        /// 待て(おすわり)
        /// </summary>
        StayWait,
        /// <summary>
        /// ターゲットを探して追いかける
        /// </summary>
        MoveTracking,
        /// <summary>
        /// じゃんけんで遊ぶ
        /// </summary>
        Janken,
        /// <summary>
        /// ステップダンスを行う
        /// </summary>
        Dance,
        /// <summary>
        /// 眠る
        /// </summary>
        Sleep,
        /// <summary>
        /// 食事する
        /// </summary>
        MealFood,
        /// <summary>
        /// ボール遊び
        /// </summary>
        CatchBall,
        /// <summary>
        /// うんちする
        /// </summary>
        PutoutPoop,
        /// <summary>
        /// 掴まれる
        /// </summary>
        HungUp,
        /// <summary>
        /// 持ってくる
        /// </summary>
        BringItem,
        /// <summary>
        /// 逃げる
        /// </summary>
        RunFrom,
    }
}
