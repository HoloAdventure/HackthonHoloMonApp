using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Conditions;
using HoloMonApp.Content.Character.Data.Conditions.Body;

namespace HoloMonApp.Content.Character.Interrupt.Conditions.Body
{
    /// <summary>
    /// 身体パラメータの変化によって行動目的を決定する
    /// </summary>
    public class HoloMonInterruptConditionsBodyAPI : MonoBehaviour, HoloMonInterruptIF
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
        /// 時刻判定(分刻み)のトリガー
        /// </summary>
        IDisposable p_MinuteTimeTrigger;

        /// <summary>
        /// 通知判定のトリガー
        /// </summary>
        IDisposable p_NotifyTrigger;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // 一分毎にチェックを行う
            p_MinuteTimeTrigger = Observable
                .Timer(TimeSpan.FromSeconds(60.0f - DateTime.Now.Second), TimeSpan.FromMinutes(1.0f))
                .SubscribeOnMainThread()
                .Subscribe(x => {
                    HoloMonBodyStatus status = p_Reference.View.ConditionsBodyAPI.BodyStatus;
                    CheckBodyCondition(status);
                })
                .AddTo(this);

            // 通知が来た場合にチェックを行う
            p_NotifyTrigger = p_Reference.View.ConditionsBodyAPI.ReactivePropertyStatus
                .ObserveOnMainThread()
                .Subscribe(status => {
                    CheckBodyCondition(status);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 身体パラメータによるアクションのチェックを行う
        /// </summary>
        private void CheckBodyCondition(HoloMonBodyStatus a_Status)
        {
            // 身長のチェックを行う
            if (BodyHeightLevel(a_Status.BodyHeight.Value))
            {
                // 要求が実行されればチェックは終了する
                return;
            }
        }

        /// <summary>
        /// 身長の変化に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_BodyHeightParameter"></param>
        private bool BodyHeightLevel(float a_BodyHeightParameter)
        {
            // 現在のボディY軸ローカルスケールを実スケールとして参照する
            float currentBodyScale = p_Reference.View.TransformsBodyAPI.WorldHeight;

            // 実寸とパラメータが異なるか確認する
            if (currentBodyScale != a_BodyHeightParameter)
            {
                // 異なっていれば身長パラメータの値をスケールに反映する
                p_Reference.Control.BodyComponentsToTransformUtilityAPI.SetScaleSmooth(a_BodyHeightParameter);
                return true;
            }

            return false;
        }
    }
}