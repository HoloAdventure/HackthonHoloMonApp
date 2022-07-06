using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.ItemSpace
{
    public class ShowerWaterHitActiveController : MonoBehaviour
    {
        [SerializeField, Tooltip("自動タイムアウト時刻")]
        private float p_InactiveTimeOut;
        private float p_ActiveTimeElapsed;

        [SerializeField, Tooltip("アクティブコントロール対象")]
        private GameObject p_TargetObject;

        void Start()
        {
            // 初期状態は非アクティブにする
            p_TargetObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            p_ActiveTimeElapsed += Time.deltaTime;

            if (p_ActiveTimeElapsed >= p_InactiveTimeOut)
            {
                // タイムアウト時間を超えた場合オブジェクトを無効化する
                if (p_TargetObject.activeSelf)
                {
                    p_TargetObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// オブジェクトをアクティブにする
        /// </summary>
        public void ActiveObject()
        {
            p_TargetObject.SetActive(true);
            p_ActiveTimeElapsed = 0.0f;
        }

        /// <summary>
        /// ワールド座標の設定
        /// </summary>
        /// <param name="a_WorldPosition"></param>
        public void SetWorldPosition(Vector3 a_WorldPosition)
        {
            p_TargetObject.transform.position = a_WorldPosition;
        }
    }
}