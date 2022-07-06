using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UniRx;
using Cysharp.Threading.Tasks;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Behave.ModeLogic.Dance;
using HoloMonApp.Content.Character.Behave.ModeLogic.HungUp;
using HoloMonApp.Content.Character.Behave.ModeLogic.Janken;
using HoloMonApp.Content.Character.Behave.ModeLogic.LookAround;
using HoloMonApp.Content.Character.Behave.ModeLogic.MealFood;
using HoloMonApp.Content.Character.Behave.ModeLogic.None;
using HoloMonApp.Content.Character.Behave.ModeLogic.RunFromTarget;
using HoloMonApp.Content.Character.Behave.ModeLogic.PutoutPoop;
using HoloMonApp.Content.Character.Behave.ModeLogic.SitDown;
using HoloMonApp.Content.Character.Behave.ModeLogic.Sleep;
using HoloMonApp.Content.Character.Behave.ModeLogic.Standby;
using HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTarget;
using HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh;
using HoloMonApp.Content.Character.Behave.ModeLogic.TurnTarget;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.ModeLogic
{
    public class HoloMonActionModeLogicAPI : MonoBehaviour, HoloMonBehaveIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonBehaveReference p_Reference;

        /// <summary>
        /// 現在のモード
        /// </summary>
        [SerializeField, Tooltip("現在のモード")]
        private HoloMonActionMode p_HoloMonMode;

        /// <summary>
        /// 現在のモードの参照変数
        /// </summary>
        public HoloMonActionMode HoloMonMode => p_HoloMonMode;

        [Header("各種ロジック")]
        /// <summary>
        /// 無行動ロジック
        /// </summary>
        [SerializeField, Tooltip("無行動モードロジック")]
        private HoloMonModeLogicNone None;

        /// <summary>
        /// スタンバイモードロジック
        /// </summary>
        [SerializeField, Tooltip("スタンバイモードロジック")]
        private HoloMonModeLogicStandby Standby;

        /// <summary>
        /// 見回しロジック
        /// </summary>
        [SerializeField, Tooltip("見回しロジック")]
        private HoloMonModeLogicLookAround LookAround;

        /// <summary>
        /// ターゲット追跡モードロジック
        /// </summary>
        [SerializeField, Tooltip("ターゲット追跡モードロジック")]
        private HoloMonModeLogicTrackingTarget TrackingTarget;

        /// <summary>
        /// ターゲット追跡モードロジック(NavMesh使用)
        /// </summary>
        [SerializeField, Tooltip("ターゲット追跡モードロジック(NavMesh使用)")]
        private HoloMonModeLogicTrackingTargetNavMesh TrackingTargetNavMesh;

        /// <summary>
        /// ターゲット回転モードロジック
        /// </summary>
        [SerializeField, Tooltip("ターゲット回転モードロジック")]
        private HoloMonModeLogicTurnTarget TurnTarget;

        /// <summary>
        /// おすわりモードロジック
        /// </summary>
        [SerializeField, Tooltip("おすわりモードロジック")]
        private HoloMonModeLogicSitDown SitDown;

        /// <summary>
        /// じゃんけんモードロジック
        /// </summary>
        [SerializeField, Tooltip("じゃんけんモードロジック")]
        private HoloMonModeLogicJanken Janken;

        /// <summary>
        /// 眠りモードロジック
        /// </summary>
        [SerializeField, Tooltip("眠りモードロジック")]
        private HoloMonModeLogicSleep Sleep;

        /// <summary>
        /// ダンスモードロジック
        /// </summary>
        [SerializeField, Tooltip("ダンスモードロジック")]
        private HoloMonModeLogicDance Dance;

        /// <summary>
        /// 食事モードロジック
        /// </summary>
        [SerializeField, Tooltip("食事モードロジック")]
        private HoloMonModeLogicMealFood MealFood;

        /// <summary>
        /// うんちモードロジック
        /// </summary>
        [SerializeField, Tooltip("うんちモードロジック")]
        private HoloMonModeLogicPutoutPoop PutoutPoop;

        /// <summary>
        /// 掴まれモードロジック
        /// </summary>
        [SerializeField, Tooltip("掴まれモードロジック")]
        private HoloMonModeLogicHungUp HungUp;

        /// <summary>
        /// 逃走モードロジック
        /// </summary>
        [SerializeField, Tooltip("逃走モードロジック")]
        private HoloMonModeLogicRunFromTarget RunFromTarget;

        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        private List<HoloMonActionModeLogicIF> p_ListIF
            => p_ListIFInstance
            ?? (p_ListIFInstance = new List<HoloMonActionModeLogicIF>()
            {
                None,
                Standby,
                LookAround,
                TrackingTarget,
                TrackingTargetNavMesh,
                TurnTarget,
                SitDown,
                Janken,
                Sleep,
                Dance,
                MealFood,
                PutoutPoop,
                HungUp,
                RunFromTarget,
            });

        /// <summary>
        /// インタフェースの参照リスト(実体)
        /// </summary>
        private List<HoloMonActionModeLogicIF> p_ListIFInstance;


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="reference"></param>
        public void AwakeInit(HoloMonBehaveReference reference)
        {
            p_Reference = reference;
            foreach (HoloMonActionModeLogicIF instance in p_ListIF)
            {
                instance.AwakeInit(p_Reference);
            }
        }

        /// <summary>
        /// 実行待機中のアクションに割込み通知を行う
        /// </summary>
        public bool TransmissionInterrupt(InterruptInformation a_InterruptInfo)
        {
            return TransmissionInterruptModeLogic(a_InterruptInfo);
        }

        /// <summary>
        /// アクション無しモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionNoneAcync()
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicNoneData());

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// スタンバイモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionStandbyAcync(
            GameObject a_StartLookObject = null
            )
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicStandbyData(a_StartLookObject));

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// 見回しモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionLookAroundAcync(
            ObjectUnderstandType a_SearchObjectUnderstandType = ObjectUnderstandType.Nothing
            )
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicLookAroundData(a_SearchObjectUnderstandType));

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// 追跡モードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionTrackingTargetAcync(
            GameObject a_TargetObject,
            float a_Speed = 0.25f,
            float a_AngleSpeed = 1.0f,
            float a_StoppingDistance = 0.5f,
            TrackingTargetCheckPosition a_ArrivalCheckPoint = TrackingTargetCheckPosition.FootPosition,
            GameObject a_HoldItemObject = null
            )
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicTrackingTargetData(
                            a_TargetObject,
                            new TrackingSetting(a_Speed, a_AngleSpeed, a_StoppingDistance, a_ArrivalCheckPoint),
                            new HoldItemSetting(a_HoldItemObject)));

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// 振り向きモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionTurnTargetAcync(
            GameObject a_TargetObject,
            bool a_IsAchievementFront = true,
            float a_CheckFrontAngle = 1.0f,
            bool a_IsCheckObjectStatus = false,
            float a_CheckFarDistance = -1.0f
            )
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicTurnTargetData(
                a_TargetObject,
                a_IsAchievementFront,
                a_CheckFrontAngle,
                a_IsCheckObjectStatus,
                a_CheckFarDistance));

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// お座りモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionSitDownAcync(
            GameObject a_LookObject = null,
            float a_CheckNearDistance = -1.0f,
            float a_CheckFarDistance = -1.0f,
            bool a_CheckReleaseSignal = false
            )
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicSitDownData(
                a_LookObject,
                a_CheckNearDistance,
                a_CheckFarDistance,
                a_CheckReleaseSignal));

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// じゃんけんモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionJankenAcync()
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicJankenData());

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// 食事モードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionMealFoodAcync(
            GameObject a_FoodObject
            )
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicMealFoodData(a_FoodObject));

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// ダンスモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionDanceAcync()
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicDanceData());

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// 睡眠モードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionSleepAcync()
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicSleepData());

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// うんちモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionPutoutPoopAcync()
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicPutoutPoopData());

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// 持ち上げモードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionHungUpAcync()
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicHungUpData());

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        /// <summary>
        /// 逃走モードアクションを実行して待機する
        /// </summary>
        public async UniTask<ModeLogicResult> ActionRunFromTargetAcync(
            GameObject a_TargetObject,
            float a_Speed = 0.25f,
            float a_AngleSpeed = 1.0f,
            float a_StoppingDistance = 0.5f,
            RunFromTargetCheckPosition a_RunCheckPoint = RunFromTargetCheckPosition.FootPosition,
            GameObject a_HoldItemObject = null
            )
        {
            // モードのアクション設定を指定する
            ModeLogicSetting modeLogicSetting = new ModeLogicSetting(new ModeLogicRunFromTargetData(
                            a_TargetObject,
                            new RunSetting(a_Speed, a_AngleSpeed, a_StoppingDistance, a_RunCheckPoint),
                            new HoldItemSetting(a_HoldItemObject)));

            // 指定のモードで実行完了待機する
            return await RunModeLogicAsync(modeLogicSetting);
        }

        #region "private"
        /// <summary>
        /// 実行待機中のモードロジックに割込み通知を行う
        /// </summary>
        private bool TransmissionInterruptModeLogic(InterruptInformation a_InterruptInfo)
        {
            bool isProcessed = false;

            if (SpecificHoloMonModeLogicIntarfaces(p_HoloMonMode).CurrentRunAwaitFlg())
            {
                // 実行中のロジックに割込み通知を行う
                isProcessed = SpecificHoloMonModeLogicIntarfaces(p_HoloMonMode)
                    .TransmissionInterrupt(a_InterruptInfo);
            }

            return isProcessed;
        }

        /// <summary>
        /// モードロジックの実行完了待機
        /// </summary>
        private async UniTask<ModeLogicResult> RunModeLogicAsync(ModeLogicSetting a_Setting)
        {
            // 指定のモードロジック種別を取得する
            HoloMonActionMode actionMode = a_Setting.HoloMonActionMode;

            // 実行待機中のロジックを無効化する
            CancelModeLogic();

            // 指定のモードを現在のモードとする
            p_HoloMonMode = actionMode;

            // 指定のロジックを実行待機する
            ModeLogicResult modeLogicResult =
                await SpecificHoloMonModeLogicIntarfaces(actionMode).RunModeAsync(a_Setting);

            return modeLogicResult;
        }

        /// <summary>
        /// 実行待機中のモードロジックをキャンセルする
        /// </summary>
        private void CancelModeLogic()
        {
            // 全てのロジックを確認する
            foreach (HoloMonActionModeLogicIF logicBase in p_ListIF)
            {
                if (logicBase.CurrentRunAwaitFlg())
                {
                    // 実行待機中ならモードロジックをキャンセルする
                    logicBase.CancelMode();
                }
            }
        }

        /// <summary>
        /// 指定モードのインタフェースを返す
        /// </summary>
        /// <param name="a_HoloMonMode"></param>
        /// <returns></returns>
        private HoloMonActionModeLogicIF SpecificHoloMonModeLogicIntarfaces(HoloMonActionMode a_HoloMonMode)
        {
            // デフォルトはスタンバイモードとする
            HoloMonActionModeLogicIF specificInterface = Standby;

            // 全てのロジックを確認する
            foreach (HoloMonActionModeLogicIF logicBase in p_ListIF)
            {
                // タイプが一致するロジックを返却する
                if (logicBase.GetHoloMonActionMode() == a_HoloMonMode)
                {
                    specificInterface = logicBase;
                }
            }

            return specificInterface;
        }
        #endregion
    }
}