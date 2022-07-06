namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    /// <summary>
    /// ホロモンアクションモード状態
    /// </summary>
    public enum HoloMonActionModeStatus
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 実行中
        /// </summary>
        Runtime,
        /// <summary>
        /// 停止中
        /// </summary>
        Stopping,
        /// <summary>
        /// 目的達成
        /// </summary>
        Achievement,
        /// <summary>
        /// 目的未達
        /// </summary>
        Missing,
        /// <summary>
        /// キャンセル
        /// </summary>
        Cancel,
    }
}
