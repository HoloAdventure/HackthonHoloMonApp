using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Model.References.Transforms.Around;

namespace HoloMonApp.Content.Character.View.References.Transforms.Around
{
    /// <summary>
    /// 周囲トランスフォームAPI
    /// </summary>
    public class HoloMonViewTransformsAroundAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonTransformsAroundAPI p_TransformsAroundAPI;

        /// <summary>
        /// うんち出現ポジションの参照用変数
        /// </summary>
        public Vector3 WorldPoopOutletPosition => p_TransformsAroundAPI.PoopOutlet.position;

        /// <summary>
        /// うんち出現ローテーションの参照用変数
        /// </summary>
        public Quaternion WorldPoopOutletRotation => p_TransformsAroundAPI.PoopOutlet.rotation;
    }
}