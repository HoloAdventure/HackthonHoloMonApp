using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

namespace HoloMonApp.Content.Character.WorldItem.Model.Common
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class WorldItemCommonAPI : MonoBehaviour, WorldItemCommonInterface
    {
        [SerializeField]
        WorldItemCommonPhysicsController p_WorldItemCommonPhysicsController;

        void Awake()
        {
            p_WorldItemCommonPhysicsController = new WorldItemCommonPhysicsController(
                this.GetComponent<Collider>(), this.GetComponent<Rigidbody>());
        }

        void Start()
        {
        }

        void Update()
        {
        }

        void OnEnable()
        {
        }

        void OnDisable()
        {
        }

        /// <summary>
        /// アイテムをリセットする
        /// </summary>
        public void ResetObject()
        {
        }

        /// <summary>
        /// アイテムを表示する
        /// </summary>

        public void ShowItem()
        {
        }

        /// <summary>
        /// アイテムを隠す
        /// </summary>

        public void HideItem()
        {
        }

        /// <summary>
        /// 物理演算を有効化する
        /// </summary>

        public void EnablePhysics()
        {
        }

        /// <summary>
        /// 物理演算を無効化する
        /// </summary>

        public void DisablePhysics()
        {
        }


        /// <summary>
        /// 座標を設定する
        /// </summary>
        public void SetPosition(Vector3 a_Position)
        {
        }

        /// <summary>
        /// 回転を設定する
        /// </summary>
        public void SetRotation(Quaternion a_Rotation)
        {
        }

        /// <summary>
        /// スケールを設定する
        /// </summary>
        public void SetScale(Vector3 a_Scale)
        {
        }

        /// <summary>
        /// スケール比率を指定して即座に変形する
        /// </summary>
        public void SetScaleRatio(float a_Ratio)
        {
        }

        /// <summary>
        /// 慣性速度を設定する
        /// </summary>
        public void AddForce(Vector3 a_Force)
        {
        }

        /// <summary>
        /// スケール比率を指定して変形する
        /// </summary>
        public void MorphScaleRatio(float a_Ratio)
        {
        }

        /// <summary>
        /// アイテムを消失させる
        /// </summary>
        public void DisappearItem(float a_LearpTime)
        {
        }
    }
}