using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;

namespace HoloMonApp.Content.Character.Behave.Purpose.Dance
{
    /// <summary>
    /// ダンスを行う行動ロジック
    /// </summary>
    public class PurposeDanceLogic : MonoBehaviour, HoloMonPurposeBehaveIF
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
        private PurposeDanceData p_Data => (PurposeDanceData)p_Info.PurposeData;

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
            return HoloMonPurposeType.Dance;
        }

        /// <summary>
        /// ダンスを行う行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse)
        {
            // 行動設定を反映する
            ApplyData(a_Porpuse);

            // アクション変数
            ModeLogicResult modeLogicResult;

            // ステップダンスモードのアクションを実行待機する
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionDanceAcync();

            // 結果に応じて次のイベントを発生させる
            if (modeLogicResult.FinishModeStatus == HoloMonActionModeStatus.Achievement)
            {
                // ロジックを達成できた場合

                // 特になし
            }

            // TODO: この箇所で増加処理を行うべきかは要検討
            // タスクを終了したら元気度を下げる
            p_PurposeReference.Control.ConditionsLifeAPI.AddStamina(-5);

            return true;
        }
    }
}