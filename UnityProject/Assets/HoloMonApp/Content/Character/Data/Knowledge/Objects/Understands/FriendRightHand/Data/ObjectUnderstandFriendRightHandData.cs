using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectUnderstandFriendRightHandData : ObjectUnderstandDataInterface
    {
        [SerializeField, Tooltip("ハンドステータス情報")]
        private ObjectStatusHand p_HandStatus;

        /// <summary>
        /// ハンドステータス情報
        /// </summary>
        public ObjectStatusHand HandStatus => p_HandStatus;

        public ObjectUnderstandFriendRightHandData(ObjectStatusHand a_HandStatus)
        {
            p_HandStatus = a_HandStatus;
        }

        public int StatusHash()
        {
            return (int)p_HandStatus;
        }
    }
}