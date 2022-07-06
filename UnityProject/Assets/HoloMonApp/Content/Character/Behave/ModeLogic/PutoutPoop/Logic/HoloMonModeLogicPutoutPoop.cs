using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.View;

using HoloMonApp.Content.ItemSpace;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.PutoutPoop

{
    public class HoloMonModeLogicPutoutPoop : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.PutoutPoop;
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
        private ModeLogicPutoutPoopData p_Data =>
            (ModeLogicPutoutPoopData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// うんちオブジェクトの参照
        /// </summary>
        [SerializeField, Tooltip("うんちオブジェクトの参照")]
        private ItemShitController p_ShitObjectController;

        /// <summary>
        /// 成功時の減少うんち度
        /// </summary>
        [SerializeField, Tooltip("成功時の減少うんち度")]
        int p_DecreaseShit = -90;

        [SerializeField, Tooltip("うんち完了イベント")]
        private UnityEvent p_PoopCompleteEvent;


        /// <summary>
        /// トイレ実行のトリガー
        /// </summary>
        IDisposable p_ShitPutoutTrigger;

        /// <summary>
        /// アニメーション完了待機のトリガー
        /// </summary>
        IDisposable p_ActionEndTrigger;


        /// <summary>
        /// モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションをうんちモードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartPoopPutoutMode();

            // トリガーを設定済みの場合は一旦破棄する
            p_ShitPutoutTrigger?.Dispose();

            // 4.8秒後にトリガーを実行する
            p_ShitPutoutTrigger = Observable
                .Timer(TimeSpan.FromSeconds(4.8f))
                .SubscribeOnMainThread()
                .Subscribe(_ =>
                {
                    // うんちを表示する
                    SpawnShitPutout();
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

            // トリガーを破棄する
            p_ShitPutoutTrigger?.Dispose();
            p_ActionEndTrigger?.Dispose();

            return true;
        }

        /// <summary>
        /// うんちを表示する
        /// </summary>
        private void SpawnShitPutout()
        {
            // うんち出現座標を取得する
            Vector3 shitWorldPosition = p_ModeLogicReference.View.TransformsAroundAPI.WorldPoopOutletPosition;

            // うんちをホロモンの背後に表示する
            p_ShitObjectController.SpawnObject(shitWorldPosition);

            // うんち度を減少する
            p_ModeLogicReference.Control.ConditionsLifeAPI.AddPoop(p_DecreaseShit);

            // 現在のアニメーションが完了するのを待機する
            p_ActionEndTrigger = p_ModeLogicReference.View.AnimationsBodyAPI.AnimationTrigger
                .OnStateUpdateAsObservable()
                .Where(onStateInfo => !onStateInfo.Animator.IsInTransition(onStateInfo.LayerIndex)) // アニメーションの移行が完了するのを待つ
                .Where(onStateInfo => onStateInfo.StateInfo.normalizedTime > 0.95) // アニメーションの完了直前を検出する
                .Take(1)
                .Subscribe(_ =>
                {
                    // うんち完了イベントを発火する
                    p_PoopCompleteEvent.Invoke();

                    // アニメーションの完了とともに目標を達成したと判定する
                    Debug.Log("ShitPutout Achievement");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicPutoutPoopReturn()));
                })
                .AddTo(this);
        }
    }
}
