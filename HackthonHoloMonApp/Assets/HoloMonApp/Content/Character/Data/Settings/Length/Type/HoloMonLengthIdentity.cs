using System;

namespace HoloMonApp.Content.Character.Data.Settings.Length
{
    /// <summary>
    /// アイデンティティ設定の定義
    /// </summary>
    [Serializable]
    public class HoloMonLengthIdentity
    {
        /// <summary>
        /// ホロモンの縦幅(基本サイズ1.0f時)
        /// </summary>
        public float HoloMonHeightLength;

        /// <summary>
        /// ホロモンの横幅(基本サイズ1.0f時)
        /// </summary>
        public float HoloMonWideLength;

        /// <summary>
        /// ホロモンの奥幅(基本サイズ1.0f時)
        /// </summary>
        public float HoloMonDepthLength;

        /// <summary>
        /// ホロモンの手が届く距離(基本サイズ1.0f時)
        /// </summary>
        public float HoloMonHandReachableLength;

        public HoloMonLengthIdentity(
            float a_HoloMonHeightLength,
            float a_HoloMonWideLength,
            float a_HoloMonDepthLength,
            float a_HoloMonHandReachableLength)
        {
            HoloMonHeightLength = a_HoloMonHeightLength;
            HoloMonWideLength = a_HoloMonWideLength;
            HoloMonDepthLength = a_HoloMonDepthLength;
            HoloMonHandReachableLength = a_HoloMonHandReachableLength;
        }

        // XmlSerializeのため、引数無しのコンストラクタを定義する
        private HoloMonLengthIdentity() { }
    }
}