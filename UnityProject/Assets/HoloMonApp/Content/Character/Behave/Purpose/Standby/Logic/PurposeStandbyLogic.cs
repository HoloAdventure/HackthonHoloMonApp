using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Utilities.Generic;

namespace HoloMonApp.Content.Character.Behave.Purpose.Standby
{
    /// <summary>
    /// スタンバイ行動ロジック
    /// </summary>
    public class PurposeStandbyLogic : MonoBehaviour, HoloMonPurposeBehaveIF
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
        private PurposeStandbyData p_Data => (PurposeStandbyData)p_Info.PurposeData;

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
            return HoloMonPurposeType.Standby;
        }

        /// <summary>
        /// スタンバイ行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse)
        {
            // 行動設定を反映する
            ApplyData(a_Porpuse);

            // アクション変数
            ModeLogicResult modeLogicResult;

            // スタンバイモードのアクションを実行待機する
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionStandbyAcync();

            return true;
        }
    }
}