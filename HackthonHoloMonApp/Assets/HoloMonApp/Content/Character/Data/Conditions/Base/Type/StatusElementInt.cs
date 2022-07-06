using System;
using System.Xml;
using System.Xml.Serialization;

namespace HoloMonApp.Content.Character.Data.Conditions
{
    [Serializable]
    public class StatusElementInt
    {
        // TODO : XmlSerialize利用のため、public とする
        public DateTime LastUpdateDateTime;
#if UNITY_EDITOR
        [XmlIgnore]
        public string LastUpdateDateTimeText;
#endif

        // TODO : XmlSerialize利用のため、public とする
        public int Value;

        public StatusElementInt(int a_Value)
        {
            Value = a_Value;
            LastUpdateDateTime = DateTime.Now;
#if UNITY_EDITOR
            LastUpdateDateTimeText = LastUpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
#endif
        }

        public void UpdateValue(int a_Value)
        {
            Value = a_Value;
            LastUpdateDateTime = DateTime.Now;
#if UNITY_EDITOR
            LastUpdateDateTimeText = LastUpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
#endif
        }

        // XmlSerialize利用のため、引数無しのコンストラクタを定義する
        private StatusElementInt() { }
    }
}