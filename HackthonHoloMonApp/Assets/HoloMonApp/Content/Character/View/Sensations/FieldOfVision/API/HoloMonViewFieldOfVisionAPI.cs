using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Model.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.View.Sensations.FieldOfVision
{
    /// <summary>
    /// 視界システムAPI
    /// </summary>
    public class HoloMonViewFieldOfVisionAPI : MonoBehaviour
    {
        /// <summary>
        /// APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonFieldOfVisionAPI p_FieldOfVisionAPI;

        /// <summary>
        /// 視界内でホロモンの見つけたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<VisionObjectWrap> ReactivePropertyFindObjectWrap
            => p_FieldOfVisionAPI.IReadOnlyReactivePropertyFindObjectWrap;

        /// <summary>
        /// 視界内でホロモンが見失ったもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<int> ReactivePropertyLostObjectInstanceID
            => p_FieldOfVisionAPI.IReadOnlyReactivePropertyLostObjectInstanceID;

        /// <summary>
        /// 視界内でホロモンの状態変化に気づいたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<VisionObjectWrap> ReactivePropertyUpdateStatusObjectWrap
            => p_FieldOfVisionAPI.IReadOnlyReactivePropertyUpdateStatusObjectWrap;

        /// <summary>
        /// 視界内でホロモンの距離変化に気づいたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<VisionObjectWrap> ReactivePropertyUpdateRangeObjectWrap
            => p_FieldOfVisionAPI.IReadOnlyReactivePropertyUpdateRangeObjectWrap;


        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public VisionObjectWrap CheckCollectionByGameObject(GameObject a_Object)
        {
            return p_FieldOfVisionAPI.CheckCollectionByGameObject(a_Object);
        }

        /// <summary>
        /// 視界オブジェクトコレクションから指定条件が一致する
        /// かつ、距離が最も近いオブジェクトを探す
        /// </summary>
        public VisionObjectWrap CheckCollectionByNearDistance(
            string a_ObjectName = null,
            float a_NearDistance = -1.0f,
            float a_InsideAngle = -1.0f,
            ObjectUnderstandType a_ObjectUnderstandType = ObjectUnderstandType.Nothing
            )
        {
            bool checkObjectName = !(a_ObjectName == null);
            bool checkNearDistance = !(a_NearDistance == -1.0f);
            bool checkInsideAngle = !(a_InsideAngle == -1.0f);
            bool checkObjectUnderstandType = !(a_ObjectUnderstandType == ObjectUnderstandType.Nothing);

            return p_FieldOfVisionAPI.CheckCollectionByNearDistance(
                checkObjectName, a_ObjectName,
                checkNearDistance, a_NearDistance,
                checkInsideAngle, a_InsideAngle,
                checkObjectUnderstandType, a_ObjectUnderstandType
                );
        }

        /// <summary>
        /// 視界オブジェクトコレクションから指定条件が一致する
        /// かつ、優先度が最も近いオブジェクトを探す
        /// 優先度が同じ場合は距離の近いものを選択する
        /// </summary>
        public VisionObjectWrap CheckCollectionByTypePriority(
            string a_ObjectName = null,
            float a_NearDistance = -1.0f,
            float a_InsideAngle = -1.0f
            )
        {
            bool checkObjectName = !(a_ObjectName == null);
            bool checkNearDistance = !(a_NearDistance == -1.0f);
            bool checkInsideAngle = !(a_InsideAngle == -1.0f);

            return p_FieldOfVisionAPI.CheckCollectionByTypePriority(
                checkObjectName, a_ObjectName,
                checkNearDistance, a_NearDistance,
                checkInsideAngle, a_InsideAngle
                );
        }

        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// 存在する場合はそのオブジェクトまでの距離を返す
        /// 視界内にゲームオブジェクトがなかった場合は -1.0f を返す
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public float CurrentDistanceForGameObject(GameObject a_Object, bool a_IgnoreYAxis = false)
        {
            return p_FieldOfVisionAPI.CurrentDistanceForGameObject(a_Object, a_IgnoreYAxis);
        }

        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// 存在する場合はそのオブジェクトとの相対角度を返す
        /// 視界内にゲームオブジェクトがなかった場合は -1.0f を返す
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public float CurrentAngleForGameObject(GameObject a_Object, bool a_IgnoreYAxis = false)
        {
            return p_FieldOfVisionAPI.CurrentAngleForGameObject(a_Object, a_IgnoreYAxis);
        }

        /// <summary>
        /// 対象オブジェクトがいずれかの指定の距離内にあるかを判定する
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public bool CurrentVisionRangeAreaForGameObject(GameObject a_Object, bool a_InNear, bool a_InMiddle, bool a_InFar)
        {
            bool resultInArea = false;
            VisionRangeType visionRangeType = p_FieldOfVisionAPI.CurrentVisionRangeAreaForGameObject(a_Object);

            switch (visionRangeType)
            {
                case VisionRangeType.Nothing:
                    break;
                case VisionRangeType.NearArea:
                    if (a_InNear) resultInArea = true;
                    break;
                case VisionRangeType.MiddleArea:
                    if (a_InMiddle) resultInArea = true;
                    break;
                case VisionRangeType.FarArea:
                    if (a_InFar) resultInArea = true;
                    break;
                case VisionRangeType.OutRangeArea:
                    break;
            }
            return resultInArea;
        }
    }
}