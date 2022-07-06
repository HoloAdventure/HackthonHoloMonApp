using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Eye
{
    public class HoloMonPupilController : MonoBehaviour
    {
        private enum PupilAnimType
        {
            Contraction = 0,
            Expansion = 1,
        }

        [Serializable]
        private struct ActionTiming
        {
            public float StartTimeSpan;
            public float KeepTimeSpan;
            public float EndTimeSpan;
        }

        [SerializeField, Tooltip("ボディトランスフォームの参照")]
        private HoloMonTransformsBodyData p_HoloMonTransformsBodyData;

        [SerializeField, Tooltip("収縮の最小スケール")]
        private Vector3 p_SpanMinScale;

        [SerializeField, Tooltip("拡大の最大スケール")]
        private Vector3 p_SpanMaxScale;

        [SerializeField, Tooltip("アクションタイミング設定")]
        private ActionTiming p_ActionTiming;

        [SerializeField, Tooltip("瞳孔アニメーション実行中フラグ")]
        private bool p_EyeAnimationFlg = false;

        // 経過時刻
        private float timeElapsed;

        /// <summary>
        /// 一時アクションの実行のトリガー
        /// </summary>
        IDisposable p_TempActionTrigger;

        /// <summary>
        /// 瞳孔の収縮アクションを実行する
        /// </summary>
        /// <returns></returns>
        [ContextMenu("ExecuteActionContraction")]
        public bool ActionContraction()
        {
            PupiAnimation(PupilAnimType.Contraction);
            return true;
        }

        /// <summary>
        /// 瞳孔の拡大アクションを実行する
        /// </summary>
        /// <returns></returns>
        [ContextMenu("ExecuteActionExpansion")]
        public bool ActionExpansion()
        {
            PupiAnimation(PupilAnimType.Expansion);
            return true;
        }


        private bool PupiAnimation(PupilAnimType a_AnimType)
        {
            if (p_EyeAnimationFlg) return false;
            if (p_HoloMonTransformsBodyData.RightEyeShrinkage == null) return false;
            if (p_HoloMonTransformsBodyData.LeftEyeShrinkage == null) return false;

            // 実行中変数を初期化
            p_EyeAnimationFlg = true;
            timeElapsed = 0.0f;

            // 変形スケールを設定
            Vector3 targetScale = Vector3.one;
            switch (a_AnimType)
            {
                case PupilAnimType.Contraction:
                    targetScale = p_SpanMinScale;
                    break;
                case PupilAnimType.Expansion:
                    targetScale = p_SpanMaxScale;
                    break;
                default:
                    break;
            }

            // トリガーを設定済みの場合は一旦破棄する
            p_TempActionTrigger?.Dispose();

            // アニメーション完了後キャンセルのトリガーを実行する
            p_TempActionTrigger = this.LateUpdateAsObservable()
                .Subscribe(_ =>
                {
                    timeElapsed += Time.deltaTime;

                    float lerpFactor = 0.0f;
                    if (timeElapsed < p_ActionTiming.StartTimeSpan)
                    {
                        // 徐々に瞳を変形させる
                        lerpFactor = (timeElapsed % p_ActionTiming.StartTimeSpan);

                        p_HoloMonTransformsBodyData.RightEyeShrinkage.localScale = Vector3.Lerp(Vector3.one, targetScale, lerpFactor);
                        p_HoloMonTransformsBodyData.LeftEyeShrinkage.localScale = Vector3.Lerp(Vector3.one, targetScale, lerpFactor);
                    }
                    else if (timeElapsed < p_ActionTiming.StartTimeSpan + p_ActionTiming.KeepTimeSpan)
                    {
                        // 状態をキープする
                        lerpFactor = 1.0f;

                        p_HoloMonTransformsBodyData.RightEyeShrinkage.localScale = Vector3.Lerp(Vector3.one, targetScale, lerpFactor);
                        p_HoloMonTransformsBodyData.LeftEyeShrinkage.localScale = Vector3.Lerp(Vector3.one, targetScale, lerpFactor);
                    }
                    else if (timeElapsed < p_ActionTiming.StartTimeSpan + p_ActionTiming.KeepTimeSpan + p_ActionTiming.EndTimeSpan)
                    {
                        float checkTimeElapsed = timeElapsed - (p_ActionTiming.StartTimeSpan + p_ActionTiming.KeepTimeSpan);

                        // 徐々に瞳の大きさを元に戻す
                        lerpFactor = 1.0f - (checkTimeElapsed % p_ActionTiming.EndTimeSpan);

                        p_HoloMonTransformsBodyData.RightEyeShrinkage.localScale = Vector3.Lerp(Vector3.one, targetScale, lerpFactor);
                        p_HoloMonTransformsBodyData.LeftEyeShrinkage.localScale = Vector3.Lerp(Vector3.one, targetScale, lerpFactor);
                    }
                    else
                    {
                        // アニメーションの終了
                        p_EyeAnimationFlg = false;
                        p_TempActionTrigger.Dispose();
                    }
                })
                .AddTo(this);

            return true;
        }
    }
}