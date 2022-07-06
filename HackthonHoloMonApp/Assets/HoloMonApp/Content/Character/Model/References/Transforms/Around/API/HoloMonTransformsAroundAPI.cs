using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Data.Transforms.Around;

namespace HoloMonApp.Content.Character.Model.References.Transforms.Around
{
    /// <summary>
    /// 周囲トランスフォームAPI
    /// </summary>
    public class HoloMonTransformsAroundAPI : MonoBehaviour
    {
        /// <summary>
        /// データの参照
        /// </summary>
        [SerializeField]
        private HoloMonTransformsAroundData p_AroundTransformsDATA;

        /// <summary>
        /// うんち出現トランスフォームの参照
        /// </summary>
        public Transform PoopOutlet => p_AroundTransformsDATA.PoopOutlet;
    }
}