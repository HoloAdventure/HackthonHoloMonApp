using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Behave.ModeLogic.LookAround;
using HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTarget;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Behave.Purpose.CatchBall
{
    /// <summary>
    /// ボール遊び行動ロジック
    /// </summary>
    public class PurposeCatchBallLogic : MonoBehaviour, HoloMonPurposeBehaveIF
    {
        /// <summary>
        /// 目的行動共通参照
        /// </summary>
        private PurposeReference p_PurposeReference;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="behaveReference"></param>
        /// <param name="actionModeLogicAPI"></param>
        public void AwakeInit(
            HoloMonBehaveReference behaveReference,
            HoloMonActionModeLogicAPI actionModeLogicAPI)
        {
            p_PurposeReference = new PurposeReference(behaveReference, actionModeLogicAPI);
        }

        /// <summary>
        /// 反映中の設定データ
        /// </summary>
        [SerializeField]
        private PurposeInformation p_Info;
        private PurposeCatchBallData p_Data => (PurposeCatchBallData)p_Info.PurposeData;

        private void ApplyData(PurposeInformation a_Porpuse)
        {
            // 設定データの取得
            p_Info = a_Porpuse;
        }

        /// <summary>
        /// ホロモン目的行動種別
        /// </summary>
        /// <returns></returns>
        public HoloMonPurposeType GetHoloMonPurposeType()
        {
            return HoloMonPurposeType.CatchBall;
        }

        /// <summary>
        /// ボール遊び行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse)
        {
            // 行動設定を反映する
            ApplyData(a_Porpuse);

            // アクション変数
            ModeLogicResult modeLogicResult;

            // この状態でプレイヤーオブジェクトが見えていれば記憶しておく
            GameObject playerVisionObject = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.FriendFace)?.Object;

            // ボールオブジェクトを取得する
            GameObject targetBallObject = p_Data.BallObject;

            if (playerVisionObject == null)
            {
                // プレイヤーを未発見だった場合

                // 見回しモードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                    a_SearchObjectUnderstandType: ObjectUnderstandType.FriendFace
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できなかった場合

                    // 処理を終了する
                    return true;
                }

                // 発見オブジェクトを取得する
                ModeLogicLookAroundReturn lookAroundReturn = (ModeLogicLookAroundReturn)modeLogicResult.ModeLogicReturn;
                playerVisionObject = lookAroundReturn.FindedObject;
            }

            if (targetBallObject == null)
            {
                // ボールオブジェクトが指定されていない場合は視界発見によるロジックに移行する
                // 初めに最も近いボールオブジェクトを発見する
                targetBallObject = p_PurposeReference
                    .View.SensationsFieldOfVisionAPI
                    .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.Ball)?.Object;

                if (targetBallObject == null)
                {
                    // ボールを発見できない場合

                    // 見回しモードのアクションを実行待機する
                    modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                        a_SearchObjectUnderstandType: ObjectUnderstandType.Ball
                        );

                    // 結果に応じて次のイベントを発生させる
                    if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                    {
                        // ロジックを達成できなかった場合

                        // 処理を終了する
                        return true;
                    }

                    // 発見オブジェクトを取得する
                    ModeLogicLookAroundReturn lookAroundReturn = (ModeLogicLookAroundReturn)modeLogicResult.ModeLogicReturn;
                    targetBallObject = lookAroundReturn.FindedObject;
                }
            }

            // 発見したボールの状態を確認する
            ObjectUnderstandInformation ballUnderstandInformtaion = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByGameObject(a_Object: targetBallObject)?.CurrentFeatures();

            if (ballUnderstandInformtaion?.ObjectUnderstandType != ObjectUnderstandType.Ball)
            {
                // 対象がボールでない場合
                // 処理を終了する
                return true;
            }

            while (ballUnderstandInformtaion.ObjectUnderstandBallData.ItemStatus == ObjectStatusItem.Item_PlayerHold)
            {
                // プレイヤーがボールを持っている場合は放り投げられるまで待機を繰り返す

                // ターゲット追跡モードのアクションを実行待機する
                // プレイヤーの持っている食べ物を目標に 1m 手前まで歩み寄る
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                    a_TargetObject: targetBallObject,
                    a_StoppingDistance: 1.0f
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できなかった場合

                    // 処理を終了する
                    return true;
                }

                // 振り向きのアクションを実行待機する
                // プレイヤーがボールを手放すまでボールの様子を伺う
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTurnTargetAcync(
                            a_TargetObject: targetBallObject,
                            a_IsAchievementFront: false,
                            a_CheckFrontAngle: 30.0f,
                            a_IsCheckObjectStatus: true,
                            a_CheckFarDistance: 2.0f
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できなかった場合

                    // 指定種別のオブジェクトがまだ見えているか否か
                    VisionObjectWrap targetBallObjectWrap = p_PurposeReference
                        .View.SensationsFieldOfVisionAPI
                        .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.Ball);

                    targetBallObject = targetBallObjectWrap?.Object;

                    if (targetBallObject == null)
                    {
                        // ターゲットが見えていない場合

                        // 見回しモードのアクションを実行待機する
                        modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                            a_SearchObjectUnderstandType: ObjectUnderstandType.Ball
                            );

                        // 結果に応じて次のイベントを発生させる
                        if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                        {
                            // ロジックを達成できなかった場合

                            // 処理を終了する
                            return true;
                        }

                        // 目標を再発見できた場合は発見オブジェクトを取得して再ループする
                        ModeLogicLookAroundReturn lookAroundReturn = (ModeLogicLookAroundReturn)modeLogicResult.ModeLogicReturn;
                        targetBallObject = lookAroundReturn.FindedObject;
                    }
                    else
                    {
                        // 目標が見えていて回転をあきらめた場合はそのまま状態てチェックのループを繰り返して再追跡する
                        ballUnderstandInformtaion = targetBallObjectWrap?.CurrentFeatures();
                    }
                }
            }

            // ターゲット追跡モードのアクションを実行待機する
            // ボールの近く 0.15m の距離まで歩み寄る
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                a_TargetObject: targetBallObject,
                a_StoppingDistance: 0.15f,
                a_ArrivalCheckPoint: TrackingTargetCheckPosition.ObjectOrigin
                );

            // 結果に応じて次のイベントを発生させる
            if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
            {
                // ロジックを達成できなかった場合

                // 処理を終了する
                return true;
            }

            // ターゲット追跡モードのアクションを実行待機する
            // ボールを保持しつつ、プレイヤーに歩み寄る
            // 歩きの速度は遅くなる
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                a_TargetObject: playerVisionObject,
                a_Speed: 0.125f,
                a_AngleSpeed: 2.0f,
                a_HoldItemObject: p_Data.BallObject
                );

            // 結果に応じて次のイベントを発生させる
            if (modeLogicResult.FinishModeStatus == HoloMonActionModeStatus.Achievement)
            {
                // ロジックを達成できた場合

                // TODO: この箇所で増加処理を行うべきかは要検討
                // 目的を達成したら機嫌度を増加する
                if (p_PurposeReference.View.ConditionsLifeAPI.HumorPercent <= 100)
                {
                    p_PurposeReference.Control.ConditionsLifeAPI.AddHumor(40);
                }
            }

            // TODO: この箇所で増加処理を行うべきかは要検討
            // タスクを終了したら元気度を下げる
            p_PurposeReference.Control.ConditionsLifeAPI.AddStamina(-10);

            return true;
        }
    }
}