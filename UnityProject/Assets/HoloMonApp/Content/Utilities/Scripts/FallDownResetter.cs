using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HoloMonApp.Content.UtilitiesSpace
{
    public class FallDownResetter : MonoBehaviour
    {
        [SerializeField, Tooltip("落下時リセット呼び出し処理")]
        private UnityEvent ResetEvents;

        [SerializeField, Tooltip("リセット判定高度(Y)")]
        private float ResetYHeight = -30.0f;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            if (ResetYHeight > this.transform.position.y)
            {
                // 敷居値以下の高度になった場合リセット処理呼び出し
                ResetEvents.Invoke();
            }
        }
    }
}