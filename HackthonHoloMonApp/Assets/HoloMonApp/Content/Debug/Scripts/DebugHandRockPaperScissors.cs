using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic.Janken;

namespace HoloMonApp.Content.DebugInfoSpace
{
    public class DebugHandRockPaperScissors : MonoBehaviour
    {
        [SerializeField, Tooltip("テキスト出力先")]
        private Text p_DebugText;

        [SerializeField, Tooltip("参照ジェスチャー")]
        private HandRockPaperScissorsGesture p_ReferenceGesture;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            if (p_ReferenceGesture != null)
            {
                Observable.Interval(System.TimeSpan.FromSeconds(3))
                    .ObserveOnMainThread()
                    .Subscribe(x =>
                    {
                        bool handTracking = p_ReferenceGesture.isHandTracking();
                        HandRockPaperScissorsStatus status = p_ReferenceGesture.IsRockPaperScissorsStatus();
                        p_DebugText.text = "T : " + handTracking.ToString() + ", S : " + status.ToString();
                    }).AddTo(this);
            }
        }
    }
}