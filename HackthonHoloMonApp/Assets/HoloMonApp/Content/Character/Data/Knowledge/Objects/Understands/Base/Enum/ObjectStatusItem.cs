namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    /// <summary>
    /// オブジェクト状態の理解種別
    /// </summary>
    public enum ObjectStatusItem
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 床上
        /// </summary>
        Item_Floor = 1,
        /// <summary>
        /// 空中
        /// </summary>
        Item_Air = 2,
        /// <summary>
        /// ホールド状態(ホロモン)
        /// </summary>
        Item_HoloMonHold = 3,
        /// <summary>
        /// ホールド状態(プレイヤー)
        /// </summary>
        Item_PlayerHold = 4,
    }
}
