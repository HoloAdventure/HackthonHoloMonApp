using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.View.References.Transforms.Around;
using HoloMonApp.Content.Character.View.References.Transforms.Body;
using HoloMonApp.Content.Character.View.References.Transforms.Bone;

namespace HoloMonApp.Content.Character.View.References.Transforms
{
    /// <summary>
    /// トランスフォームデータAPI
    /// </summary>
    public class HoloMonViewTransformsAPI : MonoBehaviour
    {
        /// <summary>
        /// 周囲トランスフォームの参照
        /// </summary>
        [SerializeField, Tooltip("周囲トランスフォームの参照")]
        private HoloMonViewTransformsAroundAPI p_Around;
        public HoloMonViewTransformsAroundAPI Around => p_Around;

        /// <summary>
        /// ボディトランスフォームの参照
        /// </summary>
        [SerializeField, Tooltip("ボディトランスフォームの参照")]
        private HoloMonViewTransformsBodyAPI p_Body;
        public HoloMonViewTransformsBodyAPI Body => p_Body;

        /// <summary>
        /// ボディボーンの参照
        /// </summary>
        [SerializeField, Tooltip("ボディボーンの参照")]
        private HoloMonViewTransformsBoneAPI p_Bone;
        public HoloMonViewTransformsBoneAPI Bone => p_Bone;
    }
}