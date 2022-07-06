namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    /// <summary>
    /// オブジェクト状態の理解種別
    /// </summary>
    public enum ObjectStatusHead
    {
        /// <summary>
        /// デフォルト
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 頷く
        /// </summary>
        Head_Nod = 1,
        /// <summary>
        /// 首を振る
        /// </summary>
        Head_Shake = 2,
        /// <summary>
        /// 首を傾げる(左方向)
        /// </summary>
        Head_TiltLeft = 3,
        /// <summary>
        /// 首を傾げる(右方向)
        /// </summary>
        Head_TiltRight = 4,
        /// <summary>
        /// 寝転び
        /// </summary>
        Head_LieDown = 5,
    }
}
