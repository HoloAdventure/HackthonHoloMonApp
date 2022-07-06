using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.References.Transforms.Bone
{
    /// <summary>
    /// ボディボーンAPI
    /// </summary>
    public class HoloMonTransformsBoneAPI : MonoBehaviour
    {
        /// <summary>
        /// データの参照
        /// </summary>
        [SerializeField]
        private HoloMonTransformsBodyData p_BodyTransformsDATA;

        /// <summary>
        /// 頭部(head)ボーンの参照
        /// </summary>
        public Transform Head => p_BodyTransformsDATA.Head;

        /// <summary>
        /// 右手ボーンの参照
        /// </summary>
        public Transform RightHand => p_BodyTransformsDATA.RightHand;

        /// <summary>
        /// 左手ボーンの参照
        /// </summary>
        public Transform LeftHand => p_BodyTransformsDATA.LeftHand;
    }
}