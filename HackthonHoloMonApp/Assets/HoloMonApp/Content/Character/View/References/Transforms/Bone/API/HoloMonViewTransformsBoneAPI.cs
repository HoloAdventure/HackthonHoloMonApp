using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Model;
using HoloMonApp.Content.Character.Model.References.Transforms.Bone;

namespace HoloMonApp.Content.Character.View.References.Transforms.Bone
{
    /// <summary>
    /// ボディボーンAPI
    /// </summary>
    public class HoloMonViewTransformsBoneAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonTransformsBoneAPI p_TransformsBoneAPI;

        /// <summary>
        /// 頭部ポジションの参照用変数
        /// </summary>
        public Vector3 HeadWorldPosition => p_TransformsBoneAPI.Head.position;

        /// <summary>
        /// 頭部ローテーションの参照用変数
        /// </summary>
        public Quaternion HeadWorldRotation => p_TransformsBoneAPI.Head.rotation;

        /// <summary>
        /// 右手ポジションの参照用変数
        /// </summary>
        public Vector3 RightHandWorldPosition => p_TransformsBoneAPI.RightHand.position;

        /// <summary>
        /// 右手ローテーションの参照用変数
        /// </summary>
        public Quaternion RightHandWorldRotation => p_TransformsBoneAPI.RightHand.rotation;

        /// <summary>
        /// 左手ポジションの参照用変数
        /// </summary>
        public Vector3 LeftHandWorldPosition => p_TransformsBoneAPI.LeftHand.position;

        /// <summary>
        /// 左手ローテーションの参照用変数
        /// </summary>
        public Quaternion LeftHandWorldRotation => p_TransformsBoneAPI.LeftHand.rotation;
    }
}