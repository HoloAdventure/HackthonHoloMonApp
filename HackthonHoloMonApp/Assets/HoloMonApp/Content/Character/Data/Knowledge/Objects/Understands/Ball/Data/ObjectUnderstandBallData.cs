using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectUnderstandBallData : ObjectUnderstandDataInterface
    {
        [SerializeField, Tooltip("アイテムステータス情報")]
        private ObjectStatusItem p_ItemStatus;

        /// <summary>
        /// アイテムステータス情報
        /// </summary>
        public ObjectStatusItem ItemStatus => p_ItemStatus;

        public ObjectUnderstandBallData(ObjectStatusItem a_ItemStatus)
        {
            p_ItemStatus = a_ItemStatus;
        }

        public int StatusHash()
        {
            return (int)p_ItemStatus;
        }
    }
}