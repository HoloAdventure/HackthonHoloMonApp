using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    /// <summary>
    /// 視界システムAPI
    /// </summary>
    public class HoloMonFieldOfVisionAPI : MonoBehaviour
    {
        /// <summary>
        /// 視界内でホロモンが見つけたもの
        /// </summary>
        [SerializeField, Tooltip("視界内でホロモンが見つけたもの")]
        private VisionObjectWrapReactiveProperty p_FindObjectWrap
            = new VisionObjectWrapReactiveProperty();

        /// <summary>
        /// 視界内でホロモンの見つけたもののReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<VisionObjectWrap> p_IReadOnlyReactivePropertyFindObjectWrap;

        /// <summary>
        /// 視界内でホロモンの見つけたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<VisionObjectWrap> IReadOnlyReactivePropertyFindObjectWrap
            => p_IReadOnlyReactivePropertyFindObjectWrap
            ?? (p_IReadOnlyReactivePropertyFindObjectWrap = p_FindObjectWrap.ToSequentialReadOnlyReactiveProperty());



        /// <summary>
        /// 視界内でホロモンが見失ったもの
        /// </summary>
        [SerializeField, Tooltip("視界内でホロモンが見失ったもの")]
        private IntReactiveProperty p_LostObjectInstanceID
            = new IntReactiveProperty();

        /// <summary>
        /// 視界内でホロモンが見失ったもののReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<int> p_IReadOnlyReactivePropertyLostObjectInstanceID;

        /// <summary>
        /// 視界内でホロモンが見失ったもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<int> IReadOnlyReactivePropertyLostObjectInstanceID
            => p_IReadOnlyReactivePropertyLostObjectInstanceID
            ?? (p_IReadOnlyReactivePropertyLostObjectInstanceID = p_LostObjectInstanceID.ToSequentialReadOnlyReactiveProperty());


        /// <summary>
        /// 視界内でホロモンが状態変化に気づいたもの
        /// </summary>
        [SerializeField, Tooltip("視界内で状態変化に気づいたもの")]
        private VisionObjectWrapReactiveProperty p_UpdateStatusObjectWrap
            = new VisionObjectWrapReactiveProperty();

        /// <summary>
        /// 視界内でホロモンの状態変化に気づいたもののReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<VisionObjectWrap> p_IReadOnlyReactivePropertyUpdateStatusObjectWrap;

        /// <summary>
        /// 視界内でホロモンの状態変化に気づいたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<VisionObjectWrap> IReadOnlyReactivePropertyUpdateStatusObjectWrap
            => p_IReadOnlyReactivePropertyUpdateStatusObjectWrap
            ?? (p_IReadOnlyReactivePropertyUpdateStatusObjectWrap = p_UpdateStatusObjectWrap.ToSequentialReadOnlyReactiveProperty());


        /// <summary>
        /// 視界内でホロモンが距離変化に気づいたもの
        /// </summary>
        [SerializeField, Tooltip("視界内で距離変化に気づいたもの")]
        private VisionObjectWrapReactiveProperty p_UpdateRangeObjectWrap
            = new VisionObjectWrapReactiveProperty();

        /// <summary>
        /// 視界内でホロモンの距離変化に気づいたもののReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<VisionObjectWrap> p_IReadOnlyReactivePropertyUpdateRangeObjectWrap;

        /// <summary>
        /// 視界内でホロモンの距離変化に気づいたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<VisionObjectWrap> IReadOnlyReactivePropertyUpdateRangeObjectWrap
            => p_IReadOnlyReactivePropertyUpdateRangeObjectWrap
            ?? (p_IReadOnlyReactivePropertyUpdateRangeObjectWrap = p_UpdateRangeObjectWrap.ToSequentialReadOnlyReactiveProperty());




        /// <summary>
        /// 視界オブジェクト監視の参照
        /// </summary>
        [SerializeField, Tooltip("視界オブジェクト監視の参照")]
        private FieldOfVisionMonitor p_Monitor;
        public IEnumerable<VisionObjectWrap> VisionObjectWrapCollection => p_Monitor.ValueList();

        /// <summary>
        /// 視覚内オブジェクト条件検索の参照
        /// </summary>
        [SerializeField]
        private FieldOfVisionConditionSearcher p_ConditionSearcher;

        /// <summary>
        /// 視覚内オブジェクト範囲の参照
        /// </summary>
        [SerializeField]
        private FieldOfVisionRangeChecker p_RangeChecker;

        /// <summary>
        /// オブジェクト理解種別ごとの優先度リストの参照
        /// </summary>
        [SerializeField]
        private FieldOfVisionPriority p_Priority;
        public IEnumerable<ObjectUnderstandTypePriority> ObjectUnderstandTypePriorityList => p_Priority.PriorityList;


        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public VisionObjectWrap CheckCollectionByGameObject(GameObject a_Object)
        {
            return p_ConditionSearcher.CheckCollectionByGameObject(p_Monitor.ValueList(), a_Object);
        }


        /// <summary>
        /// 視界オブジェクトコレクションから指定条件が一致する
        /// かつ、距離が最も近いオブジェクトを探す
        /// </summary>
        public VisionObjectWrap CheckCollectionByNearDistance(
            bool a_CheckObjectName, string a_ObjectName,
            bool a_CheckNearDistance, float a_NearDistance,
            bool a_CheckInsideAngle, float a_InsideAngle,
            bool a_CheckObjectUnderstandType, ObjectUnderstandType a_ObjectUnderstandType
            )
        {
            return p_ConditionSearcher.CheckCollectionByNearDistance(
                p_Monitor.ValueList(),
                a_CheckObjectName, a_ObjectName,
                a_CheckNearDistance, a_NearDistance,
                a_CheckInsideAngle, a_InsideAngle,
                a_CheckObjectUnderstandType, a_ObjectUnderstandType
                );
        }

        /// <summary>
        /// 視界オブジェクトコレクションから指定条件が一致する
        /// かつ、優先度が最も近いオブジェクトを探す
        /// 優先度が同じ場合は距離の近いものを選択する
        /// </summary>
        public VisionObjectWrap CheckCollectionByTypePriority(
            bool a_CheckObjectName, string a_ObjectName,
            bool a_CheckNearDistance, float a_NearDistance,
            bool a_CheckInsideAngle, float a_InsideAngle
            )
        {
            return p_ConditionSearcher.CheckCollectionByTypePriority(
                p_Monitor.ValueList(),
                p_Priority.PriorityList,
                a_CheckObjectName, a_ObjectName,
                a_CheckNearDistance, a_NearDistance,
                a_CheckInsideAngle, a_InsideAngle
                );
        }


        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// 存在する場合はそのオブジェクトまでの距離を返す
        /// 視界内にゲームオブジェクトがなかった場合は -1.0f を返す
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public float CurrentDistanceForGameObject(GameObject a_Object, bool a_IgnoreYAxis)
        {
            VisionObjectWrap visionObjectWrap = p_ConditionSearcher.CheckCollectionByGameObject(p_Monitor.ValueList(), a_Object);
            if (visionObjectWrap == null) return -1.0f;

            return p_RangeChecker.TargetDistance(visionObjectWrap.Object, a_IgnoreYAxis);
        }

        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// 存在する場合はそのオブジェクトとの相対角度を返す
        /// 視界内にゲームオブジェクトがなかった場合は -1.0f を返す
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public float CurrentAngleForGameObject(GameObject a_Object, bool a_IgnoreYAxis)
        {
            VisionObjectWrap visionObjectWrap = p_ConditionSearcher.CheckCollectionByGameObject(p_Monitor.ValueList(), a_Object);
            if (visionObjectWrap == null) return -1.0f;

            return p_RangeChecker.TargetAngle(visionObjectWrap.Object, a_IgnoreYAxis);
        }

        /// <summary>
        /// 対象オブジェクトの視覚範囲を判定する
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public VisionRangeType CurrentVisionRangeAreaForGameObject(GameObject a_Object)
        {
            return p_RangeChecker.TargetVisionRangeArea(a_Object);
        }


        private void OnEnable()
        {
            // コレクションの追加、削除イベントを追加
            AddSetValueAndForceNotifyEvents();
        }

        private void OnDisable()
        {
            // コレクションの追加、削除イベントを削除
            RemoveSetValueAndForceNotifyEvents();
        }

        #region "apply events"
        /// <summary>
        /// コレクションの追加、削除イベントを追加
        /// </summary>
        private void AddSetValueAndForceNotifyEvents()
        {
            p_Monitor.FindObjectWrapEvent += ApplyFindObjectWrap;
            p_Monitor.LostObjectNameEvent += ApplyLostObjectName;
            p_Monitor.UpdateStatusObjectWrapEvent += ApplyUpdateStatusObjectWrap;
            p_Monitor.UpdateRangeObjectWrapEvent += ApplyUpdateRangeObjectWrap;
        }

        /// <summary>
        /// コレクションの追加、削除イベントを削除
        /// </summary>
        private void RemoveSetValueAndForceNotifyEvents()
        {
            p_Monitor.FindObjectWrapEvent -= ApplyFindObjectWrap;
            p_Monitor.LostObjectNameEvent -= ApplyLostObjectName;
            p_Monitor.UpdateStatusObjectWrapEvent -= ApplyUpdateStatusObjectWrap;
            p_Monitor.UpdateRangeObjectWrapEvent -= ApplyUpdateRangeObjectWrap;
        }

        /// <summary>
        /// 見つけたオブジェクトを設定する
        /// </summary>
        /// <returns></returns>
        private void ApplyFindObjectWrap(VisionObjectWrap a_VisionObjectWrap)
        {
            p_FindObjectWrap.SetValueAndForceNotify(a_VisionObjectWrap);
        }

        /// <summary>
        /// 見失ったオブジェクトを設定する
        /// </summary>
        /// <returns></returns>
        private void ApplyLostObjectName(int a_ObjectInstanceID)
        {
            p_LostObjectInstanceID.SetValueAndForceNotify(a_ObjectInstanceID);
        }

        /// <summary>
        /// 状態変化に気づいたオブジェクトを設定する
        /// </summary>
        /// <returns></returns>
        private void ApplyUpdateStatusObjectWrap(VisionObjectWrap a_VisionObjectWrap)
        {
            p_UpdateStatusObjectWrap.SetValueAndForceNotify(a_VisionObjectWrap);
        }

        /// <summary>
        /// 距離変化に気づいたオブジェクトを設定する
        /// </summary>
        /// <returns></returns>
        private void ApplyUpdateRangeObjectWrap(VisionObjectWrap a_VisionObjectWrap)
        {
            p_UpdateRangeObjectWrap.SetValueAndForceNotify(a_VisionObjectWrap);
        }
        #endregion
    }
}