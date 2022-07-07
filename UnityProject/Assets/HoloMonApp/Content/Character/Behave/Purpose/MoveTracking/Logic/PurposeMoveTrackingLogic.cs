using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Behave.ModeLogic.LookAround;
using HoloMonApp.Content.Character.View.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.Model.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Utilities.Generic;

namespace HoloMonApp.Content.Character.Behave.Purpose.MoveTracking
{
    /// <summary>
    /// 指定種別を探して追跡する行動ロジック
    /// </summary>
    public class PurposeMoveTrackingLogic : MonoBehaviour, HoloMonPurposeBehaveIF
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
        private PurposeMoveTrackingData p_Data => (PurposeMoveTrackingData)p_Info.PurposeData;

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
            return HoloMonPurposeType.MoveTracking;
        }

        /// <summary>
        /// 指定種別を探して追跡する行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse)
        {
            // 行動設定を反映する
            ApplyData(a_Porpuse);

            // アクション変数
            ModeLogicResult modeLogicResult;

            // 指定種別のオブジェクトが見えているか否か
            GameObject targetVisionObject = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: p_Data.TrackingUnderstandType)?.Object;

            while (targetVisionObject == null)
            {
                // ターゲットが見えていない場合

                // 見回しモードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                    a_SearchObjectUnderstandType: p_Data.TrackingUnderstandType
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できなかった場合
                    if (!p_Data.NeverGiveUp)
                    {
                        // 諦めないフラグがOFFの場合
                        // 処理を終了する
                        return true;
                    }
                    else
                    {
                        // 諦めないフラグがONの場合
                        // 再度見回しを行う
                        continue;
                    }
                }

                // 発見オブジェクトを取得する
                ModeLogicLookAroundReturn lookAroundReturn = (ModeLogicLookAroundReturn)modeLogicResult.ModeLogicReturn;
                targetVisionObject = lookAroundReturn.FindedObject;
            }

            bool isArrived = false;

            while (!isArrived)
            {
                // ターゲット追跡モードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                    a_TargetObject: targetVisionObject
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus == HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できた場合

                    // TODO: この箇所で増加処理を行うべきかは要検討
                    // 目的を達成したら機嫌度を増加する
                    if (p_PurposeReference.View.ConditionsLifeAPI.HumorPercent <= 100)
                    {
                        p_PurposeReference.Control.ConditionsLifeAPI.AddHumor(5);
                    }

                    // 目的を達成したら元気度を下げる
                    p_PurposeReference.Control.ConditionsLifeAPI.AddStamina(-5);

                    // 追跡ループを抜ける
                    isArrived = true;
                }
                else
                {
                    // ロジックを達成できなかった場合

                    // 指定種別のオブジェクトが見えているか否か
                    targetVisionObject = p_PurposeReference
                        .View.SensationsFieldOfVisionAPI
                        .CheckCollectionByNearDistance(a_ObjectUnderstandType: p_Data.TrackingUnderstandType)?.Object;

                    if (targetVisionObject != null)
                    {
                        // ターゲットが見えていて追跡を諦めた場合
                        // 異常事態として処理を終了する
                        return true;
                    }

                    while (targetVisionObject == null)
                    {
                        // ターゲットが見えていない場合

                        // 見回しモードのアクションを実行待機する
                        modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                            a_SearchObjectUnderstandType: p_Data.TrackingUnderstandType
                            );

                        // 結果に応じて次のイベントを発生させる
                        if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                        {
                            // ロジックを達成できなかった場合
                            if (!p_Data.NeverGiveUp)
                            {
                                // 諦めないフラグがOFFの場合
                                // 処理を終了する
                                return true;
                            }
                            else
                            {
                                // 諦めないフラグがONの場合
                                // 再度見回しを行う
                                continue;
                            }
                        }

                        // 目標を再発見できた場合は発見オブジェクトを取得して再ループする
                        ModeLogicLookAroundReturn lookAroundReturn = (ModeLogicLookAroundReturn)modeLogicResult.ModeLogicReturn;
                        targetVisionObject = lookAroundReturn.FindedObject;
                    }
                }
            }

            return true;
        }
    }
}