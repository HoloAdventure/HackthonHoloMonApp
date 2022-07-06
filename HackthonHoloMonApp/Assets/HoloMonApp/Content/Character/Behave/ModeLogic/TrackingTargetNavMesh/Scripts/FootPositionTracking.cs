using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh
{
    public class FootPositionTracking : MonoBehaviour
    {
        /// <summary>
        /// 空間認識レイヤー
        /// </summary>
        private int p_SpatialAwarenessLayer => HoloMonXRPlatform.Instance.EnvironmentMap.Access.GetEnvironmentMapLayer();

        /// <summary>
        /// 現在の足元トランスフォーム
        /// </summary>
        public Transform FootTransform => this.transform;

        /// <summary>
        /// 追跡跡オブジェクトのトランスフォーム
        /// </summary>
        [SerializeField, Tooltip("追跡オブジェクトのトランスフォーム")]
        private Transform p_TrackingTransform;

        /// <summary>
        /// 高さのオフセット
        /// </summary>
        [SerializeField, Tooltip("高さのオフセット")]
        private float p_YAxisOffset = 0.1f;

        /// <summary>
        /// 足元までの上限距離
        /// </summary>
        [SerializeField, Tooltip("足元までの上限距離")]
        private float p_HeightMax = 2.0f;


        /// <summary>
        /// 定期処理
        /// </summary>
        void LateUpdate()
        {
            if(p_TrackingTransform == null)
            {
                return;
            }

            // 追跡の正否
            bool isTracking = false;

            // レイキャストを追跡対象の位置から真下に落とす
            Ray ray = new Ray(p_TrackingTransform.position, -Vector3.up);
            RaycastHit hitInfo = new RaycastHit();

            // 空間認識レイヤー用マスク
            int layerMask = 1 << p_SpatialAwarenessLayer;

            // レイキャストのヒット情報を取得する
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
            {
                // ヒット位置が上限距離を超えているかチェックする
                if (hitInfo.distance < p_HeightMax)
                {
                    // 超えていなければヒット位置を足元として自身の位置を移動する
                    this.transform.position = new Vector3(
                        hitInfo.point.x,
                        hitInfo.point.y + p_YAxisOffset,
                        hitInfo.point.z);
                    // 追跡の成功
                    isTracking = true;
                }
            }
            if (isTracking == false)
            {
                // ヒットに失敗した場合は追跡対象の真下かつ上限距離の位置に自身の位置を移動する
                this.transform.position = new Vector3(
                    p_TrackingTransform.position.x,
                    p_TrackingTransform.position.y - p_HeightMax,
                    p_TrackingTransform.position.z);
            }
        }

        public void SetTrackingTransform(Transform a_TargetTransform)
        {
            p_TrackingTransform = a_TargetTransform;
        }

        public void ClearTrackingTransform()
        {
            p_TrackingTransform = null;
        }
    }
}