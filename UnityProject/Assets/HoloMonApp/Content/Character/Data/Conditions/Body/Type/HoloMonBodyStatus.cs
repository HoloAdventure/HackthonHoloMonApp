using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

namespace HoloMonApp.Content.Character.Data.Conditions.Body
{
    /// <summary>
    /// ホロモンのボディコンディションの定義
    /// </summary>
    [Serializable]
    public class HoloMonBodyStatus
    {
        /// <summary>
        /// 初期化済みフラグ
        /// </summary>
        public bool Initialized = false;

        /// <summary>
        /// ホロモンの誕生日時
        /// </summary>
        public StatusElementDateTime BirthTime;

        /// <summary>
        /// ホロモンの日齢
        /// </summary>
        public StatusElementInt AgeOfDay;

        /// <summary>
        /// ホロモンの大きさ
        /// </summary>
        public StatusElementFloat BodyHeight;

        /// <summary>
        /// ホロモンの初期の大きさ
        /// </summary>
        public StatusElementFloat DefaultBodyHeight;

        /// <summary>
        /// ホロモンのちから
        /// </summary>
        public StatusElementFloat BodyPower;


        /// <summary>
        /// ホロモンのデフォルトサイズの変化比率(計算式)
        /// </summary>
        [XmlIgnore]
        public float GetDefaultBodyHeightRatio
            => (float)System.Math.Round(
                BodyHeight.Value / DefaultBodyHeight.Value,
                1,
                MidpointRounding.AwayFromZero
            );


        public HoloMonBodyStatus(
            DateTime a_BirthTime,
            int a_AgeOfDay,
            float a_BodyHeight,
            float a_DefaultBodyHeight,
            float a_BodyPower
            )
        {
            BirthTime = new StatusElementDateTime(a_BirthTime);
            AgeOfDay = new StatusElementInt(a_AgeOfDay);
            BodyHeight = new StatusElementFloat(a_BodyHeight);
            DefaultBodyHeight = new StatusElementFloat(a_DefaultBodyHeight);
            BodyPower = new StatusElementFloat(a_BodyPower);
        }

        // XmlSerializeのため、引数無しのコンストラクタを定義する
        private HoloMonBodyStatus() { }
    }
}
