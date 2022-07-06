using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Model.References.Transforms.Body;

namespace HoloMonApp.Content.Character.View.References.Transforms.Body
{
    /// <summary>
    /// ボディトランスフォームAPI
    /// </summary>
    public class HoloMonViewTransformsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonTransformsBodyAPI p_TransformsBodyAPI;

        /// <summary>
        /// 身体ポジションの参照用変数
        /// </summary>
        public Vector3 WorldPosition => p_TransformsBodyAPI.Body.position;

        /// <summary>
        /// 身体ローテーションの参照用変数
        /// </summary>
        public Quaternion WorldRotation => p_TransformsBodyAPI.Body.rotation;

        /// <summary>
        /// 身体正面方向ベクトルの参照用変数
        /// </summary>
        public Vector3 WorldForward => p_TransformsBodyAPI.Body.forward;

        /// <summary>
        /// 身体上方向ベクトルの参照用変数
        /// </summary>
        public Vector3 WorldUp => p_TransformsBodyAPI.Body.up;

        /// <summary>
        /// 身長の参照用変数
        /// (ワールド空間におけるBodyのY軸スケール)
        /// </summary>
        public float WorldHeight => p_TransformsBodyAPI.Body.lossyScale.y;
    }
}