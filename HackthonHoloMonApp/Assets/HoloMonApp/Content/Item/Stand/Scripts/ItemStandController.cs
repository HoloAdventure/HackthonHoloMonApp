using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.ItemSpace
{
    public class ItemStandController : MonoBehaviour
    {
        /// <summary>
        /// コントロール対象
        /// </summary>
        [SerializeField, Tooltip("コントロール対象")]
        private GameObject p_ControlObject;

        /// <summary>
        /// デフォルトスケール
        /// </summary>
        [SerializeField, Tooltip("デフォルトスケール")]
        private Vector3 p_DefaultLocalScale;

        /// <summary>
        /// オフセット座標
        /// </summary>
        [SerializeField, Tooltip("オフセット座標")]
        private Vector3 p_LocalOffsetPosition;

        /// <summary>
        /// 現在のスケール変化比率
        /// </summary>
        [SerializeField, Tooltip("現在のスケール変化比率")]
        private float p_ScaleRatio;

        /// <summary>
        /// スタンドモード
        /// </summary>
        [SerializeField, Tooltip("スタンドモード")]
        private StandMode p_StandMode;

        /// <summary>
        /// スタンドオブジェクトの参照
        /// </summary>
        [SerializeField, Tooltip("スタンドオブジェクトの参照")]
        private GameObject p_StandObject;

        /// <summary>
        /// モデルオブジェクトの参照
        /// </summary>
        [SerializeField, Tooltip("モデルオブジェクトの参照")]
        private GameObject p_ModelObject;


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // デフォルトスケールを保存する
            p_DefaultLocalScale = p_ControlObject.transform.localScale;

            // 状態を設定する
            p_StandMode = StandMode.Show;
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
        public void ResetObject(Transform a_ResetPosition)
        {
            // Showモードに切り替える
            SwitchShowMode();

            // スポーン位置位置に戻す
            p_ControlObject.transform.position = a_ResetPosition.position + p_LocalOffsetPosition;
        }

        /// <summary>
        /// モードをスイッチする
        /// </summary>
        public void SwitchMode()
        {
            // 現在のモードに合わせて次のモードに切り替える
            switch (p_StandMode)
            {
                case StandMode.Disable:
                    // Disableの場合は次はShowに切り替える
                    SwitchShowMode();
                    break;
                case StandMode.Show:
                    // Showの場合は次はHideに切り替える
                    SwitchHideMode();
                    break;
                case StandMode.Hide:
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
            switch (p_StandMode)
            {
                case StandMode.Disable:
                    // Disableの場合は次はShowに切り替える
                    SwitchShowMode();
                    break;
                case StandMode.Show:
                case StandMode.Hide:
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
            switch (p_StandMode)
            {
                case StandMode.Disable:
                case StandMode.Hide:
                    // DisableまたはHideの場合は次はShowに切り替える
                    SwitchShowMode();
                    break;
                case StandMode.Show:
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
        public void SwitchDisableMode()
        {
            // 注視/モデルオブジェクトを無効化する
            p_StandObject.SetActive(false);
            p_ModelObject.SetActive(false);

            // 状態を設定する
            p_StandMode = StandMode.Disable;
        }

        /// <summary>
        /// Showモードに切り替える
        /// </summary>
        public void SwitchShowMode()
        {

            // 注視/モデルオブジェクトを有効化する
            p_StandObject.SetActive(true);
            p_ModelObject.SetActive(true);

            // 状態を設定する
            p_StandMode = StandMode.Show;
        }

        /// <summary>
        /// Hideモードに切り替える
        /// </summary>
        public void SwitchHideMode()
        {
            // 注視オブジェクトを有効化し、
            // モデルオブジェクトを無効化する
            p_StandObject.SetActive(true);
            p_ModelObject.SetActive(false);

            // 状態を設定する
            p_StandMode = StandMode.Hide;
        }
    }
}