using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectUnderstandFriendLeftHandData : ObjectUnderstandDataInterface
    {
        [SerializeField, Tooltip("ハンドステータス情報")]
        private ObjectStatusHand p_HandStatus;

        /// <summary>
        /// ハンドステータス情報
        /// </summary>
        public ObjectStatusHand HandStatus => p_HandStatus;

        public ObjectUnderstandFriendLeftHandData(ObjectStatusHand a_HandStatus)
        {
            p_HandStatus = a_HandStatus;
        }

        public int StatusHash()
        {
            return (int)p_HandStatus;
        }
    }
}