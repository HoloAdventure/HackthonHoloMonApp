using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Behave.Purpose;
using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave
{
    /// <summary>
    /// ホロモンの行動API
    /// </summary>
    [RequireComponent(typeof(HoloMonBehaveReference))]
    public class HoloMonBehaveAPI : MonoBehaviour
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonBehaveReference p_Reference;

        /// <summary>
        /// ホロモンの目的行動APIの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンの目的行動APIの参照")]
        private HoloMonPurposeBehaveAPI p_PurposeBehave;

        /// <summary>
        /// ホロモンのアクションモードロジックAPIの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンのアクションモードロジックAPIの参照")]
        private HoloMonActionModeLogicAPI p_ActionModeLogic;

        /// <summary>
        /// 現在実行中の目的行動
        /// </summary>
        public HoloMonPurposeType CurrentBehave => p_PurposeBehave.CurrentBehave;

        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        private List<HoloMonBehaveIF> p_ListIF
            => p_ListIFInstance
            ?? (p_ListIFInstance = new List<HoloMonBehaveIF>()
            {
                p_PurposeBehave,
                p_ActionModeLogic,
            });

        /// <summary>
        /// インタフェースの参照リスト(実体)
        /// </summary>
        private List<HoloMonBehaveIF> p_ListIFInstance;

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            p_Reference = this.GetComponent<HoloMonBehaveReference>();
            foreach (HoloMonBehaveIF instance in p_ListIF)
            {
                instance.AwakeInit(p_Reference);
            }
        }

        /// <summary>
        /// 実行待機中の目的行動に割込み通知を行う
        /// </summary>
        public bool TransmissionInterrupt(InterruptInformation a_InterruptInfo)
        {
            return p_PurposeBehave.TransmissionInterrupt(a_InterruptInfo);
        }

        /// <summary>
        /// 無行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallNoneAsync()
        {
            return await p_PurposeBehave.CallNoneAsync();
        }

        /// <summary>
        /// スタンバイ行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallStandbyAsync()
        {
            return await p_PurposeBehave.CallStandbyAsync();
        }

        /// <summary>
        /// 待て行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallStayWaitAsync()
        {
            return await p_PurposeBehave.CallStayWaitAsync();
        }

        /// <summary>
        /// 指定種別を探して追跡する行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallMoveTrackingAsync(
            ObjectUnderstandType a_TrackingUnderstandType, bool a_NeverGiveUp = false)
        {
            return await p_PurposeBehave.CallMoveTrackingAsync(a_TrackingUnderstandType, a_NeverGiveUp);
        }

        /// <summary>
        /// じゃんけんで遊ぶ行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallJankenAsync()
        {
            return await p_PurposeBehave.CallJankenAsync();
        }

        /// <summary>
        /// 寝る行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallSleepAsync()
        {
            return await p_PurposeBehave.CallSleepAsync();
        }

        /// <summary>
        /// ダンスを行う行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallDanceAsync()
        {
            return await p_PurposeBehave.CallDanceAsync();
        }

        /// <summary>
        /// 食事を与えられた行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallMealFoodAsync(ObjectFeatureWrap a_FoodObjectData)
        {
            return await p_PurposeBehave.CallMealFoodAsync(a_FoodObjectData);
        }

        /// <summary>
        /// ボール遊び行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallCatchBallAsync(ObjectFeatureWrap a_BallObjectData)
        {
            return await p_PurposeBehave.CallCatchBallAsync(a_BallObjectData);
        }

        /// <summary>
        /// うんちを行う行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallPutoutPoopAsync()
        {
            return await p_PurposeBehave.CallPutoutPoopAsync();
        }

        /// <summary>
        /// 掴まれる行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallHungUpAsync()
        {
            return await p_PurposeBehave.CallHungUpAsync();
        }

        /// <summary>
        /// アイテムを持ってくる行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallBringItemAsync(ObjectUnderstandType a_BringUnderstandType)
        {
            return await p_PurposeBehave.CallBringItemAsync(a_BringUnderstandType);
        }

        /// <summary>
        /// 対象から逃げる行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallRunFromAsync(ObjectUnderstandType a_RunFromUnderstandType)
        {
            return await p_PurposeBehave.CallRunFromAsync(a_RunFromUnderstandType);
        }

        /// <summary>
        /// 友人に注目する行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallLookFriendAsync()
        {
            return await p_PurposeBehave.CallLookFriendAsync();
        }
    }
}