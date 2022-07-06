using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Model.Sensations.TactileBody
{
    /// <summary>
    /// 触覚システムAPI
    /// </summary>
    public class HoloMonTactileBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// 触覚内でホロモンが見つけたもの
        /// </summary>
        [SerializeField, Tooltip("触覚内でホロモンが見つけたもの")]
        private TactileObjectWrapReactiveProperty p_FindObjectWrap
            = new TactileObjectWrapReactiveProperty();

        /// <summary>
        /// 触覚内でホロモンの見つけたもののReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<TactileObjectWrap> p_IReadOnlyReactivePropertyFindObjectWrap;

        /// <summary>
        /// 触覚内でホロモンの見つけたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<TactileObjectWrap> IReadOnlyReactivePropertyFindObjectWrap
            => p_IReadOnlyReactivePropertyFindObjectWrap
            ?? (p_IReadOnlyReactivePropertyFindObjectWrap = p_FindObjectWrap.ToSequentialReadOnlyReactiveProperty());



        /// <summary>
        /// 触覚内でホロモンが見失ったもの
        /// </summary>
        [SerializeField, Tooltip("触覚内でホロモンが見失ったもの")]
        private IntReactiveProperty p_LostObjectInstanceID
            = new IntReactiveProperty();

        /// <summary>
        /// 触覚内でホロモンが見失ったもののReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<int> p_IReadOnlyReactivePropertyLostObjectInstanceID;

        /// <summary>
        /// 触覚内でホロモンが見失ったもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<int> IReadOnlyReactivePropertyLostObjectInstanceID
            => p_IReadOnlyReactivePropertyLostObjectInstanceID
            ?? (p_IReadOnlyReactivePropertyLostObjectInstanceID = p_LostObjectInstanceID.ToSequentialReadOnlyReactiveProperty());


        /// <summary>
        /// 触覚内でホロモンが状態変化に気づいたもの
        /// </summary>
        [SerializeField, Tooltip("触覚内で状態変化に気づいたもの")]
        private TactileObjectWrapReactiveProperty p_UpdateStatusObjectWrap
            = new TactileObjectWrapReactiveProperty();

        /// <summary>
        /// 触覚内でホロモンの状態変化に気づいたもののReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<TactileObjectWrap> p_IReadOnlyReactivePropertyUpdateStatusObjectWrap;

        /// <summary>
        /// 触覚内でホロモンの状態変化に気づいたもののReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<TactileObjectWrap> IReadOnlyReactivePropertyUpdateStatusObjectWrap
            => p_IReadOnlyReactivePropertyUpdateStatusObjectWrap
            ?? (p_IReadOnlyReactivePropertyUpdateStatusObjectWrap = p_UpdateStatusObjectWrap.ToSequentialReadOnlyReactiveProperty());



        /// <summary>
        /// 触覚内オブジェクト監視の参照
        /// </summary>
        [SerializeField]
        private TactileBodyMonitor p_Monitor;
        public IEnumerable<TactileObjectWrap> TactileObjectWrapCollection => p_Monitor.ValueList();

        /// <summary>
        /// 触覚内オブジェクト条件検索の参照
        /// </summary>
        [SerializeField]
        private TactileBodyConditionSearcher p_ConditionSearcher;

        /// <summary>
        /// 触覚内オブジェクト範囲の参照
        /// </summary>
        [SerializeField]
        private TactileBodyRangeChecker p_RangeChecker;


        /// <summary>
        /// 触覚内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// </summary>
        public TactileObjectWrap CheckCollectionByGameObject(GameObject a_Object)
        {
            return p_ConditionSearcher.CheckCollectionByGameObject(p_Monitor.ValueList(), a_Object);
        }

        /// <summary>
        /// 触覚オブジェクトコレクションから指定条件が一致する
        /// かつ、距離が最も近いオブジェクトを探す
        /// </summary>
        public TactileObjectWrap CheckCollectionByNearDistance(
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
        /// 対象オブジェクトの触覚範囲を判定する
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public TactileRangeType CurrentTactileRangeAreaForGameObject(GameObject a_Object)
        {
            return p_RangeChecker.TargetTactileRangeArea(a_Object);
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
        }

        /// <summary>
        /// コレクションの追加、削除イベントを削除
        /// </summary>
        private void RemoveSetValueAndForceNotifyEvents()
        {
            p_Monitor.FindObjectWrapEvent -= ApplyFindObjectWrap;
            p_Monitor.LostObjectNameEvent -= ApplyLostObjectName;
            p_Monitor.UpdateStatusObjectWrapEvent -= ApplyUpdateStatusObjectWrap;
        }

        /// <summary>
        /// 見つけたオブジェクトを設定する
        /// </summary>
        /// <returns></returns>
        private void ApplyFindObjectWrap(TactileObjectWrap a_TactileObjectWrap)
        {
            p_FindObjectWrap.SetValueAndForceNotify(a_TactileObjectWrap);
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
        private void ApplyUpdateStatusObjectWrap(TactileObjectWrap a_TactileObjectWrap)
        {
            p_UpdateStatusObjectWrap.SetValueAndForceNotify(a_TactileObjectWrap);
        }
        #endregion
    }
}