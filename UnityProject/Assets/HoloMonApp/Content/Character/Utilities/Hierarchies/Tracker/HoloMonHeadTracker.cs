using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Access;

namespace HoloMonApp.Content.Character.Utilities.Hierarchies.Tracker
{
    public class HoloMonHeadTracker : MonoBehaviour
    {
        [SerializeField]
        private HoloMonAccessAPI p_AccessAPI;

        [SerializeField, Tooltip("デフォルトのスケール")]
        private Vector3 p_DefaultScale;

        [SerializeField, Tooltip("現在のスケール変化比率")]
        private float p_ScaleRatio;

        [SerializeField, Tooltip("スケールの固定フラグ")]
        private bool p_LookScale;


        // Start is called before the first frame update
        void Start()
        {
            // デフォルトのスケールを保持する
            p_DefaultScale = this.transform.localScale;

            // デフォルトの変化比率は 1 とする
            p_ScaleRatio = 1.0f;

            // 身長設定変更時の処理を設定する
            p_AccessAPI.View.ConditionsBodyAPI.ReactivePropertyStatus
                .ObserveOnMainThread()
                .Subscribe(status => {
                    // 現在のサイズの変化比率を取得する
                    float currentScaleRatio = status.GetDefaultBodyHeightRatio;
                    // サイズの変化比率が異なれば設定を行う
                    if (p_ScaleRatio != currentScaleRatio)
                    {
                        // スケールが固定されている場合は変化なし
                        if (p_LookScale) return;
                        ApplySizeCondition(currentScaleRatio);
                    }
                })
                .AddTo(this);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            HeadTracking();
        }

        /// <summary>
        /// トランスフォームの追跡
        /// </summary>
        private void HeadTracking()
        {
            this.transform.position = p_AccessAPI.View.TransformsBoneAPI.HeadWorldPosition;
            this.transform.rotation = p_AccessAPI.View.TransformsBoneAPI.HeadWorldRotation;
        }

        /// <summary>
        /// 現在のホロモンの身長に合わせて大きさを反映する
        /// </summary>
        /// <param name="a_ScaleRatio"></param>
        private void ApplySizeCondition(float a_ScaleRatio)
        {
            // スケールを比率に合わせて調整する
            this.transform.localScale *= a_ScaleRatio;

            // 設定中のサイズ比率を記録する
            p_ScaleRatio = a_ScaleRatio;
        }
    }
}