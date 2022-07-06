using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace HoloMonApp.Content.Character.WorldItem.Model.Common
{
    public class WorldItemCommonPhysicsController
    {
        /// <summary>
        /// アイテムコライダー
        /// </summary>
        private Collider p_ItemCollider;

        /// <summary>
        /// アイテムリジッドボディ
        /// </summary>
        private Rigidbody p_ItemRigidbody;

        public WorldItemCommonPhysicsController(Collider a_ItemCollider, Rigidbody a_ItemRigidbody)
        {
            p_ItemCollider = a_ItemCollider;
            p_ItemRigidbody = a_ItemRigidbody;
        }

        /// <summary>
        /// 慣性をリセットする
        /// </summary>

        public void ResetVelocity()
        {
            // 慣性をリセットする
            p_ItemRigidbody.velocity = Vector3.zero;
            p_ItemRigidbody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// 物理演算を有効化する
        /// </summary>

        public void EnablePhysics()
        {
            // アタリ判定を有効化する
            p_ItemCollider.enabled = true;

            // 重力を有効化する
            p_ItemRigidbody.useGravity = true;
        }

        /// <summary>
        /// 物理演算を無効化する
        /// </summary>

        public void DisablePhysics()
        {
            // アタリ判定を無効化する
            p_ItemCollider.enabled = false;

            // 重力を切る
            p_ItemRigidbody.useGravity = false;

            // 慣性をリセットする
            p_ItemRigidbody.velocity = Vector3.zero;
            p_ItemRigidbody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// 慣性速度を設定する
        /// </summary>
        public void AddForce(Vector3 a_Force)
        {
            // 慣性を設定する
            p_ItemRigidbody.AddForce(a_Force);
        }
    }
}