using System;
using System.Xml;
using System.Xml.Serialization;

namespace HoloMonApp.Content.Character.Data.Conditions
{
    [Serializable]
    public class StatusElementPackageVersion
    {
        // TODO : XmlSerialize利用のため、public とする
        public DateTime LastUpdateDateTime;
#if UNITY_EDITOR
        [XmlIgnore]
        public string LastUpdateDateTimeText;
#endif

        [Serializable]
        public class PackageVersionFormat
        {
            public ushort Major;
            public ushort Minor;
            public ushort Build;
            public ushort Revision;
        }

        // TODO : XmlSerialize利用のため、public とする
        public PackageVersionFormat Value;

        public StatusElementPackageVersion(ushort a_Major, ushort a_Minor, ushort a_Build, ushort a_Revision)
        {
            Value.Major = a_Major;
            Value.Minor = a_Minor;
            Value.Build = a_Build;
            Value.Revision = a_Revision;
            LastUpdateDateTime = DateTime.Now;
#if UNITY_EDITOR
            LastUpdateDateTimeText = LastUpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
#endif
        }

        public void UpdateValue(ushort a_Major, ushort a_Minor, ushort a_Build, ushort a_Revision)
        {
            Value.Major = a_Major;
            Value.Minor = a_Minor;
            Value.Build = a_Build;
            Value.Revision = a_Revision;
            LastUpdateDateTime = DateTime.Now;
#if UNITY_EDITOR
            LastUpdateDateTimeText = LastUpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
#endif
        }

        // XmlSerialize利用のため、引数無しのコンストラクタを定義する
        private StatusElementPackageVersion() { }
    }
}