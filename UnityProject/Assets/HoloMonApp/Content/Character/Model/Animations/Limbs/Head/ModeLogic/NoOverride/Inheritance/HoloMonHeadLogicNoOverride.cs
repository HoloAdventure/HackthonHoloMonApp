using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    public class HoloMonHeadLogicNoOverride : MonoBehaviour, HoloMonHeadLogicInterface
    {
        /// <summary>
        /// 視線ロジック共通情報
        /// </summary>
        [SerializeField, Tooltip("視線ロジック共通情報")]
        private HoloMonHeadLogicCommon p_HeadLogicCommon = new HoloMonHeadLogicCommon();

        /// <summary>
        /// 現在のモードを取得する
        /// </summary>
        /// <returns></returns>
        public HoloMonActionHeadStatus GetHoloMonHeadStatus() => p_HeadLogicCommon.HeadLogicStatus;

        /// <summary>
        /// EveryValueChanged オブザーバ参照変数を取得する
        /// </summary>
        /// <returns></returns>
        public IObservable<HoloMonActionHeadStatus> GetHoloMonHeadStatusEveryValueChanged()
            => p_HeadLogicCommon.IObservableHeadLogicStatusEveryValueChanged;

        /// <summary>
        /// モード設定を有効化する
        /// </summary>
        public bool EnableHead()
        {
            // 開始処理を行う
            EnableSetting();

            // 開始状態を設定する
            p_HeadLogicCommon.SetHeadStatus(HoloMonActionHeadStatus.Runtime);

            return true;
        }

        /// <summary>
        /// モード設定を無効化する
        /// </summary>
        public bool DisableHead(HoloMonActionHeadStatus a_DisableStatus)
        {
            // 停止処理を行う
            DisableSetting();

            // 停止状態を設定する
            p_HeadLogicCommon.SetHeadStatus(a_DisableStatus);

            return true;
        }

        /// <summary>
        /// アクション設定を反映する
        /// </summary>
        /// <returns></returns>
        public bool ApplySetting(HeadLogicSetting a_HeadLogicSetting)
        {
            // 設定の反映を行う
            ApplyData(a_HeadLogicSetting);

            return true;
        }


        /// <summary>
        /// 設定アクションデータ
        /// </summary>
        [SerializeField, Tooltip("設定アクションデータ")]
        private HeadLogicNoOverrideData p_Data;


        /// <summary>
        /// オーバーライド無しモードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // 処理なし

            return true;
        }

        /// <summary>
        /// オーバーライド無しモードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // 処理なし

            return true;
        }

        /// <summary>
        /// 設定アクションデータを反映する
        /// </summary>
        private bool ApplyData(HeadLogicSetting a_HeadLogicSetting)
        {
            // 設定アクションデータをキャストして取得する
            p_Data = a_HeadLogicSetting.HeadLogicNoOverrideData;

            return true;
        }
    }
}