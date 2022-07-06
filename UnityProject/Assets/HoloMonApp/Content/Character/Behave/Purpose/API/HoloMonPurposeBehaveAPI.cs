using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Behave.Purpose.BringItem;
using HoloMonApp.Content.Character.Behave.Purpose.CatchBall;
using HoloMonApp.Content.Character.Behave.Purpose.Dance;
using HoloMonApp.Content.Character.Behave.Purpose.HungUp;
using HoloMonApp.Content.Character.Behave.Purpose.Janken;
using HoloMonApp.Content.Character.Behave.Purpose.LookFriend;
using HoloMonApp.Content.Character.Behave.Purpose.MealFood;
using HoloMonApp.Content.Character.Behave.Purpose.MoveTracking;
using HoloMonApp.Content.Character.Behave.Purpose.None;
using HoloMonApp.Content.Character.Behave.Purpose.PutoutPoop;
using HoloMonApp.Content.Character.Behave.Purpose.RunFrom;
using HoloMonApp.Content.Character.Behave.Purpose.Sleep;
using HoloMonApp.Content.Character.Behave.Purpose.Standby;
using HoloMonApp.Content.Character.Behave.Purpose.StayWait;
using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose
{
    /// <summary>
    /// ホロモンの目的行動API
    /// </summary>
    public class HoloMonPurposeBehaveAPI : MonoBehaviour, HoloMonBehaveIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonBehaveReference p_Reference;

        /// <summary>
        /// アクションモードAPIの参照
        /// </summary>
        [SerializeField, Tooltip("アクションモードAPIの参照")]
        private HoloMonActionModeLogicAPI p_ActionModeLogic;

        /// <summary>
        /// 現在の行動目的
        /// </summary>
        [SerializeField, Tooltip("現在の行動目的")]
        private PurposeInformation p_CurrentPurposeInformation;

        /// <summary>
        /// 現在実行中の目的行動
        /// </summary>
        public HoloMonPurposeType CurrentBehave => p_CurrentPurposeInformation.HoloMonPurposeType;

        [Header("各種ロジック")]
        [SerializeField, Tooltip("アイテムを持ってくる行動ロジック")]
        private PurposeBringItemLogic PurposeBringItemLogic;

        [SerializeField, Tooltip("ボール遊び行動ロジック")]
        private PurposeCatchBallLogic PurposeCatchBallLogic;

        [SerializeField, Tooltip("ダンスを行う行動ロジック")]
        private PurposeDanceLogic PurposeDanceLogic;

        [SerializeField, Tooltip("掴まれる行動ロジック")]
        private PurposeHungUpLogic PurposeHungUpLogic;

        [SerializeField, Tooltip("じゃんけんで遊ぶ行動ロジック")]
        private PurposeJankenLogic PurposeJankenLogic;

        [SerializeField, Tooltip("友人に注目する行動ロジック")]
        private PurposeLookFriendLogic PurposeLookFriendLogic;

        [SerializeField, Tooltip("食事を与えられた行動ロジック")]
        private PurposeMealFoodLogic PurposeMealFoodLogic;

        [SerializeField, Tooltip("指定種別を探して追跡する行動ロジック")]
        private PurposeMoveTrackingLogic PurposeMoveTrackingLogic;

        [SerializeField, Tooltip("無行動ロジック")]
        private PurposeNoneLogic PurposeNoneLogic;

        [SerializeField, Tooltip("うんちを行う行動ロジック")]
        private PurposePutoutPoopLogic PurposePutoutPoopLogic;

        [SerializeField, Tooltip("対象から逃げる行動ロジック")]
        private PurposeRunFromLogic PurposeRunFromLogic;

        [SerializeField, Tooltip("寝る行動ロジック")]
        private PurposeSleepLogic PurposeSleepLogic;

        [SerializeField, Tooltip("スタンバイ行動ロジック")]
        private PurposeStandbyLogic PurposeStandbyLogic;

        [SerializeField, Tooltip("待て(おすわり)行動ロジック")]
        private PurposeStayWaitLogic PurposeStayWaitLogic;

        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        private List<HoloMonPurposeBehaveIF> p_ListIF
            => p_ListIFInstance
            ?? (p_ListIFInstance = new List<HoloMonPurposeBehaveIF>()
            {
                PurposeBringItemLogic,
                PurposeCatchBallLogic,
                PurposeDanceLogic,
                PurposeHungUpLogic,
                PurposeJankenLogic,
                PurposeLookFriendLogic,
                PurposeMealFoodLogic,
                PurposeMoveTrackingLogic,
                PurposeNoneLogic,
                PurposePutoutPoopLogic,
                PurposeRunFromLogic,
                PurposeSleepLogic,
                PurposeStandbyLogic,
                PurposeStayWaitLogic,
            });

        /// <summary>
        /// インタフェースの参照リスト(実体)
        /// </summary>
        private List<HoloMonPurposeBehaveIF> p_ListIFInstance;


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="reference"></param>
        public void AwakeInit(HoloMonBehaveReference reference)
        {
            p_Reference = reference;
            foreach (HoloMonPurposeBehaveIF instance in p_ListIF)
            {
                instance.AwakeInit(p_Reference, p_ActionModeLogic);
            }
        }

        /// <summary>
        /// 実行待機中の目的行動に割込み通知を行う
        /// </summary>
        public bool TransmissionInterrupt(InterruptInformation a_InterruptInfo)
        {
            return p_ActionModeLogic.TransmissionInterrupt(a_InterruptInfo);
        }

        /// <summary>
        /// 無行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallNoneAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeNoneData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// スタンバイ行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallStandbyAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeStandbyData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// 待て行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallStayWaitAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeStayWaitData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// 指定種別を探して追跡する行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallMoveTrackingAsync(
            ObjectUnderstandType a_TrackingUnderstandType, bool a_NeverGiveUp)
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeMoveTrackingData(
                a_TrackingUnderstandType,
                a_NeverGiveUp
                ));

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// じゃんけんで遊ぶ行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallJankenAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeJankenData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// 寝る行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallSleepAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeSleepData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// ダンスを行う行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallDanceAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeDanceData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// 食事を与えられた行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallMealFoodAsync(ObjectFeatureWrap a_FoodObjectData)
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeMealFoodData(a_FoodObjectData));

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// ボール遊び行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallCatchBallAsync(ObjectFeatureWrap a_BallObjectData)
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeCatchBallData(a_BallObjectData));

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// うんちを行う行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallPutoutPoopAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposePutoutPoopData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// 掴まれる行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallHungUpAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeHungUpData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// アイテムを持ってくる行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallBringItemAsync(ObjectUnderstandType a_BringUnderstandType)
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeBringItemData(a_BringUnderstandType));

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// 対象から逃げる行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallRunFromAsync(ObjectUnderstandType a_RunFromUnderstandType)
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeRunFromData(a_RunFromUnderstandType));

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        /// <summary>
        /// 友人に注目する行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> CallLookFriendAsync()
        {
            // 行動目的を設定する
            PurposeInformation currentPurposeInformation = new PurposeInformation(new PurposeLookFriendData());

            // 目的行動の開始と完了チェックを行う
            return await StartCheckPurposeBehaveComplete(currentPurposeInformation);
        }

        #region "private"
        /// <summary>
        /// 目的行動の開始と完了チェック
        /// </summary>
        /// <param name="a_PurposeResult"></param>
        private async UniTask<bool> StartCheckPurposeBehaveComplete(PurposeInformation a_Porpuse)
        {
            // 行動目的の処理を開始する
            HoloMonPurposeStatus result = await StartPorpuseAsync(a_Porpuse);

            // 処理がキャンセルされなかったか
            return CheckPurposeBehaveComplete(result);
        }

        /// <summary>
        /// 目的行動の開始
        /// </summary>
        /// <param name="a_Porpuse"></param>
        /// <returns></returns>
        private async UniTask<HoloMonPurposeStatus> StartPorpuseAsync(PurposeInformation a_Porpuse)
        {
            // 実行する目的行動の種別を記録する
            p_CurrentPurposeInformation = a_Porpuse;

            try
            {
                // 実行状態
                HoloMonPurposeStatus status = HoloMonPurposeStatus.Runtime;

                // 目的行動の実行結果
                bool result = false;

                // 目的行動の種別に応じた関数を呼び出す
                foreach (HoloMonPurposeBehaveIF basePurpose in p_ListIF)
                {
                    // 指定の目的行動を実行する
                    if (a_Porpuse.HoloMonPurposeType == basePurpose.GetHoloMonPurposeType())
                    {
                        result = await basePurpose.StartLogicAsync(a_Porpuse);
                    }
                }

                // ロジックの成否で状態を決定する
                status = result
                    ? HoloMonPurposeStatus.Achievement
                    : HoloMonPurposeStatus.Missing;

                Debug.Log("EndLogicAsync : " + a_Porpuse.HoloMonPurposeType.ToString() + ", " + result.ToString());

                return status;
            }
            catch (OperationCanceledException ex)
            {
                Debug.Log(ex.Message);

                return HoloMonPurposeStatus.Cancel;
            }
        }

        /// <summary>
        /// アクションの完了チェック
        /// </summary>
        /// <param name="a_PurposeResult"></param>
        private bool CheckPurposeBehaveComplete(HoloMonPurposeStatus a_PurposeResult)
        {
            if (a_PurposeResult == HoloMonPurposeStatus.Cancel)
            {
                // アクションがキャンセルで終了していた場合（割込みがあった場合）
                // 本スレッドを終了する
                return false;
            }

            return true;
        }

        #endregion
    }
}