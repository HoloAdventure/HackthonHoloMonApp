using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

// ToList使用のため
using System.Linq;

namespace HoloMonApp.Content.Character.Data.Sensations.TactileBody
{
    /// <summary>
    /// 触覚内の視界オブジェクトコレクション
    /// </summary>
    public class TactileBodyCollectionData : MonoBehaviour
    {
        /// <summary>
        /// 触覚内の視認オブジェクトリスト
        /// </summary>
        private Dictionary<int, TactileObjectWrap> p_TactileObjectDictionary
            = new Dictionary<int, TactileObjectWrap>();

        [SerializeField, Tooltip("触覚内の視認オブジェクトリスト(Editor確認用)")]
        private List<TactileObjectWrap> p_TactileObjectWrapList;

        private void LateUpdate()
        {
# if UNITY_EDITOR
            // Editor確認用
            if (EditorApplication.isPlaying)
            {
                p_TactileObjectWrapList = p_TactileObjectDictionary.Values.ToList();
            }
# endif
        }

        /// <summary>
        /// オブジェクト追加
        /// </summary>
        public void Add(TactileObjectWrap a_VisionObjectWrap)
        {
            p_TactileObjectDictionary.Add(a_VisionObjectWrap.Object.GetInstanceID(), a_VisionObjectWrap);
        }

        /// <summary>
        /// オブジェクト削除
        /// </summary>
        public bool Remove(GameObject a_Object)
        {
            return p_TactileObjectDictionary.Remove(a_Object.GetInstanceID());
        }

        /// <summary>
        /// オブジェクト削除(ID指定)
        /// </summary>
        public bool Remove(int a_InstanceID)
        {
            return p_TactileObjectDictionary.Remove(a_InstanceID);
        }

        /// <summary>
        /// TactileObjectの取得
        /// </summary>
        public TactileObjectWrap GetTactileObject(GameObject a_Object)
        {
            return p_TactileObjectDictionary[a_Object.GetInstanceID()];
        }

        /// <summary>
        /// TactileObjectの取得(ID指定)
        /// </summary>
        public TactileObjectWrap GetTactileObject(int a_InstanceID)
        {
            return p_TactileObjectDictionary[a_InstanceID];
        }

        /// <summary>
        /// 値の存在チェック
        /// </summary>
        public bool ContainsKey(GameObject a_Object)
        {
            return p_TactileObjectDictionary.ContainsKey(a_Object.GetInstanceID());
        }

        /// <summary>
        /// 値の存在チェック(ID指定)
        /// </summary>
        public bool ContainsKey(int a_InstanceID)
        {
            return p_TactileObjectDictionary.ContainsKey(a_InstanceID);
        }

        /// <summary>
        /// キー一覧の取得
        /// </summary>
        public IEnumerable<int> KeyList()
        {
            return p_TactileObjectDictionary.Keys.ToList();
        }

        /// <summary>
        /// 値一覧の取得
        /// </summary>
        public IEnumerable<TactileObjectWrap> ValueList()
        {
            return p_TactileObjectDictionary.Values.ToList();
        }
    }
}
