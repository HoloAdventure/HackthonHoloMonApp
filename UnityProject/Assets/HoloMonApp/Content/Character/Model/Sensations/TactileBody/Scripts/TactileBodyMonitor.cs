using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

// ToList使用のため
using System.Linq;

using HoloMonApp.Content.Character.Data.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Model.Sensations.TactileBody
{
    /// <summary>
    /// 触覚内の視界オブジェクト監視
    /// </summary>
    public class TactileBodyMonitor : MonoBehaviour
    {
        /// <summary>
        /// コレクションデータ
        /// </summary>
        [SerializeField]
        private TactileBodyCollectionData p_CollectionData;

        /// <summary>
        /// 触覚内の特徴ハッシュ値リスト
        /// </summary>
        private Dictionary<int, int> p_TactileObjectFeatureHashDictionary
            = new Dictionary<int, int>();

        [SerializeField, Tooltip("触覚内の特徴ハッシュ値リスト(Editor確認用)")]
        private List<int> p_TactileObjectFeatureHashList;


        /// <summary>
        /// 発見イベント
        /// </summary>
        public Action<TactileObjectWrap> FindObjectWrapEvent;

        /// <summary>
        /// ロストイベント
        /// </summary>
        public Action<int> LostObjectNameEvent;

        /// <summary>
        /// 状態アップデートイベント
        /// </summary>
        public Action<TactileObjectWrap> UpdateStatusObjectWrapEvent;

        private void UpdateList()
        {
# if UNITY_EDITOR
            // Editor確認用
            if (EditorApplication.isPlaying)
            {
                p_TactileObjectFeatureHashList = p_TactileObjectFeatureHashDictionary.Values.ToList();
            }
# endif
        }


        /// <summary>
        /// オブジェクト追加
        /// </summary>
        public void Add(TactileObjectWrap a_VisionObjectWrap)
        {
            p_CollectionData.Add(a_VisionObjectWrap);
            p_TactileObjectFeatureHashDictionary.Add(a_VisionObjectWrap.Object.GetInstanceID(), a_VisionObjectWrap.CurrentStatusHash());
            UpdateList();
            FindObjectWrapEvent.Invoke(a_VisionObjectWrap);
        }

        /// <summary>
        /// オブジェクト削除
        /// </summary>
        public bool Remove(GameObject a_Object)
        {
            bool result = p_CollectionData.Remove(a_Object.GetInstanceID());
            p_TactileObjectFeatureHashDictionary.Remove(a_Object.GetInstanceID());
            UpdateList();
            if (result)
            {
                LostObjectNameEvent.Invoke(a_Object.GetInstanceID());
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
                TactileObjectWrap checkTactileObjectWrap = p_CollectionData.GetTactileObject(key);
                // 状態の変化チェック
                int currentStatusHash = checkTactileObjectWrap.CurrentStatusHash();
                if (currentStatusHash != p_TactileObjectFeatureHashDictionary[key])
                {
                    p_TactileObjectFeatureHashDictionary[key] = currentStatusHash;
                    UpdateList();
                    UpdateStatusObjectWrapEvent.Invoke(checkTactileObjectWrap);
                    isUpdated = true;
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// TactileObjectの取得
        /// </summary>
        public TactileObjectWrap GetTactileObject(int a_ObjectID)
        {
            return p_CollectionData.GetTactileObject(a_ObjectID);
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
        public IEnumerable<int> KeyList()
        {
            return p_CollectionData.KeyList();
        }

        /// <summary>
        /// 値一覧の取得
        /// </summary>
        public IEnumerable<TactileObjectWrap> ValueList()
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
                    p_TactileObjectFeatureHashDictionary.Remove(key);
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
