using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace HoloMonApp.Content.Character.WorldItem.Model.Common
{
    [Serializable]
    public class WorldItemCommonController
    {
        private WorldItemCommonComponentController p_WorldItemCommonComponentController;

        private WorldItemCommonPhysicsController p_WorldItemCommonPhysicsController;


        public WorldItemCommonController(
            Component a_ItemComponent,
            Collider a_ItemCollider,
            Rigidbody a_ItemRigidbody)
        {
            p_WorldItemCommonComponentController = new WorldItemCommonComponentController(a_ItemComponent);
            p_WorldItemCommonPhysicsController = new WorldItemCommonPhysicsController(a_ItemCollider, a_ItemRigidbody);
        }

        /// <summary>
        /// アイテムをリセットする
        /// </summary>
        public void ResetObject()
        {
            // アイテムをリセットする
            p_WorldItemCommonComponentController.ResetObject();

            // 物理演算を有効化する
            p_WorldItemCommonPhysicsController.EnablePhysics();
        }

        /// <summary>
        /// アイテムを表示する
        /// </summary>

        public void ShowItem()
        {
            // アイテムを表示する
            p_WorldItemCommonComponentController.ShowItem();

            // 慣性をリセットする
            p_WorldItemCommonPhysicsController.ResetVelocity();
        }

        /// <summary>
        /// アイテムを隠す
        /// </summary>

        public void HideItem()
        {
            // アイテムを隠す
            p_WorldItemCommonComponentController.HideItem();

            // 慣性をリセットする
            p_WorldItemCommonPhysicsController.ResetVelocity();
        }

        /// <summary>
        /// 物理演算を有効化する
        /// </summary>

        public void EnablePhysics()
        {
            // 物理演算を有効化する
            p_WorldItemCommonPhysicsController.EnablePhysics();
        }

        /// <summary>
        /// 物理演算を無効化する
        /// </summary>

        public void DisablePhysics()
        {
            // 物理演算を無効化する
            p_WorldItemCommonPhysicsController.DisablePhysics();
        }


        /// <summary>
        /// 座標を設定する
        /// </summary>
        public void SetPosition(Vector3 a_Position)
        {
            // 座標を設定する
            p_WorldItemCommonComponentController.SetPosition(a_Position);
        }

        /// <summary>
        /// 回転を設定する
        /// </summary>
        public void SetRotation(Quaternion a_Rotation)
        {
            // 回転を設定する
            p_WorldItemCommonComponentController.SetRotation(a_Rotation);
        }

        /// <summary>
        /// スケールを設定する
        /// </summary>
        public void SetScale(Vector3 a_Scale)
        {
            // スケールを設定する
            p_WorldItemCommonComponentController.SetScale(a_Scale);
        }

        /// <summary>
        /// スケール比率を指定して変形する
        /// </summary>
        public void SetScaleRatio(float a_Ratio)
        {
            // スケール比率を指定して変形する
            p_WorldItemCommonComponentController.SetScaleRatio(a_Ratio);
        }

        /// <summary>
        /// 慣性速度を設定する
        /// </summary>
        public void AddForce(Vector3 a_Force)
        {
            // 慣性速度を設定する
            p_WorldItemCommonPhysicsController.AddForce(a_Force);
        }

        /// <summary>
        /// スケール比率を指定して徐々に変形する
        /// </summary>
        public void SmoothScaleRatio(float a_Ratio)
        {
            // スケール比率を指定して徐々に変形する
            p_WorldItemCommonComponentController.SmoothScaleRatio(a_Ratio);
        }

        /// <summary>
        /// アイテムを徐々に消失させる
        /// </summary>
        public void SmoothDisappearItem(float a_LearpTime)
        {
            // アイテムを徐々に消失させる
            p_WorldItemCommonComponentController.SmoothDisappearItem(a_LearpTime);
        }
    }
}