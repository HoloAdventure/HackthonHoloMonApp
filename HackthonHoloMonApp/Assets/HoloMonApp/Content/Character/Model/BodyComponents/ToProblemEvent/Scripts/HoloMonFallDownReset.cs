using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToProblemEvent
{
    public class HoloMonFallDownReset : MonoBehaviour
    {
        [SerializeField, Tooltip("ボディトランスフォームの参照")]
        private HoloMonTransformsBodyData p_HoloMonTransformsBodyData;

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
            if (ResetYHeight > p_HoloMonTransformsBodyData.Root.position.y)
            {
                // 敷居値以下の高度になった場合リセット処理呼び出し
                ResetEvents.Invoke();
            }
        }
    }
}