using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace HoloMonApp.Content.Character.Data.Transforms.Around
{
    /// <summary>
    /// 周囲トランスフォームDATA
    /// </summary>
    public class HoloMonTransformsAroundData : MonoBehaviour
    {
        [SerializeField, Tooltip("うんち出現トランスフォームの参照")]
        private Transform p_PoopOutlet;
        public Transform PoopOutlet => p_PoopOutlet;
    }
}