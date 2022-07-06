using System;
using System.Xml;
using System.Xml.Serialization;

using HoloMonApp.Content.Character.Data.Conditions.Body;
using HoloMonApp.Content.Character.Data.Conditions.Life;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    /// <summary>
    /// データファイル構造クラス
    /// </summary>
    [Serializable]
    public class HoloMonSaveData
    {
        /// <summary>
        /// 保存時刻
        /// </summary>
        public SaveDateTime SavedDataTime;

        /// <summary>
        /// 保存バージョン
        /// </summary>
        public SavePackageVersion SavedPackageVersion;

        /// <summary>
        /// ホロモンアプリの名前
        /// </summary>
        public string AppName;

        /// <summary>
        /// 身体状態
        /// </summary>
        public HoloMonBodyStatus BodyStatus;

        /// <summary>
        /// 生活状態
        /// </summary>
        public HoloMonLifeStatus LifeStatus;
    }

}
