using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.Player
{
    public class HeadTracker : MonoBehaviour
    {
        [SerializeField, Tooltip("頭部オブジェクトの参照")]
        private GameObject p_HeadObject;

        /// <summary>
        /// 頭部の追跡対象
        /// </summary>
        private Transform p_TrackingTransform => HoloMonXRPlatform.Instance.MainCamera.Access.GetMainCamera().transform;


        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            HeadTracking();
        }

        /// <summary>
        /// 頭部オブジェクトの追跡設定
        /// </summary>
        private void HeadTracking()
        {
            p_HeadObject.transform.position = p_TrackingTransform.position;
            p_HeadObject.transform.rotation = p_TrackingTransform.rotation;
        }
    }
}