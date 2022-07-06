using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble
{
    public class WordBubbleController : MonoBehaviour
    {
        /// <summary>
        /// 完了時イベント
        /// </summary>
        public Action CompletedEvent;

        [SerializeField, Tooltip("到達モーションの参照")]
        private RotateAroundTargetGravitation p_RotateAroundTargetGravitation;

        [SerializeField, Tooltip("破裂アニメーションの参照")]
        private WordBubbleAnimation p_BreakBubbleAnimation;

        [SerializeField, Tooltip("開始位置オブジェクト")]
        private GameObject StartObject;

        [SerializeField, Tooltip("ターゲットオブジェクト")]
        private GameObject TargetObject;

        [SerializeField, Tooltip("開始時の回転速度係数")]
        private float SpeedFactor;

        [SerializeField, Tooltip("到達までの秒数")]
        private float LerpTime;

        // 実行アニメーション定義
        [Serializable]
        private enum EnumAnimation
        {
            Noting,
            NonBreak,
            Break,
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
            p_RotateAroundTargetGravitation.MoveEndEvent += BreakAnimation;
            p_BreakBubbleAnimation.AnimationEndEvent += CompleteAction;
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

        /// <summary>
        /// Nothingアクションの実行
        /// </summary>
        public void NothingStart()
        {
            // モーション実行中は処理しない
            if (p_ActionFlg) return;

            // 処理なしに完了イベントを呼び出し
            // 各種フラグの設定
            p_ActionFlg = true;
            p_CompleteFlg = true;
        }

        /// <summary>
        /// Breakアクションの実行
        /// </summary>
        public void BreakStart()
        {
            // モーション実行中は処理しない
            if (p_ActionFlg) return;

            // 到達設定を行う
            p_RotateAroundTargetGravitation.StartObject = StartObject;
            p_RotateAroundTargetGravitation.TargetObject = TargetObject;
            p_RotateAroundTargetGravitation.SpeedFactor = SpeedFactor;
            p_RotateAroundTargetGravitation.LerpTime = LerpTime;

            // 実行アニメーションの定義を設定する
            p_Animation = EnumAnimation.Break;

            // 到達アニメーションを再生開始する
            p_RotateAroundTargetGravitation.MoveStart();

            // 破裂アニメーションを初期状態にする
            p_BreakBubbleAnimation.ActiveAnimation();

            // 各種フラグを実行中にする
            p_ActionFlg = true;
            p_CompleteFlg = false;
        }

        /// <summary>
        /// NonBreakアクションの実行
        /// </summary>
        public void NonBreakStart()
        {
            // モーション実行中は処理しない
            if (p_ActionFlg) return;

            // 到達設定を行う
            p_RotateAroundTargetGravitation.StartObject = StartObject;
            p_RotateAroundTargetGravitation.TargetObject = TargetObject;
            p_RotateAroundTargetGravitation.SpeedFactor = SpeedFactor;
            p_RotateAroundTargetGravitation.LerpTime = LerpTime;

            // 実行アニメーションの定義を設定する
            p_Animation = EnumAnimation.NonBreak;

            // 到達アニメーションを再生開始する
            p_RotateAroundTargetGravitation.MoveStart();

            // 破裂アニメーションを初期状態にする
            p_BreakBubbleAnimation.ActiveAnimation();

            // 各種フラグを実行中にする
            p_ActionFlg = true;
            p_CompleteFlg = false;
        }

        private void BreakAnimation()
        {
            switch (p_Animation)
            {
                case EnumAnimation.NonBreak:
                    p_BreakBubbleAnimation.NonBreakAnimation();
                    break;
                case EnumAnimation.Break:
                    p_BreakBubbleAnimation.BreakAnimation();
                    break;
                default:
                    break;
            }
        }

        private void CompleteAction()
        {
            // 実行完了フラグを立てる
            // イベント呼び出しはUpdateメインスレッドで行う
            p_CompleteFlg = true;
        }
    }
}