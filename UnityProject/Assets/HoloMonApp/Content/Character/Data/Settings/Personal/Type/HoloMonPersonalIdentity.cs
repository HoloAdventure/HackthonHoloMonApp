using System;

namespace HoloMonApp.Content.Character.Data.Settings.Personal
{
    /// <summary>
    /// アイデンティティ設定の定義
    /// </summary>
    [Serializable]
    public class HoloMonPersonalIdentity
    {
        /// <summary>
        /// ホロモン固有名
        /// </summary>
        public string HoloMonName;

        public HoloMonPersonalIdentity(string a_HoloMonName)
        {
            HoloMonName = a_HoloMonName;
        }

        // XmlSerializeのため、引数無しのコンストラクタを定義する
        private HoloMonPersonalIdentity() { }
    }
}