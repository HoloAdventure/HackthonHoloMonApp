using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectUnderstandFriendFaceData : ObjectUnderstandDataInterface
    {
        [SerializeField, Tooltip("頭部ステータス情報")]
        private ObjectStatusHead p_HeadStatus;

        /// <summary>
        /// 頭部ステータス情報
        /// </summary>
        public ObjectStatusHead HeadStatus => p_HeadStatus;

        public ObjectUnderstandFriendFaceData(ObjectStatusHead a_HeadStatus)
        {
            p_HeadStatus = a_HeadStatus;
        }

        public int StatusHash()
        {
            return (int)p_HeadStatus;
        }
    }
}