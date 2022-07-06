using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

// ToList使用のため
using System.Linq;

using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    /// <summary>
    /// 視界内の視界オブジェクト監視
    /// </summary>
    public class FieldOfVisionMonitor : MonoBehaviour
    {
        /// <summary>
        /// コレクションデータ
        /// </summary>
        [SerializeField]
        private FieldOfVisionCollectionData p_CollectionData;

        /// <summary>
        /// 距離チェッカーの参照
        /// </summary>
        [SerializeField]
        private FieldOfVisionRangeChecker p_RangeChecker;

        /// <summary>
        /// 視界内の特徴ハッシュ値リスト
        /// </summary>
        private Dictionary<int, int> p_VisionObjectFeatureHashDictionary
            = new Dictionary<int, int>();

        [SerializeField, Tooltip("視界内の特徴ハッシュ値リスト(Editor確認用)")]
        private List<int> p_VisionObjectFeatureHashList;

        /// <summary>
        /// 視界内の距離範囲ハッシュ値リスト
        /// </summary>
        private Dictionary<int, VisionRangeType> p_VisionObjectRangeTypeDictionary
            = new Dictionary<int, VisionRangeType>();

        [SerializeField, Tooltip("視界内の距離範囲ハッシュ値リスト(Editor確認用)")]
        private List<VisionRangeType> p_VisionObjectRangeTypeList;


        /// <summary>
        /// 発見イベント
        /// </summary>
        public Action<VisionObjectWrap> FindObjectWrapEvent;

        /// <summary>
        /// ロストイベント
        /// </summary>
        public Action<int> LostObjectNameEvent;

        /// <summary>
        /// 状態アップデートイベント
        /// </summary>
        public Action<VisionObjectWrap> UpdateStatusObjectWrapEvent;

        /// <summary>
        /// 距離種別アップデートイベント
        /// </summary>
        public Action<VisionObjectWrap> UpdateRangeObjectWrapEvent;


        private void UpdateList()
        {
# if UNITY_EDITOR
            // Editor確認用
            if (EditorApplication.isPlaying)
            {
                p_VisionObjectFeatureHashList = p_VisionObjectFeatureHashDictionary.Values.ToList();
                p_VisionObjectRangeTypeList = p_VisionObjectRangeTypeDictionary.Values.ToList();
            }
# endif
        }


        /// <summary>
        /// オブジェクト追加
        /// </summary>
        public void Add(VisionObjectWrap a_VisionObjectWrap)
        {
            p_CollectionData.Add(a_VisionObjectWrap);
            p_VisionObjectFeatureHashDictionary.Add(a_VisionObjectWrap.Object.GetInstanceID(), a_VisionObjectWrap.CurrentStatusHash());
            p_VisionObjectRangeTypeDictionary.Add(a_VisionObjectWrap.Object.GetInstanceID(), p_RangeChecker.TargetVisionRangeArea(a_VisionObjectWrap.Object));
            UpdateList();
            FindObjectWrapEvent.Invoke(a_VisionObjectWrap);
        }

        /// <summary>
        /// オブジェクト削除
        /// </summary>
        public bool Remove(int a_ObjectID)
        {
            bool result = p_CollectionData.Remove(a_ObjectID);
            p_VisionObjectFeatureHashDictionary.Remove(a_ObjectID);
            p_VisionObjectRangeTypeDictionary.Remove(a_ObjectID);
            UpdateList();
            if (result)
            {
                LostObjectNameEvent.Invoke(a_ObjectID);
            }
            return result;
        }

        /// <summary>
        /// 状態の更新チェック
        /// </summary>
        public bool CheckStatusUpdate()
        {
            bool isUpdated = false;

            foreach(int key in p_CollectionData.KeyList())
            {
                VisionObjectWrap checkVisionObjectWrap = p_CollectionData.GetVisionObject(key);
                // 状態の変化チェック
                int currentStatusHash = checkVisionObjectWrap.CurrentStatusHash();
                if (currentStatusHash != p_VisionObjectFeatureHashDictionary[key])
                {
                    p_VisionObjectFeatureHashDictionary[key] = currentStatusHash;
                    UpdateList();
                    UpdateStatusObjectWrapEvent.Invoke(checkVisionObjectWrap);
                    isUpdated = true;
                }
                // 距離範囲の変化チェック
                VisionRangeType currentRangeType = p_RangeChecker.TargetVisionRangeArea(checkVisionObjectWrap.Object);
                if (currentRangeType != p_VisionObjectRangeTypeDictionary[key])
                {
                    p_VisionObjectRangeTypeDictionary[key] = currentRangeType;
                    UpdateList();
                    UpdateRangeObjectWrapEvent.Invoke(checkVisionObjectWrap);
                    isUpdated = true;
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// VisionObjectの取得
        /// </summary>
        public VisionObjectWrap GetVisionObject(int a_ObjectID)
        {
            return p_CollectionData.GetVisionObject(a_ObjectID);
        }

        /// <summary>
        /// 値の存在チェック
        /// </summary>
        public bool ContainsKey(int a_ObjectID)
        {
            return p_CollectionData.ContainsKey(a_ObjectID);
        }

        /// <summary>
        /// キー一覧の取得
        /// </summary>
        public List<int> KeyList()
        {
            return p_CollectionData.KeyList();
        }

        /// <summary>
        /// 値一覧の取得
        /// </summary>
        public List<VisionObjectWrap> ValueList()
        {
            return p_CollectionData.ValueList();
        }

        /// <summary>
        /// 指定リストに含まれないオブジェクトをコレクションから除去する
        /// </summary>
        public bool RemoveWithoutList(List<int> a_CheckObjectIDList)
        {
            bool isRemoved = false;

            foreach (int key in p_CollectionData.KeyList())
            {
                if (!a_CheckObjectIDList.Contains(key))
                {
                    // 検出されていなければコレクションから削除する
                    bool result = p_CollectionData.Remove(key);
                    p_VisionObjectFeatureHashDictionary.Remove(key);
                    p_VisionObjectRangeTypeDictionary.Remove(key);
                    UpdateList();
                    if (result)
                    {
                        LostObjectNameEvent.Invoke(key);
                    }
                    isRemoved = true;
                }
            }

            return isRemoved;
        }
    }
}
