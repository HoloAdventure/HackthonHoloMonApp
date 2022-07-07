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

namespace HoloMonApp.Content.Character.Behave.Purpose.BringItem
{
    /// <summary>
    /// アイテムを持ってくる行動ロジック
    /// </summary>
    public class PurposeBringItemLogic : MonoBehaviour, HoloMonPurposeBehaveIF
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
        private PurposeBringItemData p_Data => (PurposeBringItemData)p_Info.PurposeData;

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
            return HoloMonPurposeType.BringItem;
        }

        /// <summary>
        /// アイテムを持ってくる行動の開始(async/await制御)
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

            // 指定種別のオブジェクトが見えているか否か
            GameObject targetVisionObject = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: p_Data.BringUnderstandType)?.Object;

            while (targetVisionObject == null)
            {
                // ターゲットが見えていない場合

                // 見回しモードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                    a_SearchObjectUnderstandType: p_Data.BringUnderstandType
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
                targetVisionObject = lookAroundReturn.FindedObject;
            }

            // ターゲット追跡モードのアクションを実行待機する
            // ターゲットの近く 0.15m の距離まで歩み寄る
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                a_TargetObject: targetVisionObject,
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
                a_HoldItemObject: targetVisionObject
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
            }

            // TODO: この箇所で増加処理を行うべきかは要検討
            // タスクを終了したら元気度を下げる
            p_PurposeReference.Control.ConditionsLifeAPI.AddStamina(-5);

            return true;
        }
    }
}