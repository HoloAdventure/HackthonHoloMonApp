using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Behave.ModeLogic.LookAround;
using HoloMonApp.Content.Character.Behave.ModeLogic.RunFromTarget;
using HoloMonApp.Content.Character.View.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.Model.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Utilities.Generic;

namespace HoloMonApp.Content.Character.Behave.Purpose.RunFrom
{
    /// <summary>
    /// 対象から逃げる行動ロジック
    /// </summary>
    public class PurposeRunFromLogic : MonoBehaviour, HoloMonPurposeBehaveIF
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
        private PurposeRunFromData p_Data => (PurposeRunFromData)p_Info.PurposeData;

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
            return HoloMonPurposeType.RunFrom;
        }

        /// <summary>
        /// 対象から逃げる行動の開始(async/await制御)
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
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: p_Data.RunFromUnderstandType)?.Object;

            while (targetVisionObject == null)
            {
                // ターゲットが見えていない場合

                // 見回しモードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                    a_SearchObjectUnderstandType: p_Data.RunFromUnderstandType
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

            bool isRun = false;

            while (!isRun)
            {
                // ターゲット逃走モードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionRunFromTargetAcync(
                    a_TargetObject: targetVisionObject,
                    a_RunCheckPoint: RunFromTargetCheckPosition.ObjectOrigin
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus == HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できた場合

                    // 逃走ループを抜ける
                    isRun = true;
                }
                else
                {
                    // ロジックを達成できなかった場合
                    // 異常事態として処理を終了する

                    // TODO: この箇所で増加処理を行うべきかは要検討
                    // タスクを終了したら元気度を下げる
                    p_PurposeReference.Control.ConditionsLifeAPI.AddStamina(-5);
                    return true;
                }
            }

            // TODO: この箇所で増加処理を行うべきかは要検討
            // タスクを終了したら元気度を下げる
            p_PurposeReference.Control.ConditionsLifeAPI.AddStamina(-5);

            return true;
        }
    }
}