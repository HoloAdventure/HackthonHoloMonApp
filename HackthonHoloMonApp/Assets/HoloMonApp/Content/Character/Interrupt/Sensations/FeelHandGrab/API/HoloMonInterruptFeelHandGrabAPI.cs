using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.AI;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.FeelHandGrab
{
    /// <summary>
    /// 掴まれ認識によって行動を決定する
    /// </summary>
    public class HoloMonInterruptFeelHandGrabAPI : MonoBehaviour, HoloMonInterruptIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonInterruptReference p_Reference;

        /// <summary>
        /// 初期化
        /// </summary>
        public void AwakeInit(HoloMonInterruptReference reference)
        {
            p_Reference = reference;
        }


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // 掴まれ認識時の処理を設定する
            p_Reference.View.SensationsFeelHandGrab
                .ReactivePropertyHoloMonHandGrabFeel
                .ObserveOnMainThread()
                .Subscribe(grabbed => {
                    HandGrabFeel(grabbed);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 割込み情報データを作成する
        /// </summary>
        /// <param name="a_IsGrabbed"></param>
        /// <returns></returns>
        private InterruptInformation MakeInterruptInfo(bool a_IsGrabbed)
        {
            return new InterruptInformation(new InterruptFeelHandGrabData(a_IsGrabbed));
        }

        /// <summary>
        /// 掴まれ状態を感じ取った場合のアクションを実行する
        /// </summary>
        /// <param name="a_IsGrabbed"></param>
        private void HandGrabFeel(bool a_IsGrabbed)
        {
            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptInfo(a_IsGrabbed);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("HandGrabFeel Processed !!");
                return;
            }

            // 割込みで処理されなければ掴みに対するリアクションを行う
            ReactionForHandGrab(a_IsGrabbed);
        }

        /// <summary>
        /// 掴まれ状態に対するリアクション処理
        /// </summary>
        private bool ReactionForHandGrab(bool a_IsGrabbed)
        {
            // リアクションを行ったか
            bool isReactioned = false;

            if (a_IsGrabbed)
            {
                // 掴まれアクションの行動を要求する
                p_Reference.AI.RequestHungUp();
            }

            return isReactioned;
        }
    }
}