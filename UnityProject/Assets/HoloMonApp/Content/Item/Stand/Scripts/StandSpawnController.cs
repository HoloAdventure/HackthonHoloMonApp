using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UniRx;

using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.ItemSpace
{
    public class StandSpawnController : MonoBehaviour
    {
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

        [SerializeField, Tooltip("消失アニメーション中か")]
        private bool p_NowDisappear;

        [SerializeField, Tooltip("ヘルプ参照")]
        private GameObject p_HelpObject;

        /// <summary>
        /// 消失時発生イベント
        /// </summary>
        [SerializeField, Tooltip("消失時発生イベント")]
        private UnityEvent DisappearEvents;

        /// <summary>
        /// 消失トリガー
        /// </summary>
        private IDisposable p_DisappearLerpTrigger;

        /// <summary>
        /// スタンド消失のチェックカウント
        /// </summary>
        private int p_CheckStandOnThreshold = 5;
        private int p_CheckStandOnCount;

        private void OnEnable()
        {
            // チェック変数をリセット
            p_CheckStandOnCount = 0;
            p_NowDisappear = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            // デフォルト座標を保存
            p_DefaultLocalPosition = this.transform.localPosition;

            // デフォルト回転を保存
            p_DefaultLocalRotation = this.transform.localRotation;

            // デフォルトスケールを保存
            p_DefaultLocalScale = this.transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            // 表示状態のときは上に何か乗っているか常にチェックする
            if (!CheckStandOn())
            {
                // 上に物がないときはカウントアップ
                p_CheckStandOnCount++;

                if (p_CheckStandOnCount > p_CheckStandOnThreshold)
                {
                    // 一定フレーム間、上に物がないときはスタンドを消失させる
                    DisappearStand();
                }
            }
            else
            {
                // 物があればカウントをリセットする
                p_CheckStandOnCount = 0;
            }
        }

        /// <summary>
        /// スタンドを消失させる
        /// </summary>
        public void DisappearStand()
        {
            // 消失アニメーション中は処理しない
            if (p_NowDisappear) return;

            p_NowDisappear = true;

            // スムーズにトランスフォームを移動させる
            // 分割フレーム数
            int lerpTime = 5;

            // 反映前トランスフォーム
            Vector3 beforeScale = this.transform.localScale;

            // 反映後トランスフォーム
            Vector3 afterScale = Vector3.zero;

            // トリガーを設定済みの場合は一旦破棄する
            p_DisappearLerpTrigger?.Dispose();

            // 1フレームごとにトランスフォームを徐々に変化させる
            p_DisappearLerpTrigger = Observable
                .IntervalFrame(1)
                .Take(lerpTime)
                .SubscribeOnMainThread()
                .Subscribe(
                x =>
                {
                    // Lerpを使って徐々にトランスフォームを変化させる
                    float current = (float)(1.0 / lerpTime) * x;
                    Vector3 settingScale = Vector3.Lerp(beforeScale, afterScale, current);

                    this.transform.localScale = settingScale;
                },
                () =>
                {
                    // 完了時はオブジェクトを非表示にしてトランスフォームを戻す
                    this.transform.localScale = beforeScale;

                    // オブジェクトを無効化する
                    this.gameObject.SetActive(false);

                    // ヘルプオブジェクトがあれば無効化する
                    // ヘルプオブジェクトは個別に有効化しない限り再表示されない
                    p_HelpObject?.SetActive(false);

                    // 消失時発生イベントを実行する
                    DisappearEvents.Invoke();

                    // 消失アニメーション完了
                    p_NowDisappear = false;
                })
                .AddTo(this);
        }

        /// <summary>
        /// スタンドの上に何か乗っているかチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckStandOn()
        {
            bool onStand = false;

            // レイキャストの結果
            RaycastHit[] raycastHits = new RaycastHit[2];

            // チェック距離を調整する
            float checkDistance = 1.0f;

            // レイキャストでスタンド上方の衝突オブジェクトを検知する
            int hitCount = Physics.RaycastNonAlloc(this.transform.position, Vector3.up, raycastHits, checkDistance);

            if (hitCount > 0)
            {
                // ヒット数が 0 より多いなら障害物があるので何か乗っている
                return true;
            }

            return onStand;
        }
    }
}