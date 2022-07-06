using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.AI;

using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.FeelEyeGaze
{
    /// <summary>
    /// 視線認識によって行動を決定する
    /// </summary>
    public class HoloMonInterruptFeelEyeGazeAPI : MonoBehaviour, HoloMonInterruptIF
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
            // 視線認識時の処理を設定する
            p_Reference.View.SensationsFeelEyeGazeAPI
                .ReactivePropertyHoloMonEyeGazed
                .ObserveOnMainThread()
                .Subscribe(seen => {
                    EyeGazeFeel(seen);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 割込み情報データを作成する
        /// </summary>
        /// <param name="a_IsSeen"></param>
        /// <returns></returns>
        private InterruptInformation MakeInterruptInfo(bool a_IsSeen)
        {
            return new InterruptInformation(new InterruptFeelEyeGazeData(a_IsSeen));
        }

        /// <summary>
        /// 視線を感じ取った場合のアクションを実行する
        /// </summary>
        /// <param name="a_IsSeen"></param>
        private void EyeGazeFeel(bool a_IsSeen)
        {
            //Debug.Log("EyeGazeFeel : " + a_IsSeen.ToString());

            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptInfo(a_IsSeen);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if(isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("EyeGazeFeel Processed !!");
                return;
            }
        }
    }
}