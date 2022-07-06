using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace HoloMonApp.Content.Character.WorldItem.Model.Common
{
    public class WorldItemCommonComponentController
    {
        /// <summary>
        /// アイテムコンポーネント
        /// </summary>
        private Component p_ItemComponent;

        /// <summary>
        /// アイテムトランスフォーム
        /// </summary>
        private Transform p_ItemTransform => p_ItemComponent.transform;

        /// <summary>
        /// アイテムゲームオブジェクト
        /// </summary>
        private GameObject p_ItemGameObject => p_ItemComponent.gameObject;

        /// <summary>
        /// デフォルト座標
        /// </summary>
        [SerializeField, Tooltip("デフォルト座標")]
        private Vector3 p_DefaultLocalPosition;

        /// <summary>
        /// デフォルト回転
        /// </summary>
        [SerializeField, Tooltip("デフォルト回転")]
        private Quaternion p_DefaultLocalRotation;

        /// <summary>
        /// デフォルトサイズ
        /// </summary>
        [SerializeField, Tooltip("デフォルトサイズ")]
        private Vector3 p_DefaultLocalScale;


        [SerializeField, Tooltip("表示中フラグ")]
        private bool p_IsShow;


        /// <summary>
        /// モーフトリガー
        /// </summary>
        private IDisposable p_MorphLerpTrigger;

        /// <summary>
        /// 消失トリガー
        /// </summary>
        private IDisposable p_DisappearLerpTrigger;



        public WorldItemCommonComponentController(Component a_ItemComponent)
        {
            // 参照コンポーネントを保存
            p_ItemComponent = a_ItemComponent;

            // デフォルト座標を保存
            p_DefaultLocalPosition = p_ItemTransform.localPosition;

            // デフォルト回転を保存
            p_DefaultLocalRotation = p_ItemTransform.localRotation;

            // デフォルトスケールを保存
            p_DefaultLocalScale = p_ItemTransform.localScale;
        }

        /// <summary>
        /// アイテムをリセットする
        /// </summary>
        public void ResetObject()
        {
            // デフォルト位置に戻す
            p_ItemTransform.localPosition = p_DefaultLocalPosition;
            p_ItemTransform.localRotation = p_DefaultLocalRotation;

            // アイテムを表示する
            ShowItem();
        }

        /// <summary>
        /// アイテムを表示する
        /// </summary>

        public void ShowItem()
        {
            // オブジェクトを有効化する
            if (!p_ItemGameObject.activeSelf) p_ItemGameObject.SetActive(true);

            // 表示中フラグをONにする
            p_IsShow = true;
        }

        /// <summary>
        /// アイテムを隠す
        /// </summary>

        public void HideItem()
        {
            // オブジェクトを無効化する
            p_ItemGameObject.SetActive(false);

            // 表示中フラグをOFFにする
            p_IsShow = false;
        }

        /// <summary>
        /// 座標を設定する
        /// </summary>
        public void SetPosition(Vector3 a_Position)
        {
            // 座標を設定する
            p_ItemTransform.position = a_Position;
        }

        /// <summary>
        /// 回転を設定する
        /// </summary>
        public void SetRotation(Quaternion a_Rotation)
        {
            // 回転を設定する
            p_ItemTransform.rotation = a_Rotation;
        }

        /// <summary>
        /// スケールを設定する
        /// </summary>
        public void SetScale(Vector3 a_Scale)
        {
            // スケールを設定する
            p_ItemTransform.localScale = a_Scale;
        }

        /// <summary>
        /// スケール比率を指定して即座に変形する
        /// </summary>
        public void SetScaleRatio(float a_Ratio)
        {
            // スケールを設定する
            p_ItemTransform.localScale = p_DefaultLocalScale * a_Ratio;
        }

        /// <summary>
        /// スケール比率を指定して徐々に変形する
        /// </summary>
        public void SmoothScaleRatio(float a_Ratio)
        {
            // 表示中フラグがOFFの場合は処理しない
            if (!p_IsShow) return;

            // スムーズにトランスフォームを移動させる
            // 分割数
            int lerpTime = 30;

            // 反映前トランスフォーム
            Vector3 beforeScale = p_ItemTransform.localScale;

            // 反映後トランスフォーム
            Vector3 afterScale = p_DefaultLocalScale * a_Ratio;

            // トリガーを設定済みの場合は一旦破棄する
            p_MorphLerpTrigger?.Dispose();

            // 0.025秒ごとにトランスフォームを徐々に変化させる
            p_MorphLerpTrigger = Observable
                .Interval(TimeSpan.FromSeconds(0.025f))
                .Take(lerpTime)
                .SubscribeOnMainThread()
                .Subscribe(
                x =>
                {
                    // Lerpを使って徐々にトランスフォームを変化させる
                    float current = (float)(1.0 / lerpTime) * x;
                    Vector3 settingScale = Vector3.Lerp(beforeScale, afterScale, current);

                    p_ItemTransform.localScale = settingScale;
                },
                () =>
                {
                    // 完了時は最終的なトランスフォーム値を設定する
                    p_ItemTransform.localScale = afterScale;
                })
                .AddTo(p_ItemComponent);
        }

        /// <summary>
        /// アイテムを徐々に消失させる
        /// </summary>
        public void SmoothDisappearItem(float a_LearpTime)
        {
            // 表示中フラグがOFFの場合は処理しない
            if (!p_IsShow) return;

            // 表示中フラグをOFFにする
            p_IsShow = false;

            // スムーズにトランスフォームを移動させる
            // 分割数
            int lerpTime = (int)(a_LearpTime / 0.025f);

            // 反映前トランスフォーム
            Vector3 beforeScale = p_ItemTransform.localScale;

            // 反映後トランスフォーム
            Vector3 afterScale = Vector3.zero;

            // トリガーを設定済みの場合は一旦破棄する
            p_DisappearLerpTrigger?.Dispose();

            // 0.025秒ごとにトランスフォームを徐々に変化させる
            p_DisappearLerpTrigger = Observable
                .Interval(TimeSpan.FromSeconds(0.025f))
                .Take(lerpTime)
                .SubscribeOnMainThread()
                .Subscribe(
                x =>
                {
                    // Lerpを使って徐々にトランスフォームを変化させる
                    float current = (float)(1.0 / lerpTime) * x;
                    Vector3 settingScale = Vector3.Lerp(beforeScale, afterScale, current);

                    p_ItemTransform.localScale = settingScale;
                },
                () =>
                {
                    // 完了時はオブジェクトを非表示にしてトランスフォームを戻す
                    p_ItemTransform.localScale = beforeScale;

                    // オブジェクトを非表示にする
                    HideItem();
                })
                .AddTo(p_ItemComponent);
        }
    }
}