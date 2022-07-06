using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectUnderstandInformation
    {
        [SerializeField, Tooltip("オブジェクト理解種別")]
        private ObjectUnderstandType p_ObjectUnderstandType = ObjectUnderstandType.Nothing;

        /// <summary>
        /// オブジェクト理解種別
        /// </summary>
        public ObjectUnderstandType ObjectUnderstandType => p_ObjectUnderstandType;

        [SerializeField]
        private ObjectUnderstandDataInterface p_ObjectUnderstandDataInterface;

        /// <summary>
        /// オブジェクト情報
        /// </summary>
        public ObjectUnderstandDataInterface ObjectUnderstandDataInterface => p_ObjectUnderstandDataInterface;

        // オブジェクト情報のキャスト参照
        public ObjectUnderstandUnknownData ObjectUnderstandUnknownData
            => (ObjectUnderstandUnknownData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandLearningData ObjectUnderstandLearningData
            => (ObjectUnderstandLearningData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandOtherData ObjectUnderstandOtherData
            => (ObjectUnderstandOtherData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandFriendFaceData ObjectUnderstandFriendFaceData
            => (ObjectUnderstandFriendFaceData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandFriendRightHandData ObjectUnderstandFriendRightHandData
            => (ObjectUnderstandFriendRightHandData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandFriendLeftHandData ObjectUnderstandFriendLeftHandData
            => (ObjectUnderstandFriendLeftHandData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandBallData ObjectUnderstandBallData
            => (ObjectUnderstandBallData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandFoodData ObjectUnderstandFoodData
            => (ObjectUnderstandFoodData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandPoopData ObjectUnderstandPoopData
            => (ObjectUnderstandPoopData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandJewelData ObjectUnderstandJewelData
            => (ObjectUnderstandJewelData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandShowerHeadData ObjectUnderstandShowerHeadData
            => (ObjectUnderstandShowerHeadData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandShowerWaterData ObjectUnderstandShowerWaterData
            => (ObjectUnderstandShowerWaterData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandHandPointerData ObjectUnderstandHandPointerData
            => (ObjectUnderstandHandPointerData)p_ObjectUnderstandDataInterface;

        public ObjectUnderstandCardboardBoxData ObjectUnderstandCardboardData
            => (ObjectUnderstandCardboardBoxData)p_ObjectUnderstandDataInterface;


        public ObjectUnderstandInformation()
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Nothing;
            p_ObjectUnderstandDataInterface = null;
        }

        public ObjectUnderstandInformation(ObjectUnderstandUnknownData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Unknown;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandLearningData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Learning;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandOtherData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Other;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandFriendFaceData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.FriendFace;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandFriendRightHandData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.FriendRightHand;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandFriendLeftHandData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.FriendLeftHand;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandBallData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Ball;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandFoodData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Food;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandPoopData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Poop;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandJewelData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.Jewel;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandShowerHeadData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.ShowerHead;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandShowerWaterData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.ShowerWater;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandHandPointerData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.HandPointer;
            p_ObjectUnderstandDataInterface = a_Data;
        }

        public ObjectUnderstandInformation(ObjectUnderstandCardboardBoxData a_Data)
        {
            p_ObjectUnderstandType = ObjectUnderstandType.CardboardBox;
            p_ObjectUnderstandDataInterface = a_Data;
        }
    }
}