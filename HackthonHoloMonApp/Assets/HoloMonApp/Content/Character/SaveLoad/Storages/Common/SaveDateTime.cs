using System;
using System.Xml;
using System.Xml.Serialization;

namespace HoloMonApp.Content.Character.SaveLoad.Storages
{
    [Serializable]
    public class SaveDateTime
    {
        // TODO : XmlSerialize利用のため、public とする
        public DateTime Value;
#if UNITY_EDITOR
        // TODO : DateTimeText => Value.ToString("yyyy-MM-dd HH:mm:ss") の記法では
        //        Inspector ビューに表示されないため、コンストラクタで値を代入する
        [XmlIgnore]
        public string DateTimeText;
#endif
        public SaveDateTime(DateTime a_Value)
        {
            Value = a_Value;
#if UNITY_EDITOR
            DateTimeText = Value.ToString("yyyy-MM-dd HH:mm:ss");
#endif
        }

        // XmlSerialize利用のため、引数無しのコンストラクタを定義する
        private SaveDateTime() { }
    }
}