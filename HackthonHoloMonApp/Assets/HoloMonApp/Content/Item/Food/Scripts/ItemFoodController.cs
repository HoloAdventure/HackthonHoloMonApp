using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.ItemSpace
{
    public class ItemFoodController : MonoBehaviour
    {
        /// <summary>
        /// モデルコントローラ
        /// </summary>
        [SerializeField, Tooltip("モデルコントローラ")]
        private ItemCommonController p_ModelController;

        /// <summary>
        /// スポーン位置
        /// </summary>
        [SerializeField, Tooltip("スポーン位置")]
        private Transform p_SpawnPosition;

        /// <summary>
        /// デフォルト座標
        /// </summary>
        [SerializeField, Tooltip("デフォルト座標")]
        private Vector3 p_DefaultWorldPosition;

        /// <summary>
        /// デフォルトサイズ
        /// </summary>
        [SerializeField, Tooltip("デフォルトサイズ")]
        private Vector3 p_DefaultLocalScale;

        /// <summary>
        /// 現在のスケール変化比率
        /// </summary>
        [SerializeField, Tooltip("現在のスケール変化比率")]
        private float p_ScaleRatio;

        /// <summary>
        /// 投擲時イベント
        /// </summary>
        [SerializeField, Tooltip("投擲時イベント")]
        private UnityEvent p_ThrowKickEvent;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // モデルコントローラの初期化
            p_ModelController.Initialize();

            // モデルを非表示にする
            p_ModelController.HideItem();

            // デフォルトの変化比率は 1 とする
            p_ScaleRatio = 1.0f;
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// オブジェクトリセット
        /// </summary>
        public void ResetObject()
        {
            // モデルをリセットする
            p_ModelController.ResetObject();

            // モデルを非表示にする
            p_ModelController.HideItem();
        }

        /// <summary>
        /// 投げ渡す
        /// </summary>
        public void ThrowObject()
        {
            // モデルをリセットする
            p_ModelController.ResetObject();

            // モデルを表示する
            p_ModelController.ShowItem();

            // オブジェクトをスポーン位置に配置する
            p_ModelController.SetPosition(p_SpawnPosition.position + (p_SpawnPosition.up * (0.06f * p_ScaleRatio)));

            // 投擲トランスフォームの正面方向を取得する
            Vector3 forward = p_SpawnPosition.forward * 100.0f;

            // 慣性を設定する
            p_ModelController.AddForce(forward);

            // イベント実行
            p_ThrowKickEvent.Invoke();
        }
    }
}