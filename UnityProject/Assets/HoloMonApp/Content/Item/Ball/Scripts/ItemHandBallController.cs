using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UniRx;

using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.ItemSpace
{
    public class ItemHandBallController : MonoBehaviour
    {
        [Serializable]
        private class ChildObjectData
        {
            public GameObject Object;
            public Vector3 DefaultLocalPosition;
            public Quaternion DefaultLocalRotation;
        }

        [SerializeField, Tooltip("起動時アクティブフラグ")]
        private bool p_DefaultActiveFlg;

        /// <summary>
        /// コントロール対象
        /// </summary>
        [SerializeField, Tooltip("コントロール対象")]
        private GameObject p_ControlObject;

        /// <summary>
        /// デフォルト座標
        /// </summary>
        [SerializeField, Tooltip("デフォルト座標")]
        private Vector3 p_DefaultWorldPosition;

        /// <summary>
        /// デフォルトサイズ
        /// </summary>
        [SerializeField, Tooltip("デフォルトサイズ")]
        private Vector3 p_DefaultLocalScale;

        /// <summary>
        /// 現在のスケール変化比率
        /// </summary>
        [SerializeField, Tooltip("現在のスケール変化比率")]
        private float p_ScaleRatio;

        /// <summary>
        /// 管理子オブジェクトリスト
        /// </summary>
        [SerializeField, Tooltip("管理子オブジェクトリスト")]
        private List<ChildObjectData> p_ChildObjectDataList;

        /// <summary>
        /// 変化用のトリガー
        /// </summary>
        private IDisposable p_LerpTrigger;


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // デフォルト座標を保存
            p_DefaultWorldPosition = p_ControlObject.transform.position;

            // デフォルトスケールを保存
            p_DefaultLocalScale = p_ControlObject.transform.localScale;

            // 子オブジェクトのデフォルト情報を保存
            SaveChildObjectData();

            // 起動時アクティブ状態を設定
            if (p_DefaultActiveFlg)
            {
                SpawnObject(p_ControlObject.transform);
            }
            else
            {
                DespawnObject(p_ControlObject.transform);
            }
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// スポーン状態を切り替える
        /// </summary>
        public void SpawnSwitch(Transform a_SpawnTransform)
        {
            if (p_ControlObject.activeSelf)
            {
                DespawnObject(a_SpawnTransform);
            }
            else
            {
                SpawnObject(a_SpawnTransform);
            }
        }

        /// <summary>
        /// スポーンする
        /// </summary>
        public void SpawnObject(Transform a_SpawnTransform)
        {
            // 子オブジェクトの状態をデフォルトに戻す
            ApplyChildObjectData();

            // オブジェクトを有効化する
            p_ControlObject.SetActive(true);

            // スポーン位置にオブジェクトを移動する
            p_ControlObject.transform.position = a_SpawnTransform.position;

            // オブジェクトは水平に配置する
            p_ControlObject.transform.eulerAngles = new Vector3(0.0f, a_SpawnTransform.eulerAngles.y, 0.0f);

            // 子オブジェクトのアクティブを確認して有効化する
            ActiveChildObjectData();
        }

        /// <summary>
        /// デスポーンする
        /// </summary>
        public void DespawnObject(Transform a_SpawnTransform)
        {
            // 子オブジェクトの状態をデフォルトに戻す
            ApplyChildObjectData();

            // デフォルト位置に戻す
            p_ControlObject.transform.position = a_SpawnTransform.position;

            // オブジェクトを無効化する
            p_ControlObject.SetActive(false);
        }

        /// <summary>
        /// 子オブジェクトのデータを保存する
        /// </summary>
        private void SaveChildObjectData()
        {
            foreach (ChildObjectData objectData in p_ChildObjectDataList)
            {
                objectData.DefaultLocalPosition = objectData.Object.transform.localPosition;
                objectData.DefaultLocalRotation = objectData.Object.transform.localRotation;
            }
        }

        /// <summary>
        /// 子オブジェクトのデータを再反映する
        /// </summary>
        private void ApplyChildObjectData()
        {
            foreach (ChildObjectData objectData in p_ChildObjectDataList)
            {
                // オブジェクトに慣性コンポーネントがある場合は慣性を無効化しておく
                Rigidbody rigid = objectData.Object.GetComponent<Rigidbody>();
                if (rigid != null) rigid.velocity = Vector3.zero;

                objectData.Object.transform.localPosition = objectData.DefaultLocalPosition;
                objectData.Object.transform.localRotation = objectData.DefaultLocalRotation;
            }
        }

        /// <summary>
        /// 子オブジェクトのアクティブを確認して有効化する
        /// </summary>
        private void ActiveChildObjectData()
        {
            foreach (ChildObjectData objectData in p_ChildObjectDataList)
            {
                if (!objectData.Object.activeSelf) objectData.Object.SetActive(true);
            }
        }
    }
}