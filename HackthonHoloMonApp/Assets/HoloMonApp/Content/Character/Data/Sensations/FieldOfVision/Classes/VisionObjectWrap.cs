using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Data.Sensations.FieldOfVision
{
    /// <summary>
    /// 視認オブジェクトのためのラップアクセサ
    /// </summary>
    [Serializable]
    public class VisionObjectWrap
    {
        [SerializeField, Tooltip("オブジェクト理解種別情報")]
        private ObjectFeatureWrap p_ObjectFeatureData;

        /// <summary>
        /// オブジェクト理解種別情報
        /// </summary>
        public ObjectFeatureWrap ObjectFeatureData => p_ObjectFeatureData;

        /// <summary>
        /// ゲームオブジェクト
        /// </summary>
        public GameObject Object => p_ObjectFeatureData?.GameObject;

        public VisionObjectWrap(ObjectFeatureWrap a_ObjectFeatureData)
        {
            p_ObjectFeatureData = a_ObjectFeatureData;
        }

        /// <summary>
        /// 現在のオブジェクト名
        /// </summary>
        public string CurrentName()
        {
            return p_ObjectFeatureData.ObjectName;
        }

        /// <summary>
        /// 現在のオブジェクト理解種別情報
        /// </summary>
        public ObjectUnderstandInformation CurrentFeatures()
        {
            return p_ObjectFeatureData.UnderstandInformation;
        }

        /// <summary>
        /// 現在のオブジェクト理解種別情報の状態ハッシュ値を返す
        /// </summary>
        public int CurrentStatusHash()
        {
            return p_ObjectFeatureData.UnderstandInformation.ObjectUnderstandDataInterface.StatusHash();
        }
    }
}