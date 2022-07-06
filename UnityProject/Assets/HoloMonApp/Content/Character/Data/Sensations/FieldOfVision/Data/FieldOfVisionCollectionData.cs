using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

// ToList使用のため
using System.Linq;

using HoloMonApp.Content.Character.Model.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Data.Sensations.FieldOfVision
{
    /// <summary>
    /// 視界内の視界オブジェクトコレクション
    /// </summary>
    public class FieldOfVisionCollectionData : MonoBehaviour
    {
        /// <summary>
        /// 視界内の視認オブジェクトリスト
        /// </summary>
        private Dictionary<int, VisionObjectWrap> p_VisionObjectDictionary
            = new Dictionary<int, VisionObjectWrap>();

        [SerializeField, Tooltip("視界内の視認オブジェクトリスト(Editor確認用)")]
        private List<VisionObjectWrap> p_FieldOfVisionWrapList;

        void LateUpdate()
        {
# if UNITY_EDITOR
            // Editor確認用
            if (EditorApplication.isPlaying)
            {
                p_FieldOfVisionWrapList = p_VisionObjectDictionary.Values.ToList();
            }
# endif
        }

        /// <summary>
        /// オブジェクト追加
        /// </summary>
        public void Add(VisionObjectWrap a_VisionObjectWrap)
        {
            p_VisionObjectDictionary.Add(a_VisionObjectWrap.Object.GetInstanceID(), a_VisionObjectWrap);
        }

        /// <summary>
        /// オブジェクト削除
        /// </summary>
        public bool Remove(GameObject a_Object)
        {
            return p_VisionObjectDictionary.Remove(a_Object.GetInstanceID());
        }

        /// <summary>
        /// オブジェクト削除(ID指定)
        /// </summary>
        public bool Remove(int a_InstanceID)
        {
            return p_VisionObjectDictionary.Remove(a_InstanceID);
        }

        /// <summary>
        /// VisionObjectの取得
        /// </summary>
        public VisionObjectWrap GetVisionObject(GameObject a_Object)
        {
            return p_VisionObjectDictionary[a_Object.GetInstanceID()];
        }

        /// <summary>
        /// VisionObjectの取得(ID指定)
        /// </summary>
        public VisionObjectWrap GetVisionObject(int a_InstanceID)
        {
            return p_VisionObjectDictionary[a_InstanceID];
        }

        /// <summary>
        /// 値の存在チェック
        /// </summary>
        public bool ContainsKey(GameObject a_Object)
        {
            return p_VisionObjectDictionary.ContainsKey(a_Object.GetInstanceID());
        }

        /// <summary>
        /// 値の存在チェック(ID指定)
        /// </summary>
        public bool ContainsKey(int a_InstanceID)
        {
            return p_VisionObjectDictionary.ContainsKey(a_InstanceID);
        }

        /// <summary>
        /// キー一覧の取得
        /// </summary>
        public List<int> KeyList()
        {
            return p_VisionObjectDictionary.Keys.ToList();
        }

        /// <summary>
        /// 値一覧の取得
        /// </summary>
        public List<VisionObjectWrap> ValueList()
        {
            return p_VisionObjectDictionary.Values.ToList();
        }
    }
}
