using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.AI;

using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.Control.VisualizeUIs;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Conditions;
using HoloMonApp.Content.Character.Data.Conditions.Life;

namespace HoloMonApp.Content.Character.Interrupt.Conditions.Life
{
    /// <summary>
    /// 体調の変化によって行動目的を決定する
    /// </summary>
    public class HoloMonInterruptConditionsLifeAPI : MonoBehaviour, HoloMonInterruptIF
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
                    HoloMonLifeStatus status = p_Reference.View.ConditionsLifeAPI.LifeStatus;
                    CheckLifeCondition(status);
                })
                .AddTo(this);

            // 通知が来た場合にチェックを行う
            p_NotifyTrigger = p_Reference.View.ConditionsLifeAPI.ReactivePropertyStatus
                .ObserveOnMainThread()
                .Subscribe(status => {
                    // 眠り度のチェックのみ即座に行う
                    SleepinessLevel(status.SleepinessLevel.Value);
                })
                .AddTo(this);

        }

        /// <summary>
        /// 体調によるアクションのチェックを行う
        /// </summary>
        private void CheckLifeCondition(HoloMonLifeStatus a_Status)
        {
            // 眠り度のチェックを行う
            if (SleepinessLevel(a_Status.SleepinessLevel.Value))
            {
                // 要求が実行されればチェックは終了する
                return;
            }

            // うんち度のチェックを行う
            if (ShitLevel(a_Status.PoopPercent.Value))
            {
                // 要求が実行されればチェックは終了する
                return;
            }

            // 空腹度のチェックを行う
            if (HungryLevel(a_Status.HungryPercent.Value))
            {
                // 要求が実行されればチェックは終了する
                return;
            }

            // 機嫌度のチェックを行う
            if (HumorLevel(a_Status.HumorPercent.Value))
            {
                // 要求が実行されればチェックは終了する
                return;
            }

            // 元気度のチェックを行う
            if (StaminaLevel(a_Status.StaminaPercent.Value))
            {
                // 要求が実行されればチェックは終了する
                return;
            }
        }

        /// <summary>
        /// 空腹度の変化に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_HungryPercent"></param>
        private bool HungryLevel(int a_HungryPercent)
        {
            // 空腹度が 5 以下の場合はおにくを探す
            if (a_HungryPercent <= 5)
            {
                if (p_Reference.AI.RequestSearchFood())
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 機嫌度の変化に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_HumorPercent"></param>
        private bool HumorLevel(int a_HumorPercent)
        {
            // 機嫌が 5 以下の場合はじゃんけんを始める
            if (a_HumorPercent <= 5)
            {
                if (p_Reference.AI.RequestJanken())
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 元気度の変化に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_StaminaPercent"></param>
        private bool StaminaLevel(int a_StaminaPercent)
        {
            // 元気度が 95 以上の場合はボールを探す
            if (a_StaminaPercent >= 95)
            {
                if (p_Reference.AI.RequestSearchBall())
                {
                    // 要求が通った場合は感情のびっくりUIを表示する
                    p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// うんち度の変化に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_ShitPercent"></param>
        private bool ShitLevel(int a_ShitPercent)
        {
            // うんち度が100%を超えていればうんちを実行する
            if (a_ShitPercent >= 100)
            {
                p_Reference.AI.RequestPutoutShit();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 眠り度の変化に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_SleepinessLevel"></param>
        private bool SleepinessLevel(HoloMonSleepinessLevel a_SleepinessLevel)
        {
            switch (a_SleepinessLevel)
            {
                case HoloMonSleepinessLevel.Nothing:
                    // 眠気がない場合
                    // 眠りの停止要求を行う
                    if (p_Reference.AI.RequestStopSleep())
                    {
                        return true;
                    }
                    break;
                case HoloMonSleepinessLevel.Sleepy:
                    // 眠い場合
                    // 睡眠の開始要求を行う
                    if (p_Reference.AI.RequestStartSleep())
                    {
                        return true;
                    }
                    break;
                case HoloMonSleepinessLevel.Drowsy:
                    // うとうとしている場合
                    // 何も行わない
                    break;
            }
            return false;
        }
    }
}