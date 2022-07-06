using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Model.Sensations.TactileBody;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.View.Sensations.TactileBody
{
    /// <summary>
    /// 触覚システムAPI
    /// </summary>
    public class HoloMonViewTactileBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonTactileBodyAPI p_TactileBodyAPI;

        /// <summary>
        /// 触覚内でホロモンの見つけたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<TactileObjectWrap> ReactivePropertyFindObjectWrap
            => p_TactileBodyAPI.IReadOnlyReactivePropertyFindObjectWrap;

        /// <summary>
        /// 触覚内でホロモンが見失ったもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<int> ReactivePropertyLostObjectInstanceID
            => p_TactileBodyAPI.IReadOnlyReactivePropertyLostObjectInstanceID;

        /// <summary>
        /// 触覚内でホロモンの状態変化に気づいたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<TactileObjectWrap> ReactivePropertyUpdateStatusObjectWrap
            => p_TactileBodyAPI.IReadOnlyReactivePropertyUpdateStatusObjectWrap;


        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public TactileObjectWrap CheckCollectionByGameObject(GameObject a_Object)
        {
            return p_TactileBodyAPI.CheckCollectionByGameObject(a_Object);
        }

        /// <summary>
        /// 視界オブジェクトコレクションから指定条件が一致する
        /// かつ、距離が最も近いオブジェクトを探す
        /// </summary>
        public TactileObjectWrap CheckCollectionByNearDistance(
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

            return p_TactileBodyAPI.CheckCollectionByNearDistance(
                checkObjectName, a_ObjectName,
                checkNearDistance, a_NearDistance,
                checkInsideAngle, a_InsideAngle,
                checkObjectUnderstandType, a_ObjectUnderstandType
                );
        }
    }
}