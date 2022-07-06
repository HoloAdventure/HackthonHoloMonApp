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

namespace HoloMonApp.Content.Character.Behave.Purpose.MealFood
{
    /// <summary>
    /// 食事を与えられた行動ロジック
    /// </summary>
    public class PurposeMealFoodLogic : MonoBehaviour, HoloMonPurposeBehaveIF
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
        private PurposeMealFoodData p_Data => (PurposeMealFoodData)p_Info.PurposeData;

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
            return HoloMonPurposeType.MealFood;
        }

        /// <summary>
        /// 食事を与えられた行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse)
        {
            // 行動設定を反映する
            ApplyData(a_Porpuse);

            // アクション変数
            ModeLogicResult modeLogicResult;

            // 食べ物オブジェクトを取得する
            GameObject targetFoodObject = p_Data.FoodObject;

            if (targetFoodObject == null)
            {
                // 食べ物オブジェクトが指定されていない場合は視界発見によるロジックに移行する
                // 初めに最も近い食べ物オブジェクトを発見する
                targetFoodObject = p_PurposeReference
                    .View.SensationsFieldOfVisionAPI
                    .CheckCollectionByNearDistance(a_ObjectUnderstandType: ObjectUnderstandType.Food)?.Object;

                if (targetFoodObject == null)
                {
                    // 食べ物を発見できない場合

                    // 見回しモードのアクションを実行待機する
                    modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                        a_SearchObjectUnderstandType: ObjectUnderstandType.Food
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
                    targetFoodObject = lookAroundReturn.FindedObject;
                }
            }

            // 発見した食べ物の状態を確認する
            ObjectUnderstandInformation ballUnderstandInformtaion = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByGameObject(a_Object: targetFoodObject)?.CurrentFeatures();

            if (ballUnderstandInformtaion?.ObjectUnderstandType != ObjectUnderstandType.Food)
            {
                // 対象が食べ物でない場合
                // 処理を終了する
                return true;
            }

            // プレイヤーが食べ物を持っている間は繰り返し追跡し続ける
            if (ballUnderstandInformtaion.ObjectUnderstandFoodData.ItemStatus == ObjectStatusItem.Item_PlayerHold)
            {
                // プレイヤーが食べ物を持っている場合は近寄って良しと言われるのを待つ

                // ターゲット追跡モードのアクションを実行待機する
                // プレイヤーの持っている食べ物を目標に 1m 手前まで歩み寄る
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                    a_TargetObject: targetFoodObject,
                    a_StoppingDistance: 1.0f
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できなかった場合

                    // 処理を終了する
                    return true;
                }

                // プレイヤーが食べ物を持っている間は繰り返し待機し続ける
                while (ballUnderstandInformtaion.ObjectUnderstandFoodData.ItemStatus == ObjectStatusItem.Item_PlayerHold)
                {
                    // お座りのアクションを実行待機する
                    // プレイヤーが食べ物を手放すまで様子を伺う
                    modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionSitDownAcync(
                        a_LookObject: targetFoodObject,
                        a_CheckReleaseSignal: true
                        );

                    // 待機後の食べ物の状態を確認する
                    ballUnderstandInformtaion = p_PurposeReference
                        .View.SensationsFieldOfVisionAPI
                        .CheckCollectionByGameObject(a_Object: targetFoodObject)?.CurrentFeatures();

                    if (ballUnderstandInformtaion == null)
                    {
                        // 食べ物を見失っていた場合

                        // 処理を終了する
                        return true;
                    }
                }
            }

            // ターゲット追跡モードのアクションを実行待機する
            // 食べ物の近く 0.15m の距離まで歩み寄る
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                a_TargetObject: targetFoodObject,
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

            // 食事モードのアクションを実行待機する
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionMealFoodAcync(p_Data.FoodObject);

            return true;
        }
    }
}