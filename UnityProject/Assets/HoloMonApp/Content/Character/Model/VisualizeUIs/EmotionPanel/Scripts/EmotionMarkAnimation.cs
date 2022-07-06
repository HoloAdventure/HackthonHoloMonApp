using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.EmotionPanel
{
    public class EmotionMarkAnimation : MonoBehaviour
    {
        /// <summary>
        /// アニメーション完了時イベント
        /// </summary>
        public Action AnimationEndEvent;

        // Animatorの参照
        private Animator p_Animator;

        // アニメーションパラメータのステータス名定義
        private string p_ParameterStatusName = "MarkStatus";

        // アニメーション実行中フラグ
        private bool p_AnimationFlg;

        // アニメーション状態定義
        private enum EnumMarkStatus
        {
            Neutral = 0,
            Show = 1,
        }

        /// <summary>
        /// デフォルト状態のハッシュ値を取得
        /// </summary>
        static int defaultState = Animator.StringToHash("Base Layer.NeutralState");

        // Start is called before the first frame update
        void Start()
        {
            // 変数を初期化する
            p_AnimationFlg = false;
            p_Animator = this.GetComponent<Animator>();
        }

        void Update()
        {
            if (!p_AnimationFlg) return;

            // アニメーション実行中は終了タイミングの検知を行う
            // アニメーションの再生完了(normalizedTime 1以上)をチェックする
            if (p_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash != defaultState &&
                p_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            {
                // アニメーションの完了時イベントを呼び出す
                p_AnimationFlg = false;
                AnimationEndEvent?.Invoke();
            }
        }

        /// <summary>
        /// 状態の初期化
        /// </summary>
        public void NeutralAnimation()
        {
            p_Animator.SetInteger(p_ParameterStatusName, (int)EnumMarkStatus.Neutral);
            p_AnimationFlg = false;
        }

        /// <summary>
        /// マーク表示アニメーション
        /// </summary>
        public bool ShowAnimation()
        {
            // アニメーション実行中は処理しない
            if (p_AnimationFlg) return false;

            p_Animator.SetInteger(p_ParameterStatusName, (int)EnumMarkStatus.Show);
            p_AnimationFlg = true;

            return true;
        }
    }
}