using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    [Serializable]
    public class ObjectFeatureWrap
    {
        /// <summary>
        /// オブジェクトデータ
        /// </summary>
        private ObjectFeatureData p_ObjectFeatureData;

        /// <summary>
        /// ゲームオブジェクトの参照
        /// </summary>
        public GameObject GameObject => p_ObjectFeatureData?.GameObject;

        /// <summary>
        /// オブジェクトIDの参照
        /// </summary>
        public int ObjectID => p_ObjectFeatureData?.GameObject?.GetInstanceID() ?? 0;

        /// <summary>
        /// オブジェクト名の参照
        /// </summary>
        public string ObjectName => p_ObjectFeatureData?.GameObject?.name ?? "";

        /// <summary>
        /// オブジェクトの理解種別情報
        /// </summary>
        public ObjectUnderstandInformation UnderstandInformation => p_ObjectFeatureData?.UnderstandInformation ?? new ObjectUnderstandInformation();

        public ObjectFeatureWrap(GameObject a_GameObject, ObjectUnderstandInformation a_UnderstandInformation)
        {
            p_ObjectFeatureData = new ObjectFeatureData();
            p_ObjectFeatureData.GameObject = a_GameObject;
            p_ObjectFeatureData.UnderstandInformation = a_UnderstandInformation;
        }

        /// <summary>
        /// 頭部状態の変更
        /// </summary>
        public bool ChangeStatusHead(ObjectStatusHead a_StatusHead)
        {
            bool isUpdated = false;

            // 対応する種別の場合のみ状態を変更する
            switch (UnderstandInformation.ObjectUnderstandType)
            {
                case ObjectUnderstandType.FriendFace:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandFriendFaceData(a_StatusHead)));
                    isUpdated = true;
                    break;
                default:
                    break;
            }

            return isUpdated;
        }

        /// <summary>
        /// 頭部状態の変更
        /// </summary>
        public bool ChangeStatusHand(ObjectStatusHand a_StatusHand)
        {
            bool isUpdated = false;

            // 対応する種別の場合のみ状態を変更する
            switch (UnderstandInformation.ObjectUnderstandType)
            {
                case ObjectUnderstandType.FriendRightHand:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandFriendRightHandData(a_StatusHand)));
                    isUpdated = true;
                    break;
                case ObjectUnderstandType.FriendLeftHand:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandFriendLeftHandData(a_StatusHand)));
                    isUpdated = true;
                    break;
                default:
                    break;
            }

            return isUpdated;
        }

        /// <summary>
        /// アイテム状態の変更
        /// </summary>
        public bool ChangeStatusItem(ObjectStatusItem a_StatusItem)
        {
            bool isUpdated = false;

            // 対応する種別の場合のみ状態を変更する
            switch (UnderstandInformation.ObjectUnderstandType)
            {
                case ObjectUnderstandType.Ball:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandBallData(a_StatusItem)));
                    isUpdated = true;
                    break;
                case ObjectUnderstandType.Food:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandFoodData(a_StatusItem)));
                    isUpdated = true;
                    break;
                case ObjectUnderstandType.Poop:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandPoopData(a_StatusItem)));
                    isUpdated = true;
                    break;
                case ObjectUnderstandType.Jewel:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandJewelData(a_StatusItem)));
                    isUpdated = true;
                    break;
                case ObjectUnderstandType.ShowerHead:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandShowerHeadData(a_StatusItem)));
                    isUpdated = true;
                    break;
                case ObjectUnderstandType.CardboardBox:
                    ApplyUnderstandInformation(new ObjectUnderstandInformation(new ObjectUnderstandCardboardBoxData(a_StatusItem)));
                    isUpdated = true;
                    break;
                default:
                    break;
            }

            return isUpdated;
        }

        #region "private"

        /// <summary>
        /// オブジェクトの理解種別情報の反映
        /// </summary>
        public void ApplyUnderstandInformation(ObjectUnderstandInformation a_UnderstandInformation)
        {
            p_ObjectFeatureData.UnderstandInformation = a_UnderstandInformation;
        }

        #endregion
    }
}