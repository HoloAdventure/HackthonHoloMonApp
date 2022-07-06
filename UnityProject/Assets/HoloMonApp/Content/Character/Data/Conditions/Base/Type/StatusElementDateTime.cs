using System;
using System.Xml;
using System.Xml.Serialization;

namespace HoloMonApp.Content.Character.Data.Conditions
{
    [Serializable]
    public class StatusElementDateTime
    {
        // TODO : XmlSerialize利用のため、public とする
        public DateTime LastUpdateDateTime;
#if UNITY_EDITOR
        [XmlIgnore]
        public string LastUpdateDateTimeText;
#endif

        // TODO : XmlSerialize利用のため、public とする
        public DateTime Value;
#if UNITY_EDITOR
        [XmlIgnore]
        public string ValueText;
#endif

        public StatusElementDateTime(DateTime a_Value)
        {
            Value = a_Value;
            LastUpdateDateTime = DateTime.Now;
#if UNITY_EDITOR
            ValueText = Value.ToString("yyyy-MM-dd HH:mm:ss");
            LastUpdateDateTimeText = LastUpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
#endif
        }

        public void UpdateValue(DateTime a_Value)
        {
            Value = a_Value;
            LastUpdateDateTime = DateTime.Now;
#if UNITY_EDITOR
            ValueText = Value.ToString("yyyy-MM-dd HH:mm:ss");
            LastUpdateDateTimeText = LastUpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
#endif
        }

        // XmlSerialize利用のため、引数無しのコンストラクタを定義する
        private StatusElementDateTime() { }
    }
}