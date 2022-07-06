using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UniRx;

namespace HoloMonApp.Content.ItemSpace
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class ItemCommonController : MonoBehaviour, ItemCommonInterface
    {
        /// <summary>
        /// アイテムコライダー
        /// </summary>
        private Collider p_ItemCollider
            => p_ItemColliderInstance ?? (p_ItemColliderInstance = p_ItemComponent.GetComponent<Collider>());
        private Collider p_ItemColliderInstance;

        /// <summary>
        /// アイテムリジッドボディ
        /// </summary>
        private Rigidbody p_ItemRigidbody
            => p_ItemRigidbodyInstance ?? (p_ItemRigidbodyInstance = p_ItemComponent.GetComponent<Rigidbody>());
        private Rigidbody p_ItemRigidbodyInstance;

        /// <summary>
        /// アイテムコンポーネント
        /// </summary>
        private Component p_ItemComponent => this;

        /// <summary>
        /// アイテムゲームオブジェクト
        /// </summary>
        private GameObject p_ItemGameObject => this.gameObject;

        /// <summary>
        /// アイテムトランスフォーム
        /// </summary>
        private Transform p_ItemTransform => this.transform;

        private bool p_IsShow => p_ItemGameObject.activeSelf;


        /// <summary>
        /// モーフトリガー
        /// </summary>
        private IDisposable p_MorphLerpTrigger;

        /// <summary>
        /// 消失トリガー
        /// </summary>
        private IDisposable p_DisappearLerpTrigger;


        void OnEnable()
        {
        }

        void OnDisable()
        {
            // アイテムをリセットする
            ResetObject();
        }

        void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// アイテムをリセットする
        /// </summary>
        public void ResetObject()
        {
            // 物理演算を有効化する
            EnablePhysics();
        }

        /// <summary>
        /// アイテムを表示する
        /// </summary>

        public void ShowItem()
        {
            // オブジェクトを有効化する
            if (!p_ItemGameObject.activeSelf) p_ItemGameObject.SetActive(true);

            // 慣性をリセットする
            p_ItemRigidbody.velocity = Vector3.zero;
        }

        /// <summary>
        /// アイテムを隠す
        /// </summary>

        public void HideItem()
        {
            // 慣性をリセットする
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // オブジェクトを無効化する
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 物理演算を有効化する
        /// </summary>

        public void EnablePhysics()
        {
            // 重力を有効化する
            this.GetComponent<Rigidbody>().useGravity = true;

            // アタリ判定を有効化する
            this.GetComponent<Collider>().enabled = true;
        }

        /// <summary>
        /// 物理演算を無効化する
        /// </summary>

        public void DisablePhysics()
        {
            // 一旦、慣性をリセットする
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            // 重力を切る
            this.GetComponent<Rigidbody>().useGravity = false;

            // アタリ判定を無効化する
            this.GetComponent<Collider>().enabled = false;
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
            p_ItemTransform.localScale = Vector3.one * a_Ratio;
        }

        /// <summary>
        /// 慣性速度を設定する
        /// </summary>
        public void AddForce(Vector3 a_Force)
        {
            // 慣性を設定する
            p_ItemRigidbody.AddForce(a_Force);
        }

        /// <summary>
        /// スケール比率を指定して変形する
        /// </summary>
        public void MorphScaleRatio(float a_Ratio)
        {
            // 表示中フラグがOFFの場合は処理しない
            if (!p_IsShow) return;

            // スムーズにトランスフォームを移動させる
            // 分割数
            int lerpTime = 30;

            // 反映前トランスフォーム
            Vector3 beforeScale = p_ItemTransform.localScale;

            // 反映後トランスフォーム
            Vector3 afterScale = Vector3.one * a_Ratio;

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
                .AddTo(this);
        }

        /// <summary>
        /// アイテムを消失させる
        /// </summary>
        public void DisappearItem(float a_LearpTime)
        {
            // 表示中フラグがOFFの場合は処理しない
            if (!p_IsShow) return;

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
                .AddTo(this);
        }
    }
}