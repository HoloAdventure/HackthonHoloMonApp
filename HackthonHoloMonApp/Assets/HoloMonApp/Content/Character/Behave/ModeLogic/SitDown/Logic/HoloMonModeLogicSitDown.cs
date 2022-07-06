using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Interrupt.Sensations.ListenVoice;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Knowledge.Words;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.SitDown
{
    public class HoloMonModeLogicSitDown : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.SitDown;
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
                        // オッケーの合図をチェックする
                        VisionObjectWrap objectWrap = interruptFieldOfVisionData?.VisionObjectWrap;
                        if (objectWrap != null)
                        {
                            isProcessed = RectionUnderStandInformation(objectWrap);
                        }
                    }
                    break;
                case HoloMonInterruptType.ListenWord:
                    {
                        // 音声入力の割込み
                        InterruptListenVoiceData interruptListenWordData = a_InterruptInfo.InterruptListenWordData;
                        // ヨシの合図をチェックする
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
        private ModeLogicSitDownData p_Data =>
            (ModeLogicSitDownData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 現在注視中のオブジェクト
        /// </summary>
        [SerializeField, Tooltip("現在注視中のオブジェクト")]
        GameObject p_LookingObject;

        [SerializeField, Tooltip("友達をチラ見する時間間隔(秒)")]
        float p_FriendCheckTimeSpan = 5.0f;
        float p_FriendCheckTimeElapsed;

        [SerializeField, Tooltip("友達が見当たらない場合に待機を解除する上限回数")]
        int p_FriendNotFoundReleaseThreshold = 3;

        [SerializeField, Tooltip("友達が見当たらなかった回数")]
        int p_FriendNotFoundCount;

        /// <summary>
        /// よそ見実行のトリガー
        /// </summary>
        IDisposable p_LookawayTrigger;


        void Update()
        {
            // モードが実行中かチェックする
            if (p_ModeLogicCommon.ModeLogicStatus == HoloMonActionModeStatus.Runtime)
            {
                // 一定時間ごとに友達をチラ見する
                p_FriendCheckTimeElapsed += Time.deltaTime;
                if (p_FriendCheckTimeElapsed >= p_FriendCheckTimeSpan)
                {
                    // 友達が視界内に存在すればチラ見する
                    bool isLooked = LookAwayFriendObject();

                    p_FriendCheckTimeElapsed = 0.0f;

                    if (!isLooked)
                    {
                        // 友達が見当たらなかった場合はカウントアップ
                        p_FriendNotFoundCount++;

                        if (p_FriendNotFoundCount > p_FriendNotFoundReleaseThreshold)
                        {
                            // 指定回数見つからなければ失敗として待機を解除する
                            Debug.Log("SitDown Missing : Friaend Not Found");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Missing, new ModeLogicSitDownReturn()));
                            return;
                        }
                    }
                    else
                    {
                        // 友達が見つかった場合はカウントリセット
                        p_FriendNotFoundCount = 0;
                    }
                }

                // 注目オブジェクト指定時のチェックを行う
                if (p_Data.LookObject != null)
                {
                    // 近距離チェックが有効か否か
                    if (p_Data.CheckNearDistance > 0.0f)
                    {
                        bool isResult = CheckNearDistance(p_Data.LookObject, p_Data.CheckNearDistance);
                        if (isResult)
                        {
                            Debug.Log("SitDown Achievement : LookObject Near");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicSitDownReturn()));
                            return;
                        }
                    }

                    // 遠距離チェックが有効か否か
                    if (p_Data.CheckFarDistance > 0.0f)
                    {
                        bool isResult = CheckFarDistance(p_Data.LookObject, p_Data.CheckFarDistance);
                        if (isResult)
                        {
                            Debug.Log("SitDown Achievement : LookObject Far");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicSitDownReturn()));
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// おすわりモードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションをおすわりモードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.StartSitDownMode();

            // 頭部追従ロジックを設定する
            ActionHeadLookObject(p_Data.LookObject);

            return true;
        }

        /// <summary>
        /// おすわりモードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // 頭部追従ロジックを解除する
            ActionHeadLookObject(null);

            return true;
        }

        /// <summary>
        /// 友達が視界内に存在すればチラ見する
        /// </summary>
        private bool LookAwayFriendObject()
        {
            bool isLooked = false;

            // 視界内オブジェクトから最も近い友達オブジェクトを取得する
            VisionObjectWrap friendObjectWrap = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendFace);

            if (friendObjectWrap != null)
            {
                // 友達オブジェクトが視界内に取得できていれば 3 秒間よそ見アクションを行う
                Lookaway(friendObjectWrap.Object, 3.0f);
                isLooked = true;
            }

            return isLooked;
        }


        /// <summary>
        /// 一定時間よそ見をする
        /// </summary>
        /// <param name="a_LookObject"></param>
        private bool Lookaway(GameObject a_LookObject, float a_LookawaySec)
        {
            bool isProcessed = false;

            // 指定のオブジェクトを見る
            ActionHeadLookObject(a_LookObject);

            // トリガーを設定済みの場合は一旦破棄する
            p_LookawayTrigger?.Dispose();

            // 指定時間後によそ見キャンセルのトリガーを実行する
            p_LookawayTrigger = Observable
                .Timer(TimeSpan.FromSeconds(a_LookawaySec))
                .SubscribeOnMainThread()
                .Subscribe(x =>
                {
                    // 指定注視オブジェクトで頭部追従ロジックを再設定する
                    ActionHeadLookObject(p_Data.LookObject);
                })
                .AddTo(this);

            isProcessed = true;

            return isProcessed;
        }

        /// <summary>
        /// 注目オブジェクトの近距離チェックを行う
        /// </summary>
        /// <returns></returns>
        private bool CheckNearDistance(GameObject a_TargetObject, float a_CheckNearDistance)
        {
            bool isResult = false;

            // 指定オブジェクトとの距離をチェックする
            float lookObjectDistance = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CurrentDistanceForGameObject(a_Object: a_TargetObject);

            // 現在のホロモンの大きさに合わせてチェック距離を調整する
            float checkScaleNearDistance = a_CheckNearDistance + CurrentActualForwardDepthLength();

            if (lookObjectDistance < checkScaleNearDistance)
            {
                isResult = true;
            }

            return isResult;
        }

        /// <summary>
        /// 注目オブジェクトの遠距離チェックを行う
        /// </summary>
        /// <param name="a_CheckFarDistance"></param>
        /// <returns></returns>
        private bool CheckFarDistance(GameObject a_TargetObject, float a_CheckFarDistance)
        {
            bool isResult = false;

            // 指定オブジェクトとの距離をチェックする
            float lookObjectDistance = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CurrentDistanceForGameObject(a_Object: a_TargetObject);

            // 現在のホロモンの大きさに合わせてチェック距離を調整する
            float checkScaleFarDistance = a_CheckFarDistance + CurrentActualForwardDepthLength();

            if (lookObjectDistance > checkScaleFarDistance)
            {
                isResult = true;
            }

            return isResult;
        }

        /// <summary>
        /// オブジェクト理解種別に応じたリアクションを制御する
        /// </summary>
        /// <returns></returns>
        private bool RectionUnderStandInformation(VisionObjectWrap a_VisionObjectWrap)
        {
            // 解除合図をチェックしない場合は処理しない
            if (!p_Data.CheckReleaseSignal) return false;

            bool isReaction = false;

            // オブジェクトの参照を取得する
            GameObject visionObject = a_VisionObjectWrap.Object;

            // オブジェクトの特徴を取得する
            ObjectUnderstandInformation understandInfo = a_VisionObjectWrap.CurrentFeatures();

            // オブジェクト種別ごとに固有の反応を行う
            switch (understandInfo.ObjectUnderstandType)
            {
                case ObjectUnderstandType.Unknown:
                    break;
                case ObjectUnderstandType.Learning:
                    break;
                case ObjectUnderstandType.Other:
                    break;
                case ObjectUnderstandType.FriendFace:
                    // 現在の視線が顔の方を向いているかチェックする
                    if (p_LookingObject == visionObject)
                    {
                        // 頷きを見た場合
                        // 待機の完了処理を行う
                        ObjectStatusHead headStatus = understandInfo.ObjectUnderstandFriendFaceData.HeadStatus;
                        if (headStatus == ObjectStatusHead.Head_Nod)
                        {
                            Debug.Log("SitDown Achievement : Head HeadNod");
                            StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicSitDownReturn()));
                            isReaction = true;
                        }
                    }
                    break;
                case ObjectUnderstandType.FriendRightHand:
                    break;
                case ObjectUnderstandType.FriendLeftHand:
                    break;
                case ObjectUnderstandType.Ball:
                    // アイテムが手放されたとき
                    // 待機の完了処理を行う
                    ObjectStatusItem ballStatus = understandInfo.ObjectUnderstandBallData.ItemStatus;
                    if (ballStatus != ObjectStatusItem.Item_PlayerHold)
                    {
                        Debug.Log("SitDown Achievement : Item Release");
                        StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicSitDownReturn()));
                        isReaction = true;
                    }
                    break;
                case ObjectUnderstandType.Food:
                    // アイテムが手放されたとき
                    // 待機の完了処理を行う
                    ObjectStatusItem foodStatus = understandInfo.ObjectUnderstandFoodData.ItemStatus;
                    if (foodStatus != ObjectStatusItem.Item_PlayerHold)
                    {
                        Debug.Log("SitDown Achievement : Item Release");
                        StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicSitDownReturn()));
                        isReaction = true;
                    }
                    break;
                case ObjectUnderstandType.Poop:
                    break;
                case ObjectUnderstandType.Jewel:
                    break;
                default:
                    break;
            };

            return isReaction;
        }

        /// <summary>
        /// 聞き取った言葉に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_ListenWord"></param>
        private bool ListenWord(HoloMonListenWord a_ListenWord)
        {
            // 解除合図をチェックしない場合は処理しない
            if (!p_Data.CheckReleaseSignal) return false;

            bool isProcessed = false;

            switch (a_ListenWord)
            {
                case HoloMonListenWord.Yes:
                    // ヨシという言葉を聞いた場合
                    // 待機の完了処理を行う
                    Debug.Log("SitDown Achievement : Listen Yes");
                    StopMode(new ModeLogicResult(HoloMonActionModeStatus.Achievement, new ModeLogicSitDownReturn()));
                    isProcessed = true;
                    break;
                default:
                    break;
            }

            return isProcessed;
        }

        /// <summary>
        /// 頭部アクションを行う
        /// </summary>
        private void ActionHeadLookObject(GameObject a_LookObject)
        {
            if (p_LookingObject != null && a_LookObject != null)
            {
                if (p_LookingObject == a_LookObject)
                {
                    // 対象が同一オブジェクトの場合は処理しない
                    return;
                }
            }

            // 頭部追従ロジックを設定する
            p_ModeLogicReference.Control.LimbAnimationsHeadAPI.ActionLookAtTarget(a_LookObject);
            p_LookingObject = a_LookObject;
        }


        /// <summary>
        /// 現在のホロモンの前方向の体長を取得する
        /// </summary>
        /// <returns></returns>
        private float CurrentActualForwardDepthLength()
        {
            float currentActualForwardDepthLength = p_ModeLogicReference.View.SettingsLengthAPI.HoloMonActualDepthLength / 2.0f;

            return currentActualForwardDepthLength;
        }
    }
}