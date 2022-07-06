using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble
{
    [RequireComponent(typeof(Animator))]
    public class WordBubbleAnimation : MonoBehaviour
    {
        /// <summary>
        /// アニメーション完了時イベント
        /// </summary>
        public Action AnimationEndEvent;

        // Animatorの参照
        private Animator p_Animator;

        // アニメーションパラメータのステータス名定義
        private string p_ParameterStatusName = "BubbleStatus";

        // アニメーション実行中フラグ
        private bool p_AnimationFlg;

        /// <summary>
        /// 破裂アニメーション状態のハッシュ値を取得
        /// </summary>
        private int p_BreakStateHash = Animator.StringToHash("Base Layer.BreakStatus");

        /// <summary>
        /// 破裂無しアニメーション状態のハッシュ値を取得
        /// </summary>
        private int p_NonBreakStateHash = Animator.StringToHash("Base Layer.NonBreakStatus");

        // アニメーション状態定義
        private enum EnumBubbleStatus
        {
            Neutral = 0,
            Active = 1,
            NonBreak = 2,
            Break = 3,
        }

        void Start()
        {
            // 変数を初期化する
            p_AnimationFlg = false;
            p_Animator = this.GetComponent<Animator>();
        }

        void Update()
        {
            if (!p_AnimationFlg) return;

            // 破裂または破裂無しアニメーション実行中は終了タイミングの検知を行う
            if ((p_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == p_BreakStateHash) ||
                (p_Animator.GetCurrentAnimatorStateInfo(0).fullPathHash == p_NonBreakStateHash))
            {
                // アニメーションの再生完了(normalizedTime 1以上)をチェックする
                if (p_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                {
                    // アニメーションの完了時イベントを呼び出す
                    p_AnimationFlg = false;
                    AnimationEndEvent?.Invoke();
                }
            }
        }

        /// <summary>
        /// 状態の初期化
        /// </summary>
        public void ActiveAnimation()
        {
            p_Animator.SetInteger(p_ParameterStatusName, (int)EnumBubbleStatus.Active);
            p_AnimationFlg = false;
        }

        /// <summary>
        /// 破裂無しアニメーション
        /// </summary>
        public bool NonBreakAnimation()
        {
            // アニメーション実行中は処理しない
            if (p_AnimationFlg) return false;

            p_Animator.SetInteger(p_ParameterStatusName, (int)EnumBubbleStatus.NonBreak);
            p_AnimationFlg = true;

            return true;
        }

        /// <summary>
        /// 破裂アニメーション
        /// </summary>
        public bool BreakAnimation()
        {
            // アニメーション実行中は処理しない
            if (p_AnimationFlg) return false;

            p_Animator.SetInteger(p_ParameterStatusName, (int)EnumBubbleStatus.Break);
            p_AnimationFlg = true;

            return true;
        }
    }
}