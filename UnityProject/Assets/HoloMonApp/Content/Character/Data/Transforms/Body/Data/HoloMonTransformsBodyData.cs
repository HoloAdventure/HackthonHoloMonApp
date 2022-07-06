using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace HoloMonApp.Content.Character.Data.Transforms.Body
{
    /// <summary>
    /// ボディトランスフォームDATA
    /// </summary>
    public class HoloMonTransformsBodyData : MonoBehaviour
    {
        [SerializeField, Tooltip("全体(Root)トランスフォームの参照")]
        private Transform p_Root;
        public Transform Root => p_Root;

        [SerializeField, Tooltip("頭部(head)ボーンの参照")]
        private Transform p_Head;
        public Transform Head => p_Head;

        [SerializeField, Tooltip("右手ボーンの参照")]
        private Transform p_RightHand;
        public Transform RightHand => p_RightHand;

        [SerializeField, Tooltip("左手ボーンの参照")]
        private Transform p_LeftHand;
        public Transform LeftHand => p_LeftHand;

        [SerializeField, Tooltip("右瞳孔の収縮ボーンの参照")]
        private Transform p_RightEyeShrinkage;
        public Transform RightEyeShrinkage => p_RightEyeShrinkage;

        [SerializeField, Tooltip("左瞳孔の収縮ボーンの参照")]
        private Transform p_LeftEyeShrinkage;
        public Transform LeftEyeShrinkage => p_LeftEyeShrinkage;
    }
}