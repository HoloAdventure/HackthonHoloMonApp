using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// Subscribeを行うため、UniRxを参照
using UniRx;

using HoloMonApp.Content.Character.Data.Conditions.Body;
using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToTransformUtility
{
    public class HoloMonTransformUtility : MonoBehaviour
    {
        /// <summary>
        /// データの参照
        /// </summary>
        [SerializeField, Tooltip("ボディトランスフォームの参照")]
        private HoloMonTransformsBodyData p_HoloMonTransformsBodyData;

        [SerializeField]
        private HoloMonConditionBodyData p_HoloMonConditionBodyData;

        // 現在のボディY軸ローカルスケールを実スケールとして参照する
        private float p_Scale => p_HoloMonTransformsBodyData.Root.localScale.y;


        /// <summary>
        /// 変化用のトリガー
        /// </summary>
        private IDisposable p_LerpTrigger;
        

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
        }

        /// <summary>
        /// 位置の設定を行う
        /// </summary>
        /// <param name="a_WorldPosition"></param>
        public void SetPosition(Vector3 a_WorldPosition)
        {
            p_HoloMonTransformsBodyData.Root.position = a_WorldPosition;
        }

        /// <summary>
        /// 回転の設定を行う
        /// </summary>
        /// <param name="a_WorldRotation"></param>
        public void SetRotation(Quaternion a_WorldRotation)
        {
            p_HoloMonTransformsBodyData.Root.rotation = a_WorldRotation;
        }

        /// <summary>
        /// スケールの設定を行う
        /// </summary>
        public void SetScaleSmooth(float a_ApplyScale)
        {
            ApplyScaleSmooth(a_ApplyScale);
        }

        #region "private"
        /// <summary>
        /// 指定のスケールをスムーズに反映する
        /// </summary>
        /// <param name="a_ApplyScale"></param>
        private void ApplyScaleSmooth(float a_ApplyScale)
        {
            // 分割フレーム数
            int lerpTime = 10;

            // 反映後スケール
            float afterScale = a_ApplyScale;

            // 反映前スケール
            float beforeScale = p_Scale;

            // トリガーを設定済みの場合は一旦破棄する
            p_LerpTrigger?.Dispose();

            // 0.05秒ごとにスケールを徐々に拡大する
            p_LerpTrigger = Observable
                .Interval(TimeSpan.FromSeconds(0.05f))
                .Take(lerpTime)
                .SubscribeOnMainThread()
                .Subscribe(
                x =>
                {
                    // Lerpを使って徐々にスケールを変化させる
                    float current = (float)(1.0 / lerpTime) * x;
                    float settingHeight = Mathf.Lerp(beforeScale, afterScale, current);
                    UpdateScale(settingHeight);
                },
                () =>
                {
                    // 完了時は最終的なスケール値を設定する
                    UpdateScale(a_ApplyScale);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 指定のスケール値に合わせてトランスフォーム設定を行う
        /// </summary>
        private void UpdateScale(float a_Scale)
        {
            // 最小値制限
            if (a_Scale < 0.1f) return;

            // スケールを設定する
            p_HoloMonTransformsBodyData.Root.localScale = new Vector3(a_Scale, a_Scale, a_Scale);
        }
        #endregion
    }
}