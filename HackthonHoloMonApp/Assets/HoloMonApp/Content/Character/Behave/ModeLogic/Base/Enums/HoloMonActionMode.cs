namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    /// <summary>
    /// ホロモンアクションモード種別
    /// </summary>
    public enum HoloMonActionMode
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
        /// 待機モード
        /// </summary>
        Standby,
        /// <summary>
        /// 見回しモード
        /// </summary>
        LookAround,
        /// <summary>
        /// 追跡モード
        /// </summary>
        TrackingTarget,
        /// <summary>
        /// 追跡モード(NavMesh使用)
        /// </summary>
        TrackingTargetNavMesh,
        /// <summary>
        /// 振り返りモード
        /// </summary>
        TurnTarget,
        /// <summary>
        /// おすわりモード
        /// </summary>
        SitDown,
        /// <summary>
        /// じゃんけんモード
        /// </summary>
        Janken,
        /// <summary>
        /// 食事モード
        /// </summary>
        MealFood,
        /// <summary>
        /// ダンスモード
        /// </summary>
        Dance,
        /// <summary>
        /// 睡眠モード
        /// </summary>
        Sleep,
        /// <summary>
        /// うんちモード
        /// </summary>
        PutoutPoop,
        /// <summary>
        /// 掴まれモード
        /// </summary>
        HungUp,
        /// <summary>
        /// 逃走モード
        /// </summary>
        RunFromTarget,
    }
}
