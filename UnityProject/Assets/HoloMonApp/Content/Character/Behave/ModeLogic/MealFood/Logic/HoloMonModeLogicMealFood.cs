using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Control;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.MealFood
{
    public class HoloMonModeLogicMealFood : MonoBehaviour, HoloMonActionModeLogicIF
    {
        /// <summary>
        /// モードロジック共通参照
        /// </summary>
        private ModeLogicReference p_ModeLogicReference;

        /// <summary>
        /// モードロジック共通情報
        /// </summary>
        [SerializeField, Tooltip("モードロジック共通情報")]
        private HoloMonModeLogicCommon p_ModeLogicCommon = new HoloMonModeLogicCommon();

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="reference"></param>
        public void AwakeInit(HoloMonBehaveReference reference)
        {
            p_ModeLogicReference = new ModeLogicReference(reference);
        }

        /// <summary>
        /// 現在の実行待機中フラグ
        /// </summary>
        /// <returns></returns>
        public bool CurrentRunAwaitFlg()
        {
            return p_ModeLogicCommon.RunAwaitFlg;
        }

        /// <summary>
        /// ホロモンアクションモード種別
        /// </summary>
        /// <returns></returns>
        public HoloMonActionMode GetHoloMonActionMode()
        {
            return HoloMonActionMode.MealFood;
        }

        /// <summary>
        /// モード実行(async/await制御)
        /// </summary>
        public async UniTask<ModeLogicResult> RunModeAsync(ModeLogicSetting a_ModeLogicSetting)
        {
            // 設定アクションデータを保持する
            p_ModeLogicCommon.SaveCommonSetting(a_ModeLogicSetting);

            // 開始処理を行う
            EnableSetting();

            // モードを開始して完了を待機する
            ModeLogicResult result = await p_ModeLogicCommon.RunModeAsync();

            // 終了状態を返却する
            return result;
        }

        /// <summary>
        /// モードキャンセル
        /// </summary>
        public void CancelMode()
        {
            // 停止処理を行う
            DisableSetting();

            // キャンセル処理を行う
            p_ModeLogicCommon.CancelMode();
        }

        /// <summary>
        /// モード内部停止
        /// </summary>
        private void StopMode(ModeLogicResult a_StopModeLogicResult)
        {
            // 停止処理を行う
            DisableSetting();

            // 停止状態を設定する
            p_ModeLogicCommon.StopMode(a_StopModeLogicResult);
        }

        /// <summary>
        /// 割込み通知
        /// </summary>
        public bool TransmissionInterrupt(InterruptInformation a_InterruptInfo)
        {
            bool isProcessed = false;

            // 処理対象の割込み処理のみ記述

            return isProcessed;
        }


        /// <summary>
        /// 固有アクションデータの参照
        /// </summary>
        private ModeLogicMealFoodData p_Data =>
            (ModeLogicMealFoodData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 成功時の上昇空腹度
        /// </summary>
        [SerializeField, Tooltip("成功時の上昇空腹度")]
        int p_IncreaseHungry;

        [SerializeField, Tooltip("成功時の上昇ちから度")]
        float p_IncreasePower;

        [SerializeField, Tooltip("成長する身長の加算値")]
        float p_HeightAdd;


        [SerializeField, Tooltip("食事完了イベント")]
        private UnityEvent p_FoodCompleteEvent;


        /// <summary>
        /// 食事アクションのトリガー
        /// </summary>
        IDisposable p_MealActionTrigger;

        /// <summary>
        /// アニメーション完了待機のトリガー
        /// </summary>
        IDisposable p_ActionEndTrigger;


        /// <summary>
        /// モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションを食事モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartFoodMealMode();

            // 頭部追従ロジックを設定する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();

            // フードアイテムを保持位置に固定する
            p_ModeLogicReference.Control.ObjectInteractionsHoldItemAPI.PinnedHoldItem(p_Data.FoodObject);


            // トリガーを設定済みの場合は一旦破棄する
            p_MealActionTrigger?.Dispose();
            p_ActionEndTrigger?.Dispose();

            // 1.5秒後にアクショントリガーを実行する
            p_MealActionTrigger = Observable
                .Timer(TimeSpan.FromSeconds(1.5f))
                .SubscribeOnMainThread()
                .Subscribe(_ =>
                {
                    // 食事による変化を実行する
                    MealAction();
                })
                .AddTo(this);

            return true;
        }

        /// <summary>
        /// モードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // 頭部追従ロジックを解除する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();

            // トリガーを破棄する
            p_MealActionTrigger?.Dispose();
            p_ActionEndTrigger?.Dispose();

            return true;
        }

        /// <summary>
        /// 食事による変化アクションを実行する
        /// </summary>
        private void MealAction()
        {
            // 体の大きさを成長させる
            p_ModeLogicReference.Control.ConditionsBodyAPI.AddHeight(p_HeightAdd);

            // 現在のアニメーションが完了するのを待機する
            p_ActionEndTrigger = p_ModeLogicReference.View.AnimationsBodyAPI.AnimationTrigger
                .OnStateUpdateAsObservable()
                .Where(onStateInfo => !onStateInfo.Animator.IsInTransition(onStateInfo.LayerIndex)) // アニメーションの移行が完了するのを待つ
                .Where(onStateInfo => onStateInfo.StateInfo.normalizedTime > 0.95) // アニメーションの完了直前を検出する
                .Take(1)
                .Subscribe(_ =>
                {
                    // 食事完了の処理を実行する
                    MealEnd();
                })
                .AddTo(this);
        }

        /// <summary>
        /// 食事を完了する
        /// </summary>
        private void MealEnd()
        {
            // 空腹度を増加する
            p_ModeLogicReference.Control.ConditionsLifeAPI.AddHungry(p_IncreaseHungry);

            // ちからを増加する
            p_ModeLogicReference.Control.ConditionsBodyAPI.AddPower(p_IncreasePower);

            // 食べ物を消滅させる
            p_ModeLogicReference.Control.ObjectInteractionsHoldItemAPI.DisappearHoldItem();

            // 食事完了イベントを発火する
            StartCoroutine("FoodCompleteLateInvoke");

            // 食事を完了すれば目標を達成したと判定する
            Debug.Log("MealEnd Achievement");
            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicMealFoodReturn()));
        }

        private IEnumerator FoodCompleteLateInvoke()
        {
            yield return new WaitForSeconds(2.0f);

            // 食事完了イベントを発火する
            p_FoodCompleteEvent.Invoke();
        }
    }
}
