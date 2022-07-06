using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Interrupt.Sensations.ListenVoice;
using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.Data.Animations.Body;
using HoloMonApp.Content.Character.Data.Knowledge.Words;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Janken
{
    public class HoloMonModeLogicJanken : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.Janken;
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
            switch (a_InterruptInfo.HoloMonInterruptType)
            {
                case HoloMonInterruptType.FieldOfVision:
                    {
                        // 視界オブジェクトの割込み
                        InterruptFieldOfVisionData interruptFieldOfVisionData = a_InterruptInfo.InterruptFieldOfVisionData;
                        // イベント種別をチェックする
                        if (interruptFieldOfVisionData.FieldOfVisionEvent == HoloMonFieldOfVisionEvent.Add)
                        {
                            // 手オブジェクトが新規検出された場合はじゃんけんをする
                            isProcessed = CheckFindHand(interruptFieldOfVisionData.VisionObjectWrap);
                        }
                    }
                    break;
                case HoloMonInterruptType.ListenWord:
                    {
                        // 音声入力の割込み
                        InterruptListenVoiceData interruptListenWordData = a_InterruptInfo.InterruptListenWordData;
                        // じゃんけんの合図をチェックする
                        isProcessed = ListenWord(interruptListenWordData.ListenWord);
                    }
                    break;
                default:
                    break;
            }

            return isProcessed;
        }


        /// <summary>
        /// 固有アクションデータの参照
        /// </summary>
        private ModeLogicJankenData p_Data =>
            (ModeLogicJankenData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// ホロモンのじゃんけんポーズ
        /// </summary>
        [SerializeField, Tooltip("ホロモンのじゃんけんポーズ")]
        JankenPose p_HoloMonJankenPose;

        /// <summary>
        /// ホロモンのじゃんけん勝敗
        /// </summary>
        [SerializeField, Tooltip("ホロモンのじゃんけん勝敗")]
        JankenResult p_HoloMonJankenResult;


        /// <summary>
        /// 成功時の上昇空腹度
        /// </summary>
        [SerializeField, Tooltip("成功時の上昇空腹度")]
        int p_IncreaseHumor = 20;


        /// <summary>
        /// よそ見実行のトリガー
        /// </summary>
        IDisposable p_LookawayTrigger;

        /// <summary>
        /// ジャンケン時間経過実行のトリガー
        /// </summary>
        IDisposable p_JankenAutoTimeTrigger;

        /// <summary>
        /// ジャンケン結果判定のディレイトリガー
        /// </summary>
        IDisposable p_JankenResultDelayTrigger;

        /// <summary>
        /// ジャンケン終了のディレイトリガー
        /// </summary>
        IDisposable p_JankenCompleteDelayTrigger;

        /// <summary>
        /// ジャンケンの待機中か否か
        /// </summary>
        bool p_JankenWaiting;


        /// <summary>
        /// じゃんけんモードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションをじゃんけんモードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartJankenMode();

            // 頭部追従ロジックを設定する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();

            // 初期状態はじゃんけんの待機状態に移行する
            MigrationWaitStatus();

            return true;
        }

        /// <summary>
        /// じゃんけんモードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // ステータスをじゃんけんの待機状態に戻しておく
            MigrationWaitStatus();

            // 頭部追従ロジックを解除する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();

            // 全てのトリガーを破棄する
            p_LookawayTrigger?.Dispose();
            p_JankenAutoTimeTrigger?.Dispose();
            p_JankenResultDelayTrigger?.Dispose();
            p_JankenCompleteDelayTrigger?.Dispose();

            return true;
        }

        /// <summary>
        /// 手の検出アクションを実行する
        /// </summary>
        private bool CheckFindHand(VisionObjectWrap a_ObjectWrap)
        {
            bool isProcessed = false;

            ObjectUnderstandType objectType = a_ObjectWrap.CurrentFeatures().ObjectUnderstandType;

            if ((objectType == ObjectUnderstandType.FriendRightHand) ||
                (objectType == ObjectUnderstandType.FriendLeftHand))
            {
                // 右手か左手を発見したとき

                // 1 秒間その手を見る
                Lookaway(a_ObjectWrap.Object, 1.0f);

                // じゃんけんの手を出す
                PutoutJankenPose();

                // 処理実行フラグをONにする
                isProcessed = true;
            }

            return isProcessed;
        }

        /// <summary>
        /// 一定時間よそ見をする
        /// </summary>
        /// <param name="a_LookObject"></param>
        private bool Lookaway(GameObject a_LookObject, float a_LookawaySec)
        {
            bool isProcessed = false;

            // 指定のオブジェクトを見る
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionLookAtTarget(a_LookObject);

            // トリガーを設定済みの場合は一旦破棄する
            p_LookawayTrigger?.Dispose();

            // 指定時間後によそ見キャンセルのトリガーを実行する
            p_LookawayTrigger = Observable
                .Timer(TimeSpan.FromSeconds(a_LookawaySec))
                .SubscribeOnMainThread()
                .Subscribe(x =>
                {
                    // 頭部追従ロジックを解除する
                    p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionNoOverride();
                })
                .AddTo(this);

            isProcessed = true;

            return isProcessed;
        }


        /// <summary>
        /// 聞き取った言葉に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_ListenWord"></param>
        private bool ListenWord(HoloMonListenWord a_ListenWord)
        {
            bool isProcessed = false;

            switch (a_ListenWord)
            {
                case HoloMonListenWord.PonSyo:
                    // ぽん or あいこでしょという言葉を聞いた場合
                    CallReactionPonSyo();
                    isProcessed = true;
                    break;
                default:
                    break;
            }

            return isProcessed;
        }

        /// <summary>
        /// ぽん or あいこでしょという言葉を聞いた場合のリアクション
        /// </summary>
        private void CallReactionPonSyo()
        {
            // じゃんけんの手を出す
            PutoutJankenPose();
        }


        /// <summary>
        /// じゃんけんの手を出す
        /// </summary>
        private void PutoutJankenPose()
        {
            Debug.Log("PutoutJankenPose");

            // じゃんけん待機中か否か
            if (!p_JankenWaiting)
            {
                // 待機中でなければ処理しない
                return;
            }
            else
            {
                // 実行中の状態に変更する
                p_JankenWaiting = false;

                // 自動実行のトリガーを破棄する
                p_JankenAutoTimeTrigger?.Dispose();
            }

            // 0,1,2のいずれかのランダムな数値を取得する(0 : 40%, 1 : 30%, 2 : 30%)
            int poseSelect = UnityEngine.Random.Range(0, 10) % 3;

            Debug.Log("PutoutJankenPose poseSelect = " + poseSelect.ToString());
            switch (poseSelect)
            {
                case 0:
                    // グーを出す
                    MigrationRockStatus();
                    break;
                case 1:
                    // パーを出す
                    MigrationPaperStatus();
                    break;
                case 2:
                    // チョキを出す
                    MigrationScissorStatus();
                    break;
                default:
                    break;
            }

            // ディレイを設定済みの場合は一旦破棄する
            p_JankenResultDelayTrigger?.Dispose();

            // 判定はじゃんけんの手を出してから1秒後に行う
            p_JankenResultDelayTrigger = Observable.Timer(TimeSpan.FromSeconds(1.0f))
                .ObserveOnMainThread()
                .Subscribe(x =>
                {
                    // 1秒後に判定を行う
                    JudgeJankenResult();
                }).AddTo(this);
        }

        /// <summary>
        /// じゃんけんの結果を判定する
        /// </summary>
        private void JudgeJankenResult()
        {
            Debug.Log("JudgeJankenResult");

            // 右手もしくは左手から手のポーズ情報を取得する
            ObjectStatusHand handStatud = ObjectStatusHand.Nothing;

            // 視界内オブジェクトから現在注目中の右手オブジェクトを取得する
            VisionObjectWrap handObjectWrap = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendRightHand);

            if (handObjectWrap != null)
            {
                // 手のポーズ情報を取得する
                handStatud = handObjectWrap.CurrentFeatures().ObjectUnderstandFriendRightHandData.HandStatus;
            }
            else
            {
                // 右手オブジェクトが取得できなかった場合
                // 視界内オブジェクトから現在注目中の左手オブジェクトを取得する
                handObjectWrap = p_ModeLogicReference
                    .View.SensationsFieldOfVisionAPI
                    .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendLeftHand);

                if (handObjectWrap != null)
                {
                    // 手のポーズ情報を取得する
                    handStatud = handObjectWrap.CurrentFeatures().ObjectUnderstandFriendLeftHandData.HandStatus;
                }
            }

            if (handObjectWrap == null)
            {
                // 右手も左手も取得できなかった場合
                // 待機状態に戻る
                MigrationWaitStatus();

                return;
            }

            // プレイヤーのじゃんけんポーズを判定する
            JankenPose playerJankenPose = JankenPose.Nothing;

            switch(handStatud)
            {
                case ObjectStatusHand.Hand_Gur:
                case ObjectStatusHand.Hand_ThumbsUp:
                    // グー/サムズアップのときはグーと判定する
                    playerJankenPose = JankenPose.Gur;
                    break;
                case ObjectStatusHand.Hand_Choki:
                case ObjectStatusHand.Hand_Pistol:
                    // チョキ/ピストルのときはチョキと判定する
                    playerJankenPose = JankenPose.Choki;
                    break;
                case ObjectStatusHand.Hand_Par:
                    // パーのときはパーと判定する
                    playerJankenPose = JankenPose.Par;
                    break;
                default:
                    break;
            }


            Debug.Log("JudgeJankenResult playerPose = " + playerJankenPose.ToString());

            // プレイヤーが判定できる手を出しているか否か
            if(playerJankenPose == JankenPose.Nothing)
            {
                // 手を出していない場合は待機状態に戻る
                MigrationWaitStatus();

                return;
            }

            // アイコの判定を行う
            if ((playerJankenPose == JankenPose.Gur) && (p_HoloMonJankenPose == JankenPose.Gur) ||
                (playerJankenPose == JankenPose.Par) && (p_HoloMonJankenPose == JankenPose.Par) ||
                (playerJankenPose == JankenPose.Choki) && (p_HoloMonJankenPose == JankenPose.Choki))
            {
                // アイコの場合は待機状態に戻る
                MigrationWaitStatus();

                return;
            }

            // ホロモンの勝利の判定を行う
            if ((playerJankenPose == JankenPose.Gur) && (p_HoloMonJankenPose == JankenPose.Par) ||
                (playerJankenPose == JankenPose.Par) && (p_HoloMonJankenPose == JankenPose.Choki) ||
                (playerJankenPose == JankenPose.Choki) && (p_HoloMonJankenPose == JankenPose.Gur))
            {
                // ホロモンの勝利の場合は勝利状態に移行する
                MigrationHoloMonWinStatus();
            }

            // ホロモンの敗北の判定を行う
            if ((playerJankenPose == JankenPose.Gur) && (p_HoloMonJankenPose == JankenPose.Choki) ||
                (playerJankenPose == JankenPose.Par) && (p_HoloMonJankenPose == JankenPose.Gur) ||
                (playerJankenPose == JankenPose.Choki) && (p_HoloMonJankenPose == JankenPose.Par))
            {
                // ホロモンの敗北の場合は敗北状態に移行する
                MigrationHoloMonLoseStatus();
            }

            // ディレイを設定済みの場合は一旦破棄する
            p_JankenCompleteDelayTrigger?.Dispose();

            // 勝敗が決まってからから4秒後にじゃんけんを終了する
            p_JankenCompleteDelayTrigger = Observable.Timer(TimeSpan.FromSeconds(4.0f))
                .ObserveOnMainThread()
                .Subscribe(x =>
                {
                    // 4秒後に終了処理を行う
                    Debug.Log("Janken Achievement");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicJankenReturn()));
                }).AddTo(this);
        }

        /// <summary>
        /// じゃんけん待機状態に移行する際の処理
        /// </summary>
        private void MigrationWaitStatus()
        {
            // じゃんけんの待機状態とする
            p_JankenWaiting = true;

            // じゃんけんポーズ無し
            p_HoloMonJankenPose = JankenPose.Nothing;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenPose(p_HoloMonJankenPose);

            // 勝敗無し
            p_HoloMonJankenResult = JankenResult.Nothing;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenResult(p_HoloMonJankenResult);
        }

        /// <summary>
        /// じゃんけんグー状態に移行する際の処理
        /// </summary>
        private void MigrationRockStatus()
        {
            // じゃんけんの実行状態とする
            p_JankenWaiting = false;

            // グーのポーズ
            p_HoloMonJankenPose = JankenPose.Gur;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenPose(p_HoloMonJankenPose);

            // 勝敗無し
            p_HoloMonJankenResult = JankenResult.Nothing;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenResult(p_HoloMonJankenResult);
        }

        /// <summary>
        /// じゃんけんパー状態に移行する際の処理
        /// </summary>
        private void MigrationPaperStatus()
        {
            // じゃんけんの実行状態とする
            p_JankenWaiting = false;

            // パーのポーズ
            p_HoloMonJankenPose = JankenPose.Par;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenPose(p_HoloMonJankenPose);

            // 勝敗無し
            p_HoloMonJankenResult = JankenResult.Nothing;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenResult(p_HoloMonJankenResult);
        }

        /// <summary>
        /// じゃんけんチョキ状態に移行する際の処理
        /// </summary>
        private void MigrationScissorStatus()
        {
            // じゃんけんの実行状態とする
            p_JankenWaiting = false;

            // チョキのポーズ
            p_HoloMonJankenPose = JankenPose.Choki;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenPose(p_HoloMonJankenPose);

            // 勝敗無し
            p_HoloMonJankenResult = JankenResult.Nothing;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenResult(p_HoloMonJankenResult);
        }

        /// <summary>
        /// ホロモンの勝利状態に移行する際の処理
        /// </summary>
        private void MigrationHoloMonWinStatus()
        {
            // じゃんけんの実行状態とする
            p_JankenWaiting = false;
            
            // ホロモンの勝ちアクション
            p_HoloMonJankenResult = JankenResult.HoloMonWin;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenResult(p_HoloMonJankenResult);

            // 機嫌度を加算する
            p_ModeLogicReference.Control.ConditionsLifeAPI.AddHumor(p_IncreaseHumor);
        }

        /// <summary>
        /// ホロモンの敗北状態に移行する際の処理
        /// </summary>
        private void MigrationHoloMonLoseStatus()
        {
            // じゃんけんの実行状態とする
            p_JankenWaiting = false;
            
            // ホロモンの負けアクション
            p_HoloMonJankenResult = JankenResult.HoloMonLose;
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetJankenResult(p_HoloMonJankenResult);
        }
    }
}