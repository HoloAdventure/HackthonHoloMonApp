using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace HoloMonApp.Content.ItemSpace
{
    public class ItemAttentionController : MonoBehaviour
    {
        /// <summary>
        /// コントロール対象
        /// </summary>
        [SerializeField, Tooltip("コントロール対象")]
        private GameObject p_ControlObject;

        /// <summary>
        /// 開始位置
        /// </summary>
        [SerializeField, Tooltip("開始位置")]
        private Transform p_StartPosition;

        /// <summary>
        /// デフォルト座標
        /// </summary>
        [SerializeField, Tooltip("デフォルト座標")]
        private Vector3 p_DefaultWorldPosition;

        /// <summary>
        /// デフォルトスケール
        /// </summary>
        [SerializeField, Tooltip("デフォルトスケール")]
        private Vector3 p_DefaultLocalScale;

        /// <summary>
        /// 注視モード
        /// </summary>
        [SerializeField, Tooltip("注視モード")]
        private AttentionMode p_AttentionMode;

        /// <summary>
        /// 注視オブジェクトの参照
        /// </summary>
        [SerializeField, Tooltip("注視オブジェクトの参照")]
        private GameObject p_AttentionObject;

        /// <summary>
        /// モデルオブジェクトの参照
        /// </summary>
        [SerializeField, Tooltip("モデルオブジェクトの参照")]
        private GameObject p_ModelObject;


        /// <summary>
        /// Disable切り替え時の実行イベント
        /// </summary>
        [SerializeField, Tooltip("Disable切り替え時の実行イベント")]
        private UnityEvent p_DisableEvent;

        /// <summary>
        /// Show切り替え時の実行イベント
        /// </summary>
        [SerializeField, Tooltip("Show切り替え時の実行イベント")]
        private UnityEvent p_ShowEvent;

        /// <summary>
        /// Hide切り替え時の実行イベント
        /// </summary>
        [SerializeField, Tooltip("Hide切り替え時の実行イベント")]
        private UnityEvent p_HideEvent;


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // デフォルト座標を保存する
            p_DefaultWorldPosition = p_ControlObject.transform.position;

            // デフォルトスケールを保存する
            p_DefaultLocalScale = p_ControlObject.transform.localScale;

            // 注視/モデルオブジェクトを無効化する
            p_AttentionObject.SetActive(false);
            p_ModelObject.SetActive(false);

            // 状態を設定する
            p_AttentionMode = AttentionMode.Disable;
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
            // デフォルト位置に戻す
            p_ControlObject.transform.position = p_DefaultWorldPosition;

            // デフォルトスケールに戻す
            p_ControlObject.transform.localScale = p_DefaultLocalScale;
        }

        /// <summary>
        /// オブジェクト開始
        /// </summary>
        public void StartObject()
        {
            // 開始位置にオブジェクトを移動する
            p_ControlObject.transform.position = p_StartPosition.position + (p_StartPosition.up * 0.1f);

            // デフォルトスケールに戻す
            p_ControlObject.transform.localScale = p_DefaultLocalScale;
        }

        /// <summary>
        /// モードをスイッチする
        /// </summary>
        public void SwitchMode()
        {
            // 現在のモードに合わせて次のモードに切り替える
            switch(p_AttentionMode)
            {
                case AttentionMode.Disable:
                    // Disableの場合は次はShowに切り替える
                    SwitchShowMode();
                    break;
                case AttentionMode.Show:
                    // Showの場合は次はHideに切り替える
                    SwitchHideMode();
                    break;
                case AttentionMode.Hide:
                    // Hideの場合は次はDisableに切り替える
                    SwitchDisableMode();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// アクティブ状態をスイッチする
        /// </summary>
        public void SwitchActive()
        {
            // 現在のモードに合わせて次のモードに切り替える
            switch (p_AttentionMode)
            {
                case AttentionMode.Disable:
                    // Disableの場合は次はShowに切り替える
                    SwitchShowMode();
                    // オブジェクトを開始位置にリセットする
                    StartObject();
                    break;
                case AttentionMode.Show:
                case AttentionMode.Hide:
                    // ShowまたはHideの場合は次はDisableに切り替える
                    SwitchDisableMode();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 表示状態をスイッチする
        /// </summary>
        public void SwitchVisible()
        {
            // 現在のモードに合わせて次のモードに切り替える
            switch (p_AttentionMode)
            {
                case AttentionMode.Disable:
                case AttentionMode.Hide:
                    // DisableまたはHideの場合は次はShowに切り替える
                    SwitchShowMode();
                    break;
                case AttentionMode.Show:
                    // Showの場合は次はHideに切り替える
                    SwitchHideMode();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Disableモードに切り替える
        /// </summary>
        private void SwitchDisableMode()
        {
            // 注視/モデルオブジェクトを無効化する
            p_AttentionObject.SetActive(false);
            p_ModelObject.SetActive(false);

            // イベントを実行する
            p_DisableEvent.Invoke();

            // 状態を設定する
            p_AttentionMode = AttentionMode.Disable;
        }

        /// <summary>
        /// Showモードに切り替える
        /// </summary>
        private void SwitchShowMode()
        {

            // 注視/モデルオブジェクトを有効化する
            p_AttentionObject.SetActive(true);
            p_ModelObject.SetActive(true);

            // イベントを実行する
            p_ShowEvent.Invoke();

            // 状態を設定する
            p_AttentionMode = AttentionMode.Show;
        }

        /// <summary>
        /// Hideモードに切り替える
        /// </summary>
        private void SwitchHideMode()
        {
            // 注視オブジェクトを有効化し、
            // モデルオブジェクトを無効化する
            p_AttentionObject.SetActive(true);
            p_ModelObject.SetActive(false);

            // イベントを実行する
            p_HideEvent.Invoke();

            // 状態を設定する
            p_AttentionMode = AttentionMode.Hide;
        }
    }
}