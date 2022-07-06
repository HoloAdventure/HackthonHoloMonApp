using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Sensations.FeelEyeGaze;
using HoloMonApp.Content.Character.Interrupt.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Interrupt.Sensations.ListenVoice;
using HoloMonApp.Content.Character.Interrupt.Sensations.TactileBody;
using HoloMonApp.Content.Character.Data.Animations.Body;
using HoloMonApp.Content.Character.Data.Knowledge.Words;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.Standby
{
    public class HoloMonModeLogicStandby : MonoBehaviour, HoloMonActionModeLogicIF
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
            return HoloMonActionMode.Standby;
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
                case HoloMonInterruptType.ListenWord:
                    {
                        // 音声入力の割込み
                        InterruptListenVoiceData interruptListenWordData = a_InterruptInfo.InterruptListenWordData;
                        // ヨシの合図をチェックする
                        isProcessed = ListenWord(interruptListenWordData.ListenWord);
                    }
                    break;
                case HoloMonInterruptType.EyeGazeBody:
                    {
                        // 視線認識の割込み
                        InterruptFeelEyeGazeData interruptEyeGazeBodyData = a_InterruptInfo.InterruptGazeBodyData;
                        // 視線の反応をチェックする
                        ReactionEyeGazeBody(interruptEyeGazeBodyData.IsSeen);
                    }
                    break;
                case HoloMonInterruptType.FieldOfVision:
                    {
                        // 視界オブジェクトの割込み
                        InterruptFieldOfVisionData interruptFieldOfVisionData = a_InterruptInfo.InterruptFieldOfVisionData;
                        // イベント種別をチェックする
                        switch (interruptFieldOfVisionData.FieldOfVisionEvent)
                        {
                            case HoloMonFieldOfVisionEvent.Add:
                                // よそ見中でないことを確認する
                                if (!p_LookawayFlg)
                                {
                                    // 視界オブジェクトが新規検出された場合は 1 秒間よそ見をする
                                    isProcessed = Lookaway(interruptFieldOfVisionData.VisionObjectWrap.Object, 1.0f);
                                }

                                // よそ見は割込み処理と判定しない
                                isProcessed = false;
                                break;
                            case HoloMonFieldOfVisionEvent.UpdateStatus:
                                // 理解種別に応じたリアクションを行う
                                isProcessed = RectionUnderStandInformation(interruptFieldOfVisionData.VisionObjectWrap.CurrentFeatures());
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case HoloMonInterruptType.TactileBody:
                    {
                        // 触覚オブジェクトの割込みチェックは常時チェックで実施する
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
        private ModeLogicStandbyData p_Data =>
            (ModeLogicStandbyData)p_ModeLogicCommon.ModeLogicSetting.ModeLogicData;


        /// <summary>
        /// 現在注視中のオブジェクト
        /// </summary>
        [SerializeField, Tooltip("現在注視中のオブジェクト")]
        private GameObject p_LookingObject;

        /// <summary>
        /// 現在触覚中のオブジェクト
        /// </summary>
        [SerializeField, Tooltip("現在触覚中のオブジェクト")]
        private GameObject p_TactileObject;

        /// <summary>
        /// よそ見中フラグ
        /// </summary>
        private bool p_LookawayFlg;

        /// <summary>
        /// よそ見実行のトリガー
        /// </summary>
        IDisposable p_LookawayTrigger;

        /// <summary>
        /// 一時アクション中フラグ
        /// </summary>
        private bool p_TempActionFlg;

        /// <summary>
        /// 一時アクションの実行のトリガー
        /// </summary>
        IDisposable p_TempActionTrigger;


        void Update()
        {
            // モードが実行中かチェックする
            if (p_ModeLogicCommon.ModeLogicStatus == HoloMonActionModeStatus.Runtime)
            {
                // よそ見中でないことを確認する
                if (!p_LookawayFlg)
                {
                    // 最も優先度の高い注目オブジェクトを注視する
                    LookObjectByPriority();
                }
                // アクション中でないことを確認する
                if (!p_TempActionFlg)
                {
                    // 通常時のアクションを設定する
                    StandbyAction();
                }
            }
        }


        /// <summary>
        /// モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // アニメーションを待機モードにする
            p_ModeLogicReference.Control.AnimationsBodyAPI.ReturnStandbyMode();

            // 頭部追従ロジックを設定する
            ActionHeadLookObject(p_Data.StartLookObject);

            // よそ見中フラグをOFFする
            p_LookawayFlg = false;

            // 一時アクション中フラグをOFFする
            p_TempActionFlg = false;

            // 最も優先度の高い注目オブジェクトを注視する
            LookObjectByPriority();

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
            ActionHeadLookObject(null);

            return true;
        }

        /// <summary>
        /// 通常時のアクションを設定する
        /// </summary>
        private void StandbyAction()
        {
            // 現在撫でられているかチェックする
            if (CheckTactileBodyReaction())
            {
                // 撫でられリアクションをした場合は終了
                return;
            }

            // 現在見ているオブジェクトが友達かチェックする
            if (CheckVisionObjectReaction())
            {
                // 見ているリアクションをした場合は終了
                return;
            }

            // 体の状態に応じたアクションを行う
            if (CheckStatusReaction())
            {
                // 体の状態に応じたアクションをした場合は終了
                return;
            }

            // 特に行うべきアクションがない場合
            // 通常ポーズに戻す
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetReactionPose(ReactionPose.Nothing);
        }

        /// <summary>
        /// 最も優先度の高い注目オブジェクトを注視する
        /// </summary>
        private void LookObjectByPriority()
        {
            // 視界内オブジェクトから現在最も優先度の高い注目オブジェクトを取得する
            VisionObjectWrap priorityObjectWrap = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByTypePriority();

            if (priorityObjectWrap != null)
            {
                // 優先注目オブジェクトが取得できていれば注視リアクションを行う
                ActionHeadLookObject(priorityObjectWrap.Object);
            }
            else
            {
                // 取得できていなければ注視を止める
                ActionHeadLookObject(null);
            }
        }


        /// <summary>
        /// 現在の視覚オブジェクトに対するリアクションをチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckVisionObjectReaction()
        {
            // 現在見ているオブジェクトが友達かチェックする
            VisionObjectWrap lookingVisionObjectWrap = CurrentLookingVisionObjectWrap();
            if (lookingVisionObjectWrap != null)
            {
                if (lookingVisionObjectWrap.CurrentFeatures().ObjectUnderstandType == ObjectUnderstandType.FriendFace)
                {
                    // 友達を見ていた場合は理解種別に応じたリアクションを継続して行う
                    RectionUnderStandInformation(lookingVisionObjectWrap.CurrentFeatures());
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 現在の触覚オブジェクトに対するリアクションをチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckTactileBodyReaction()
        {
            // 現在触れているオブジェクトが右手かチェックする
            TactileObjectWrap tactileRightHand = p_ModeLogicReference
                .View.SensationsTactileBodyAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendRightHand);
            if (tactileRightHand != null)
            {
                if (TouchHeadReaction(tactileRightHand))
                {
                    return true;
                }
            }

            // 現在触れているオブジェクトが左手かチェックする
            TactileObjectWrap tactileLeftHand = p_ModeLogicReference
                .View.SensationsTactileBodyAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendLeftHand);
            if (tactileLeftHand != null)
            {
                if (TouchHeadReaction(tactileLeftHand))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ステータス状態によるリアクションの実行をチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckStatusReaction()
        {
            // 現在の体調に合わせたリアクションを行う
            if(ReactionCurrentStatus())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 一定時間よそ見をする
        /// </summary>
        /// <param name="a_LookObject"></param>
        private bool Lookaway(GameObject a_LookObject, float a_LookawaySec)
        {
            bool isProcessed = false;

            // よそ見中フラグをONする
            p_LookawayFlg = true;

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
                    // よそ見中フラグをOFFする
                    p_LookawayFlg = false;
                })
                .AddTo(this);

            isProcessed = true;

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
        /// 頭に触れられたことに反応する
        /// </summary>
        /// <param name="a_TaouchObject"></param>
        private bool TouchHeadReaction(TactileObjectWrap a_TactileObjectWrap)
        {
            bool isProcessed = false;

            // 頭部に触れたオブジェクトが右手もしくは左手か
            if ((a_TactileObjectWrap.CurrentFeatures().ObjectUnderstandType == ObjectUnderstandType.FriendRightHand)
                || (a_TactileObjectWrap.CurrentFeatures().ObjectUnderstandType == ObjectUnderstandType.FriendLeftHand))
            {
                // 手の状態を取得する
                int HandStatusHash = a_TactileObjectWrap.CurrentFeatures().ObjectUnderstandDataInterface.StatusHash();

#if UNITY_EDITOR
                // Editor上では確認のため、ピストル状態でパーと判定する
                if (HandStatusHash == (int)ObjectStatusHand.Hand_Pistol) HandStatusHash = (int)ObjectStatusHand.Hand_Par;
#endif

                if (HandStatusHash == (int)ObjectStatusHand.Hand_Par)
                {
                    // パーの手で触れられていた場合は頭を撫でられたアクションを行う

                    if ((p_ModeLogicReference.View.ConditionsLifeAPI.HungryPercent >= 80) ||
                        (p_ModeLogicReference.View.ConditionsLifeAPI.HumorPercent >= 80))
                    {
                        // 満腹または機嫌がよい場合
                        // お腹を見せるアクションを実行する
                        TempReaction(ReactionPose.ShowBellyPose);
                    }
                    else
                    {
                        /// 頭を撫でられたアクションを実行する
                        TempReaction(ReactionPose.StrokingHead);
                    }

                    isProcessed = true;
                }
            }

            return isProcessed;
        }

        /// <summary>
        /// オブジェクト理解種別に応じたリアクションを制御する
        /// </summary>
        /// <returns></returns>
        private bool RectionUnderStandInformation(ObjectUnderstandInformation a_Info)
        {
            bool isReaction = false;

            // オブジェクト種別ごとに固有の反応を行う
            switch (a_Info.ObjectUnderstandType)
            {
                case ObjectUnderstandType.Unknown:
                    break;
                case ObjectUnderstandType.Learning:
                    break;
                case ObjectUnderstandType.Other:
                    break;
                case ObjectUnderstandType.FriendFace:
                    // 友達が見えていれば状態に応じてリアクションする
                    isReaction = ReactionLookFriend(a_Info.ObjectUnderstandFriendFaceData);
                    break;
                case ObjectUnderstandType.FriendRightHand:
                    // 手の状態に応じてリアクションする
                    if (a_Info.ObjectUnderstandFriendRightHandData != null)
                    {
                        isReaction = ReactionLookHand(a_Info.ObjectUnderstandFriendRightHandData.HandStatus);
                    }
                    break;
                case ObjectUnderstandType.FriendLeftHand:
                    // 手の状態に応じてリアクションする
                    if (a_Info.ObjectUnderstandFriendLeftHandData != null)
                    {
                        isReaction = ReactionLookHand(a_Info.ObjectUnderstandFriendLeftHandData.HandStatus);
                    }
                    break;
                case ObjectUnderstandType.Ball:
                    break;
                case ObjectUnderstandType.Food:
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
        /// 友人オブジェクト注視時のリアクションを制御する
        /// </summary>
        private bool ReactionLookFriend(ObjectUnderstandFriendFaceData a_Data)
        {
            bool isReaction = false;

            // 顔オブジェクトの状態をチェックする
            switch (a_Data.HeadStatus)
            {
                case ObjectStatusHead.Head_Nod:
                    // 相手が頷けばこちらも頷く
                    TempReaction(ReactionPose.HeadNod);
                    isReaction = true;
                    break;
                case ObjectStatusHead.Head_Shake:
                    // 相手が首を振ればこちらも首を振る
                    TempReaction(ReactionPose.HeadShake);
                    isReaction = true;
                    break;
                case ObjectStatusHead.Head_TiltLeft:
                    // 相手が左方向に首を傾げればこちらは右方向に首を傾げる
                    TempReaction(ReactionPose.HeadTiltRight);
                    isReaction = true;
                    break;
                case ObjectStatusHead.Head_TiltRight:
                    // 相手が右方向に首を傾げればこちらは左方向に首を傾げる
                    TempReaction(ReactionPose.HeadTiltLeft);
                    isReaction = true;
                    break;
                case ObjectStatusHead.Head_LieDown:
                    // 相手が寝転んでいればこちらも寝転ぶ
                    TempReaction(ReactionPose.LyingPose);
                    isReaction = true;
                    break;
                default:
                    // 反応する行動がない場合は特に何もしない
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
            bool isProcessed = false;

            switch (a_ListenWord)
            {
                case HoloMonListenWord.Peace:
                    // ピースという言葉を聞いた場合
                    ReactionPoseForFriend(ReactionPose.PeaceSign, 3.0f);
                    isProcessed = true;
                    break;
                case HoloMonListenWord.Status:
                    // ちょうしという言葉を聞いた場合
                    bool isAction = ReactionCurrentStatus();
                    if (!isAction)
                    {
                        // 該当する状態がなかった場合、通常状態を示すアクションを行う
                        TempReaction(ReactionPose.GoodStatus);
                    }
                    isProcessed = true;
                    break;
                default:
                    break;
            }

            return isProcessed;
        }

        /// <summary>
        /// 現在の調子に合わせたリアクションを制御する
        /// </summary>
        /// <returns></returns>
        private bool ReactionCurrentStatus()
        {
            // 各種パラメータを取得する
            int hungryPercent = p_ModeLogicReference.View.ConditionsLifeAPI.HungryPercent;
            int humorPercent = p_ModeLogicReference.View.ConditionsLifeAPI.HumorPercent;
            int staminaPercent = p_ModeLogicReference.View.ConditionsLifeAPI.StaminaPercent;

            // 疲労時のアクション
            if (staminaPercent <= 40)
            {
                TempReaction(ReactionPose.TiredStatus);
                return true;
            }

            // 空腹時のアクション
            if (hungryPercent <= 40)
            {
                TempReaction(ReactionPose.HungryStatus);
                return true;
            }

            // 退屈時のアクション
            if (humorPercent <= 40)
            {
                TempReaction(ReactionPose.BoardStatus);
                return true;
            }

            // 最高時のアクション
            if ((hungryPercent >= 80) && (humorPercent >= 80) && (staminaPercent >= 80))
            {
                TempReaction(ReactionPose.BestStatus);
                return true;
            }

            return false;
        }

        /// <summary>
        /// ハンドオブジェクト注視時のリアクションを制御する
        /// </summary>
        private bool ReactionLookHand(ObjectStatusHand a_StatusHand)
        {
            bool isReaction = false;

            switch (a_StatusHand)
            {
#if UNITY_EDITOR
                // Editor上では確認のため、ピストル状態でもチョキと判定する
                case ObjectStatusHand.Hand_Pistol:
#endif
                case ObjectStatusHand.Hand_Choki:
                    // チョキが見えていればピースサインをする
                    isReaction = ReactionPoseForFriend(ReactionPose.PeaceSign, 3.0f);
                    break;
                case ObjectStatusHand.Hand_ThumbsUp:
                    // サムズアップが両手か確認する
                    VisionObjectWrap friendRightHandObjectWrap = p_ModeLogicReference
                        .View.SensationsFieldOfVisionAPI
                        .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendRightHand);
                    VisionObjectWrap friendLeftHandObjectWrap = p_ModeLogicReference
                        .View.SensationsFieldOfVisionAPI
                        .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendLeftHand);
                    if ((friendRightHandObjectWrap != null) && (friendLeftHandObjectWrap != null))
                    {
                        int rightHandStatusHash = friendRightHandObjectWrap.CurrentFeatures().ObjectUnderstandDataInterface.StatusHash();
                        int leftHandStatusHash = friendLeftHandObjectWrap.CurrentFeatures().ObjectUnderstandDataInterface.StatusHash();
                        if ((rightHandStatusHash == (int)ObjectStatusHand.Hand_ThumbsUp) && (leftHandStatusHash == (int)ObjectStatusHand.Hand_ThumbsUp))
                        {
                            // 両手の場合は両手サムズアップサインをする
                            isReaction = ReactionPoseForFriend(ReactionPose.DoubleThumbupSign, 3.0f);
                        }
                        else
                        {
                            // 片手の場合は片手サムズアップサインをする
                            isReaction = ReactionPoseForFriend(ReactionPose.ThumbupSign, 3.0f);
                        }
                    }
                    else
                    {
                        // 片手の場合は片手サムズアップサインをする
                        isReaction = ReactionPoseForFriend(ReactionPose.ThumbupSign, 3.0f);
                    }
                    break;
                default:
                    break;
            }

            return isReaction;
        }

        /// <summary>
        /// 注視された時のリアクションを制御する
        /// </summary>
        private bool ReactionEyeGazeBody(bool a_IsSeen)
        {
            bool isReaction = false;

            if (a_IsSeen)
            {
                // 現在見ているオブジェクトが友達以外かチェックする
                VisionObjectWrap lookingVisionObjectWrap = CurrentLookingVisionObjectWrap();
                if (lookingVisionObjectWrap != null)
                {
                    if (lookingVisionObjectWrap.CurrentFeatures().ObjectUnderstandType != ObjectUnderstandType.FriendFace)
                    {
                        // 友達以外を見ていた場合は首を傾げて友達を見る
                        isReaction = ReactionPoseForFriend(ReactionPose.HeadTiltLeft, 5.0f);
                    }
                }
            }

            return isReaction;
        }

        /// <summary>
        /// 友人に向けたリアクションポーズを制御する
        /// </summary>
        private bool ReactionPoseForFriend(ReactionPose a_ReactionPose, float a_LookwaySec = 1.0f)
        {
            // 視界内に友達がいるか確認する
            VisionObjectWrap friendObjectWrap = p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType:ObjectUnderstandType.FriendFace);

            // 視界内に友達がいない場合は処理しない
            if (friendObjectWrap == null) return false;

            // 一定時間友達の方を見る
            Lookaway(friendObjectWrap.Object, a_LookwaySec);

            /// リアクションを実行する
            TempReaction(a_ReactionPose);

            return true;
        }

        /// <summary>
        /// 一時的なリアクションを実行する
        /// </summary>
        /// <returns></returns>
        private bool TempReaction(ReactionPose a_Pose)
        {
            bool isProcessed = false;

            // 一時アクション中フラグをONする
            p_TempActionFlg = true;

            /// ポーズアクションを実行する
            p_ModeLogicReference.Control.AnimationsBodyAPI.SetReactionPose(a_Pose);

            // トリガーを設定済みの場合は一旦破棄する
            p_TempActionTrigger?.Dispose();

            // アニメーション完了後キャンセルのトリガーを実行する
            p_TempActionTrigger = p_ModeLogicReference.View.AnimationsBodyAPI.AnimationTrigger
                .OnStateUpdateAsObservable()
                .Where(onStateInfo => !onStateInfo.Animator.IsInTransition(onStateInfo.LayerIndex)) // アニメーションの移行が完了するのを待つ
                .Where(onStateInfo => onStateInfo.StateInfo.normalizedTime > 0.95) // アニメーションの完了直前を検出する
                .Take(1)
                .Subscribe(_ =>
                {
                    // アクション中フラグをOFFする
                    p_TempActionFlg = false;
                })
                .AddTo(this);

            isProcessed = true;

            return isProcessed;
        }

        /// <summary>
        /// 現在の注視中オブジェクトのラッピング情報を取得する
        /// </summary>
        /// <returns></returns>
        private VisionObjectWrap CurrentLookingVisionObjectWrap()
        {
            return p_ModeLogicReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByGameObject(p_LookingObject);
        }
    }
}
