using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Access;

namespace HoloMonApp.Content.Character.Model.WorldItems.Stand
{
    public class HoloMonStandController : MonoBehaviour
    {
        /// <summary>
        /// APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonAccessAPI p_AccessAPI;

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

            // 身長設定変更時の処理を設定する
            p_AccessAPI.View.ConditionsBodyAPI.ReactivePropertyStatus
                .ObserveOnMainThread()
                .Subscribe(status => {
                    // 現在のサイズの変化比率を取得する
                    float currentScaleRatio = status.GetDefaultBodyHeightRatio;
                    // サイズの変化比率が異なれば設定を行う
                    if (p_ScaleRatio != currentScaleRatio)
                    {
                        ApplySizeCondition(currentScaleRatio);
                    }
                })
                .AddTo(this);
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// オブジェクトリセット（トランスフォーム指定）
        /// </summary>
        public void ResetObject(Transform a_ResetPosition)
        {
            ResetPosition(a_ResetPosition.position);
        }

        /// <summary>
        /// オブジェクトリセット
        /// </summary>
        public void ResetPosition(Vector3 a_ResetWorldPosition)
        {
            // Showモードに切り替える
            SwitchShowMode();

            // スポーン位置位置に戻す
            p_ControlObject.transform.position = a_ResetWorldPosition + p_LocalOffsetPosition;
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

        /// <summary>
        /// 現在のホロモンの身長に合わせて大きさを反映する
        /// </summary>
        /// <param name="a_ScaleRatio"></param>
        private void ApplySizeCondition(float a_ScaleRatio)
        {
            // 現在のホロモンサイズに合わせた大きさを算出して設定する
            p_ControlObject.transform.localScale = p_DefaultLocalScale * a_ScaleRatio;

            // 設定中のサイズ比率を記録する
            p_ScaleRatio = a_ScaleRatio;
        }
    }
}