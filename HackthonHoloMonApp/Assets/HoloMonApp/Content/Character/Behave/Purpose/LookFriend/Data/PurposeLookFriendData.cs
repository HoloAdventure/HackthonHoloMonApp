using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose.LookFriend
{
    [Serializable]
    public class PurposeLookFriendData : PurposeDataInterface
    {
        public PurposeLookFriendData()
        {
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.LookFriend;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}