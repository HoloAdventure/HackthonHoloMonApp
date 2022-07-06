using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using UniRx;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel
{
    public class EmotionUIController : MonoBehaviour
    {
        /// <summary>
        /// 完了時イベント
        /// </summary>
        public Action CompletedEvent;

        [SerializeField, Tooltip("追跡トランスフォーム")]
        private Transform p_TrackingTransform;

        [SerializeField, Tooltip("マークアニメーションの参照")]
        private EmotionMarkAnimation p_EmotionMarkAnimation;

        [SerializeField, Tooltip("マーク切り替えの参照")]
        private EmotionMarkSwitch p_EmotionMarkSwitch;

        // 実行アニメーション定義
        [Serializable]
        private enum EnumAnimation
        {
            Nothing,
            Exclamation,
            Question,
        }

        [SerializeField, Tooltip("アクション実行中フラグ")]
        private bool p_ActionFlg;

        [SerializeField, Tooltip("終了検知フラグ")]
        private bool p_CompleteFlg;

        [SerializeField, Tooltip("実行アニメーション設定")]
        private EnumAnimation p_Animation;

        // Start is called before the first frame update
        void Start()
        {
            // イベント設定を行う
            p_EmotionMarkAnimation.AnimationEndEvent += CompleteAction;
        }

        // Update is called once per frame
        void Update()
        {
            if (p_ActionFlg == true && p_CompleteFlg == true)
            {
                // アクション完了時のイベント呼び出し
                CompletedEvent?.Invoke();
                p_ActionFlg = false;
                p_CompleteFlg = false;
            }
        }

        private void LateUpdate()
        {
            this.transform.position = p_TrackingTransform.position;
            this.transform.rotation = p_TrackingTransform.rotation;
        }


        public void NothingStart()
        {
            // モーション実行中は処理しない
            if (p_ActionFlg) return;

            // 処理なしに完了イベントを呼び出し
            // 各種フラグの設定
            p_ActionFlg = true;
            p_CompleteFlg = true;
        }

        public void ExclamationStart()
        {
            // モーション実行中は処理しない
            if (p_ActionFlg) return;

            // マークの切り替え
            p_EmotionMarkSwitch.ExclamationActive();

            // マーク表示アニメーションの開始
            p_EmotionMarkAnimation.ShowAnimation();

            // 各種フラグの設定
            p_ActionFlg = true;
            p_CompleteFlg = false;
        }

        public void QuestionStart()
        {
            // モーション実行中は処理しない
            if (p_ActionFlg) return;

            // マークの切り替え
            p_EmotionMarkSwitch.QuestionActive();

            // マーク表示アニメーションの開始
            p_EmotionMarkAnimation.ShowAnimation();

            // 各種フラグの設定
            p_ActionFlg = true;
            p_CompleteFlg = false;
        }

        public void IgnoredStart()
        {
            // モーション実行中は処理しない
            if (p_ActionFlg) return;

            // マークの切り替え
            p_EmotionMarkSwitch.StopActive();

            // マーク表示アニメーションの開始
            p_EmotionMarkAnimation.ShowAnimation();

            // 各種フラグの設定
            p_ActionFlg = true;
            p_CompleteFlg = false;
        }

        private void CompleteAction()
        {
            // 状態を初期状態に戻す
            p_EmotionMarkAnimation.NeutralAnimation();

            // 実行完了フラグを立てる
            // イベント呼び出しはUpdateメインスレッドで行う
            p_CompleteFlg = true;
        }
    }
}