using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.Player
{
    public class HeadHeightChecker : MonoBehaviour
    {
        /// <summary>
        /// 空間認識レイヤー
        /// </summary>
        private int p_SpatialAwarenessLayer => HoloMonXRPlatform.Instance.EnvironmentMap.Access.GetEnvironmentMapLayer();

        [SerializeField, Tooltip("高さの上限値(m)")]
        private float p_HeightMax = 2.0f;

        [SerializeField, Tooltip("現在の高さ(m)")]
        private float p_CurrentHeight;

        /// <summary>
        /// 現在の高さ(m)
        /// </summary>
        public float CurrentHeight => p_CurrentHeight;


        // Update is called once per frame
        void Update()
        {
            // レイキャストをプレイヤーの位置から真下に落とす
            Vector3 playerPosition = this.gameObject.transform.position;
            Ray ray = new Ray(playerPosition, -Vector3.up);

            // 空間認識レイヤー用マスク
            int layerMask = 1 << p_SpatialAwarenessLayer;

            // レイキャストのヒット情報を取得する
            //if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                // ヒット位置(高さ)が上限値を超えていなければ高さとして代入する
                p_CurrentHeight = (hitInfo.distance < p_HeightMax) ? hitInfo.distance : p_HeightMax;
            }
            else
            {
                // ヒット情報が得られなければ上限値を高さとして代入する
                p_CurrentHeight = p_HeightMax;
            }
        }
    }
}
