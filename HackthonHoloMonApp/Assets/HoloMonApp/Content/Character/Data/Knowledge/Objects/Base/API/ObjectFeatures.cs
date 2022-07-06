using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Data.Knowledge.Objects
{
    public class ObjectFeatures : MonoBehaviour
    {
        [SerializeField, Tooltip("オブジェクト理解種別(初期定義)")]
        private ObjectUnderstandType DefaultObjectUnderstandType = ObjectUnderstandType.Nothing;

        /// <summary>
        /// オブジェクトデータ参照
        /// </summary>
        public ObjectFeatureWrap DataWrap => p_ObjectFeatureWrapInstance ?? (p_ObjectFeatureWrapInstance = CreateObjectFeatureWrap());

        /// <summary>
        /// オブジェクトデータ参照生成インスタンス
        /// </summary>
        private ObjectFeatureWrap p_ObjectFeatureWrapInstance;

        [SerializeField, Tooltip("オブジェクトID値(Editor確認用)")]
        private int p_ObjectID;

        [SerializeField, Tooltip("オブジェクト名(Editor確認用)")]
        private string p_ObjectName;

        [SerializeField, Tooltip("オブジェクト種別値(Editor確認用)")]
        private ObjectUnderstandType p_ObjectUnderstandType;

        [SerializeField, Tooltip("状態ハッシュ値(Editor確認用)")]
        private int p_StatusHash;


        void OnEnable()
        {
            // オブジェクトデータの初期化を行う
            p_ObjectFeatureWrapInstance = CreateObjectFeatureWrap();
        }

        private void Update()
        {
            // Inspectorビューを更新する
            UpdateInspector(p_ObjectFeatureWrapInstance);
        }

        #region "private"
        /// <summary>
        /// オブジェクトデータの初期化を行う
        /// </summary>
        private ObjectFeatureWrap CreateObjectFeatureWrap()
        {
            // オブジェクトの参照を設定する
            GameObject gameObjectRef = this.gameObject;

            // 初期状態に基づきオブジェクト種別情報を生成する
            ObjectUnderstandInformation understandInfomation = new ObjectUnderstandInformation();
            switch (DefaultObjectUnderstandType)
            {
                case ObjectUnderstandType.Nothing:
                    break;
                case ObjectUnderstandType.Unknown:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandUnknownData());
                    break;
                case ObjectUnderstandType.Learning:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandLearningData());
                    break;
                case ObjectUnderstandType.Other:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandOtherData());
                    break;
                case ObjectUnderstandType.FriendFace:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandFriendFaceData(ObjectStatusHead.Nothing));
                    break;
                case ObjectUnderstandType.FriendRightHand:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandFriendRightHandData(ObjectStatusHand.Nothing));
                    break;
                case ObjectUnderstandType.FriendLeftHand:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandFriendLeftHandData(ObjectStatusHand.Nothing));
                    break;
                case ObjectUnderstandType.Ball:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandBallData(ObjectStatusItem.Nothing));
                    break;
                case ObjectUnderstandType.Food:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandFoodData(ObjectStatusItem.Nothing));
                    break;
                case ObjectUnderstandType.Poop:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandPoopData(ObjectStatusItem.Nothing));
                    break;
                case ObjectUnderstandType.Jewel:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandJewelData(ObjectStatusItem.Nothing));
                    break;
                case ObjectUnderstandType.ShowerHead:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandShowerHeadData(ObjectStatusItem.Nothing));
                    break;
                case ObjectUnderstandType.ShowerWater:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandShowerWaterData());
                    break;
                case ObjectUnderstandType.HandPointer:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandHandPointerData());
                    break;
                case ObjectUnderstandType.CardboardBox:
                    understandInfomation = new ObjectUnderstandInformation(new ObjectUnderstandCardboardBoxData(ObjectStatusItem.Nothing));
                    break;
                default:
                    break;
            }

            // オブジェクトデータを生成する
            ObjectFeatureWrap objectFeatureWrap = new ObjectFeatureWrap(gameObjectRef, understandInfomation);

            // Inspectorビューを更新する
            UpdateInspector(objectFeatureWrap);

            return objectFeatureWrap;
        }

        /// <summary>
        /// Inspectorビューを更新する
        /// </summary>
        private void UpdateInspector(ObjectFeatureWrap a_ObjectFeatureWrap)
        {
#if UNITY_EDITOR
            if (a_ObjectFeatureWrap.UnderstandInformation != null)
            {
                p_ObjectID = a_ObjectFeatureWrap.ObjectID;
                p_ObjectName = a_ObjectFeatureWrap.ObjectName;
                p_ObjectUnderstandType = a_ObjectFeatureWrap.UnderstandInformation.ObjectUnderstandType;
                p_StatusHash = a_ObjectFeatureWrap.UnderstandInformation.ObjectUnderstandDataInterface.StatusHash();
            }
#endif
        }
        #endregion
    }
}
