using HoloMonApp.Content.ItemSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using System;

namespace HoloMonApp.Content.Character.Model.ObjectInteraction.HoldItem
{
    public class HoloMonItemHolder : MonoBehaviour
    {
        /// <summary>
        /// ホールド中オブジェクト
        /// </summary>
        [SerializeField, Tooltip("ホールドオブジェクト")]
        private GameObject p_HoldObject;

        [SerializeField, Tooltip("ホールド中")]
        private bool p_IsHolding;

        /// <summary>
        /// ホールドトリガー
        /// </summary>
        private IDisposable p_HoldLerpTrigger;

        /// <summary>
        /// ホールドトランスフォーム
        /// </summary>
        private Transform p_HoldPositionTransform => this.transform;


        // Update is called once per frame
        void Update()
        {
            // ホールド時はトランスフォームを追従する
            if (p_IsHolding)
            {
                if (p_HoldObject != null)
                {
                    // 追跡位置を指定する
                    p_HoldObject.transform.position = p_HoldPositionTransform.position;
                    p_HoldObject.transform.rotation = p_HoldPositionTransform.rotation;
                }
            }
        }

        public bool SetHoldItem(GameObject a_GameObject)
        {
            // コライダーを取得する
            Collider itemCollider = a_GameObject.GetComponent<Collider>();

            // アイテムコントローラを取得する
            ItemCommonController itemCommonController = a_GameObject.GetComponent<ItemCommonController>();

            // コライダーとアイテムコントローラが取得できたことを確認する
            if ((itemCollider == null) || (itemCommonController == null))
            {
                return false;
            }

            // アイテムの物理演算を無効化する
            itemCommonController.DisablePhysics();

            // ホールド中アイテムの参照を保存する
            p_HoldObject = a_GameObject;

            // スムーズにトランスフォームを移動させる
            // 分割フレーム数
            int lerpTime = 20;

            // 反映前トランスフォーム
            Vector3 beforePosition = p_HoldObject.transform.position;
            Quaternion beforeRotation = p_HoldObject.transform.rotation;

            // トリガーを設定済みの場合は一旦破棄する
            p_HoldLerpTrigger?.Dispose();

            // 1フレームごとにトランスフォームを徐々に変化させる
            p_HoldLerpTrigger = Observable
                .IntervalFrame(1)
                .Take(lerpTime)
                .SubscribeOnMainThread()
                .Subscribe(
                x =>
                {
                    // 保持位置を常に更新する
                    Vector3 afterPosition = p_HoldPositionTransform.position;
                    Quaternion afterRotation = p_HoldPositionTransform.rotation;

                    // Lerpを使って徐々にトランスフォームを変化させる
                    float current = (float)(1.0 / lerpTime) * x;
                    Vector3 settingPosition = Vector3.Lerp(beforePosition, afterPosition, current);
                    Quaternion settingRotation = Quaternion.Lerp(beforeRotation, afterRotation, current);

                    p_HoldObject.transform.position = settingPosition;
                    p_HoldObject.transform.rotation = settingRotation;
                },
                () =>
                {
                    // 完了時は最終的なトランスフォーム値を設定する
                    p_HoldObject.transform.position = p_HoldPositionTransform.position;
                    p_HoldObject.transform.rotation = p_HoldPositionTransform.rotation;

                    // ホールド設定を行う
                    p_IsHolding = true;
                })
                .AddTo(this);

            return true;
        }

        public bool ReleaseHoldItem()
        {
            // 保持オブジェクトがない場合は処理しない
            if (p_HoldObject == null)
            {
                return false;
            }

            // コライダーを取得する
            Collider itemCollider = p_HoldObject.GetComponent<Collider>();

            // アイテムコントローラを取得する
            ItemCommonController itemCommonController = p_HoldObject.GetComponent<ItemCommonController>();

            // コライダーとアイテムコントローラが取得できたことを確認する
            if ((itemCollider == null) || (itemCommonController == null))
            {
                return false;
            }

            // アイテムの物理演算を有効化する
            itemCommonController.EnablePhysics();

            // 投擲トランスフォームの正面方向を取得する
            Vector3 forward = p_HoldObject.transform.forward * 100.0f;

            // 慣性を設定する
            itemCommonController.AddForce(forward);

            // ホールド中アイテムの参照を破棄する
            p_HoldObject = null;

            // ホールド設定を解除する
            p_IsHolding = false;

            // トリガーを設定済みの場合は破棄する
            p_HoldLerpTrigger?.Dispose();

            return true;
        }

        public bool DisappearHoldItem()
        {
            // 保持オブジェクトがない場合は処理しない
            if (p_HoldObject == null)
            {
                return false;
            }

            // コライダーを取得する
            Collider itemCollider = p_HoldObject.GetComponent<Collider>();

            // アイテムコントローラを取得する
            ItemCommonController itemCommonController = p_HoldObject.GetComponent<ItemCommonController>();

            // コライダーとアイテムコントローラが取得できたことを確認する
            if ((itemCollider == null) || (itemCommonController == null))
            {
                return false;
            }

            // ホールド中アイテムの参照を破棄する
            p_HoldObject = null;

            // ホールド設定を解除する
            p_IsHolding = false;

            // トリガーを設定済みの場合は一旦破棄する
            p_HoldLerpTrigger?.Dispose();


            // 食べ物を消滅させる
            itemCommonController.DisappearItem(0.2f);

            return true;
        }
    }
}