using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.References.Transforms.Body
{
    /// <summary>
    /// ボディトランスフォームAPI
    /// </summary>
    public class HoloMonTransformsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// データの参照
        /// </summary>
        [SerializeField]
        private HoloMonTransformsBodyData p_BodyTransformsDATA;

        /// <summary>
        /// ボディ(root)トランスフォームの参照
        /// </summary>
        public Transform Body => p_BodyTransformsDATA.Root;
    }
}