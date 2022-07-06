namespace HoloMonApp.Content.Character.Behave.Purpose
{
    /// <summary>
    /// ホロモンの目的状態
    /// </summary>
    public enum HoloMonPurposeStatus
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
