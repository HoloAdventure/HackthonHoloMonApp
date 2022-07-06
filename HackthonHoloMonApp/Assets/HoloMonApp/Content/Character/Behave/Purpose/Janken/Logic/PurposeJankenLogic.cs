using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Behave.ModeLogic.LookAround;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose.Janken
{
    /// <summary>
    /// じゃんけんで遊ぶ行動ロジック
    /// </summary>
    public class PurposeJankenLogic : MonoBehaviour, HoloMonPurposeBehaveIF
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
        private PurposeJankenData p_Data => (PurposeJankenData)p_Info.PurposeData;

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
            return HoloMonPurposeType.Janken;
        }

        /// <summary>
        /// じゃんけんで遊ぶ行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse)
        {
            // 行動設定を反映する
            ApplyData(a_Porpuse);

            // アクション変数
            ModeLogicResult modeLogicResult;

            // 友人に注目する
            ObjectUnderstandType targetType = ObjectUnderstandType.FriendFace;

            // 指定種別のオブジェクトが見えているか否か
            GameObject targetVisionObject = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: targetType)?.Object;

            if (targetVisionObject == null)
            {
                // ターゲットが見えていない場合

                // 見回しモードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                    a_SearchObjectUnderstandType: targetType
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

            // 振り向きモードのアクションを実行待機する
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTurnTargetAcync(
                a_TargetObject: targetVisionObject
                );

            // 結果に応じて次のイベントを発生させる
            if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
            {
                // ロジックを達成できなかった場合

                // 処理を終了する
                return true;
            }

            // じゃんけんモードのアクションを実行待機する
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionJankenAcync();

            // 結果に応じて次のイベントを発生させる
            if (modeLogicResult.FinishModeStatus == HoloMonActionModeStatus.Achievement)
            {
                // ロジックを達成できた場合

                // TODO: この箇所で増加処理を行うべきかは要検討
                // 目的を達成したら機嫌度を増加する
                if (p_PurposeReference.View.ConditionsLifeAPI.HumorPercent <= 100)
                {
                    p_PurposeReference.Control.ConditionsLifeAPI.AddHumor(10);
                }
            }

            return true;
        }
    }
}