using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Behave;
using HoloMonApp.Content.Character.Behave.Purpose;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.AI
{
    /// <summary>
    /// ホロモンの目的行動を定義する
    /// </summary>
    [RequireComponent(typeof(HoloMonAICenterReference))]
    public class HoloMonAICenterAPI : MonoBehaviour
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonAICenterReference p_Reference;

        /// <summary>
        /// ホロモンの行動APIの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンの行動APIの参照")]
        private HoloMonBehaveAPI p_HoloMonBehaveAPI;

        /// <summary>
        /// AIの有効化フラグ
        /// </summary>
        [SerializeField]
        private bool p_EnableAI = true;

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            p_Reference = this.GetComponent<HoloMonAICenterReference>();
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // 最初の行動は基本行動を実行する
            RequestNeutral();
        }

        /// <summary>
        /// AIの有効無効の切り替え
        /// </summary>
        /// <param name="a_OnOff"></param>
        public void ChangeEnableAI(bool a_OnOff)
        {
            p_EnableAI = a_OnOff;
            RequestNeutral();
        }

        /// <summary>
        /// 状態に応じた基本行動の要求
        /// </summary>
        public void RequestNeutral()
        {
            if (p_EnableAI)
            {
                // 待機行動を実行する
                ExecuteStandbyAsync();
            }
            else
            {
                // 無行動の状態を実行する
                ExecuteNoneAsync();
            }
        }

        /// <summary>
        /// 待機状態の要求
        /// </summary>
        public bool RequestStandby()
        {
            Debug.Log("RequestStandby");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // 待機行動を実行する
            ExecuteStandbyAsync();

            return true;
        }


        /// <summary>
        /// プレイヤーに注目する要求
        /// </summary>
        public bool RequestLookPlayer()
        {
            Debug.Log("RequestLookPlayer");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // プレイヤーへの注視を実行する
            ExecuteLookPlayerAsync();

            return true;
        }


        /// <summary>
        /// プレイヤー追跡の要求
        /// </summary>
        public bool RequestTrackingPlayer()
        {
            Debug.Log("RequestTrackingPlayer");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // プレイヤー追跡を実行する
            ExecuteTrackingPlayerAsync();

            return true;
        }

        /// <summary>
        /// じゃんけん開始の要求
        /// </summary>
        public bool RequestJanken()
        {
            Debug.Log("RequestJanken");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // じゃんけん行動を実行する
            ExecuteJankenAsync();

            return true;
        }

        /// <summary>
        /// 眠り開始の要求
        /// </summary>
        public bool RequestStartSleep()
        {
            Debug.Log("RequestStartSleep");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠中ならば処理しない
            if (CheckPurposeIsSleep())
            {
                Debug.Log("Cancel : Current Sleep");
                return false;
            }

            // 眠り行動を実行する
            ExecuteSleepAsync();

            return true;
        }

        /// <summary>
        /// 眠り停止の要求
        /// </summary>
        public bool RequestStopSleep()
        {
            Debug.Log("RequestStopSleep");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 起床中ならば処理しない
            if (!CheckPurposeIsSleep())
            {
                Debug.Log("Cancel : Current Awake");
                return false;
            }

            // 待機行動を実行する（目を覚ます）
            ExecuteStandbyAsync();

            return true;
        }

        /// <summary>
        /// ダンス開始の要求
        /// </summary>
        public bool RequestDance()
        {
            Debug.Log("RequestDance");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // ダンス行動を実行する
            ExecuteDanceAsync();

            return true;
        }

        /// <summary>
        /// 食事行動の要求
        /// </summary>
        public bool RequestGiveFood(ObjectFeatureWrap a_FoodObjectData)
        {
            Debug.Log("RequestGiveFood");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // 満腹中は反応はするが行動しない
            if (CheckStatusIsFullStomach())
            {
                Debug.Log("Cancel : Current FullStomach");
                return true;
            }

            // 食事行動を実行する
            ExecuteGiveFoodAsync(a_FoodObjectData);

            return true;
        }

        /// <summary>
        /// ボール遊び行動の要求
        /// </summary>
        public bool RequestGiveBall(ObjectFeatureWrap a_BallObjectData)
        {
            Debug.Log("RequestGiveBall");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // 疲労中は反応はするが行動しない
            if (CheckStatusIsTired())
            {
                Debug.Log("Cancel : Current Tired");
                return true;
            }

            // 待機行動を実行する
            ExecuteGiveBallAsync(a_BallObjectData);

            return true;
        }

        /// <summary>
        /// うんち行動の要求
        /// </summary>
        public bool RequestPutoutShit()
        {
            Debug.Log("RequestPutoutShit");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠中は行動しない
            if (CheckPurposeIsSleep())
            {
                Debug.Log("Cancel : Current Sleep");
                return false;
            }

            // うんちの我慢中でなければ反応はするが行動しない
            if (!CheckStatusCanShitPutout())
            {
                Debug.Log("Cancel : Current Can't ShitPutout");
                return true;
            }

            // うんち行動を実行する
            ExecutePutoutShitAsync();

            return true;
        }

        /// <summary>
        /// 掴まれ行動の要求
        /// </summary>
        public bool RequestHungUp()
        {
            Debug.Log("RequestHungUp");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠中は行動しない
            if (CheckPurposeIsSleep())
            {
                Debug.Log("Cancel : Current Sleep");
                return false;
            }

            // 掴まれ行動を実行する
            ExecuteHungUpAsync();

            return true;
        }

        /// <summary>
        /// 待て行動の要求
        /// </summary>
        public bool RequestStayWait()
        {
            Debug.Log("RequestStayWait");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠中は行動しない
            if (CheckPurposeIsSleep())
            {
                Debug.Log("Cancel : Current Sleep");
                return false;
            }

            // 待て行動を実行する
            ExecuteStayWaitAsync();

            return true;
        }

        /// <summary>
        /// おにく捜索の要求
        /// </summary>
        public bool RequestSearchFood()
        {
            Debug.Log("RequestSearchFood");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // おにく捜索を実行する
            ExecuteSearchFoodAsync();

            return true;
        }

        /// <summary>
        /// ボール捜索の要求
        /// </summary>
        public bool RequestSearchBall()
        {
            Debug.Log("RequestSearchBall");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // ボール捜索を実行する
            ExecuteSearchBallAsync();

            return true;
        }

        /// <summary>
        /// シャワー捜索の要求
        /// </summary>
        public bool RequestSearchShower()
        {
            Debug.Log("RequestSearchShower");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // シャワー捜索を実行する
            ExecuteSearchShowerAsync();

            return true;
        }

        /// <summary>
        /// ベッド捜索の要求
        /// </summary>
        public bool RequestSearchBed()
        {
            Debug.Log("RequestSearchBed");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // ベッド捜索を実行する
            ExecuteSearchBedAsync();

            return true;
        }

        /// <summary>
        /// うんちから逃走の要求
        /// </summary>
        public bool RequestRunFromPoop()
        {
            Debug.Log("RequestRunFromPoop");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠中は行動しない
            if (CheckPurposeIsSleep())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // うんちから逃走を実行する
            ExecuteRunFromPoopAsync();

            return true;
        }

        /// <summary>
        /// シャワー水から逃走の要求
        /// </summary>
        public bool RequestRunFromShowerWater()
        {
            Debug.Log("RequestRunFromShowerWater");

            // AI動作の共通チェック
            if (!CheckCommon()) return false;

            // 睡眠/うんち中は行動しない
            if (CheckPurposeIsSleep() ||
                CheckPurposeIsPutoutPoop())
            {
                Debug.Log("Cancel : Current busy");
                return false;
            }

            // シャワー水から逃走を実行する
            ExecuteRunFromShowerWaterAsync();

            return true;
        }


        /// <summary>
        /// 実行待機中のロジックに割込み通知を行う
        /// </summary>
        public bool TransmissionInterrupt(InterruptInformation a_InterruptInfo)
        {
            bool isProcessed = p_HoloMonBehaveAPI.TransmissionInterrupt(a_InterruptInfo);

            return isProcessed;
        }


        /// <summary>
        /// 無行動状態の実行
        /// </summary>
        private async UniTask<bool> ExecuteNoneAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallNoneAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// 待機状態の実行
        /// </summary>
        private async UniTask<bool> ExecuteStandbyAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallStandbyAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// プレイヤーへの注目行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteLookPlayerAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallLookFriendAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }
        
        /// <summary>
        /// プレイヤー追跡の実行
        /// </summary>
        private async UniTask<bool> ExecuteTrackingPlayerAsync()
        {
            // 行動目的の処理を開始する
            // プレイヤー追跡は諦めないフラグをONにする
            bool result = await p_HoloMonBehaveAPI.CallMoveTrackingAsync(ObjectUnderstandType.FriendFace, true);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// じゃんけん行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteJankenAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallJankenAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// 眠り行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteSleepAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallSleepAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// ダンス行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteDanceAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallDanceAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// 食事行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteGiveFoodAsync(ObjectFeatureWrap a_FoodObjectData)
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallMealFoodAsync(a_FoodObjectData);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// ボール遊び行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteGiveBallAsync(ObjectFeatureWrap a_BallObjectData)
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallCatchBallAsync(a_BallObjectData);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// うんち行動の実行
        /// </summary>
        private async UniTask<bool> ExecutePutoutShitAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallPutoutPoopAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// 掴まれ行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteHungUpAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallHungUpAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// 待て行動の実行
        /// </summary>
        private async UniTask<bool> ExecuteStayWaitAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallStayWaitAsync();

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// おにく捜索の実行
        /// </summary>
        private async UniTask<bool> ExecuteSearchFoodAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallBringItemAsync(ObjectUnderstandType.Food);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// ボール捜索の実行
        /// </summary>
        private async UniTask<bool> ExecuteSearchBallAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallBringItemAsync(ObjectUnderstandType.Ball);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// シャワー捜索の実行
        /// </summary>
        private async UniTask<bool> ExecuteSearchShowerAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallBringItemAsync(ObjectUnderstandType.ShowerHead);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// ベッド捜索の実行
        /// </summary>
        private async UniTask<bool> ExecuteSearchBedAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallBringItemAsync(ObjectUnderstandType.CardboardBox);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// うんちから逃走の実行
        /// </summary>
        private async UniTask<bool> ExecuteRunFromPoopAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallRunFromAsync(ObjectUnderstandType.Poop);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }

        /// <summary>
        /// シャワー水から逃走の実行
        /// </summary>
        private async UniTask<bool> ExecuteRunFromShowerWaterAsync()
        {
            // 行動目的の処理を開始する
            bool result = await p_HoloMonBehaveAPI.CallRunFromAsync(ObjectUnderstandType.ShowerWater);

            // アクションの完了チェックを行う
            return CheckActionComplete(result);
        }



        /// <summary>
        /// アクションの完了チェック
        /// </summary>
        /// <param name="a_ActionResult"></param>
        private bool CheckActionComplete(bool a_ActionResult)
        {
            if (!a_ActionResult)
            {
                // アクションがキャンセルで終了していた場合（割込みがあった場合）
                // 本スレッドを終了する
                return false;
            }

            // アクションがキャンセル以外で完了していた場合は基本行動に戻る
            // TODO : 未達成の目的が残っていた場合はそちらの処理を再開する
            RequestNeutral();

            return true;
        }

        /// <summary>
        /// 全要求に対する共通チェックを行う
        /// </summary>
        /// <returns></returns>
        private bool CheckCommon()
        {
            if (!CheckEnableAI()) return false;
            return true;
        }

        /// <summary>
        /// 現在AI動作が有効か否か
        /// </summary>
        /// <returns></returns>
        private bool CheckEnableAI()
        {
            return p_EnableAI;
        }

        /// <summary>
        /// 現在の行動がスタンバイまたは注視待機中か否か
        /// </summary>
        /// <returns></returns>
        private bool CheckPurposeIsStanbyAndLooking()
        {
            bool isCheckPurpose = false;
            switch (p_HoloMonBehaveAPI.CurrentBehave)
            {
                // スタンバイまたは注視待機中
                case HoloMonPurposeType.Standby:
                case HoloMonPurposeType.LookFriend:
                    isCheckPurpose = true;
                    break;
                default:
                    break;
            }
            return isCheckPurpose;
        }

        /// <summary>
        /// 現在満腹状態かチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckStatusIsFullStomach()
        {
            bool isCheckStatus = false;
            // 空腹度が100以上なら満腹と判定する
            if (p_Reference.View.ConditionsLifeAPI.HungryPercent >= 100)
            {
                isCheckStatus = true;
            }
            return isCheckStatus;
        }

        /// <summary>
        /// 現在疲労状態かチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckStatusIsTired()
        {
            bool isCheckStatus = false;
            // 疲労度が0以下なら疲労と判定する
            if (p_Reference.View.ConditionsLifeAPI.StaminaPercent <= 0)
            {
                isCheckStatus = true;
            }
            return isCheckStatus;
        }

        /// <summary>
        /// 現在うんちの我慢状態かチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckStatusCanShitPutout()
        {
            bool isCheckStatus = false;
            // うんち度が50以上ならうんちの我慢状態と判定する
            if (p_Reference.View.ConditionsLifeAPI.PoopPercent >= 50)
            {
                isCheckStatus = true;
            }
            return isCheckStatus;
        }

        /// <summary>
        /// 現在の行動が眠り行動中かチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckPurposeIsSleep()
        {
            bool isCheckPurpose = false;
            switch (p_HoloMonBehaveAPI.CurrentBehave)
            {
                // 眠り行動中
                case HoloMonPurposeType.Sleep:
                    isCheckPurpose = true;
                    break;
                default:
                    break;
            }
            return isCheckPurpose;
        }

        /// <summary>
        /// 現在の行動がうんち行動中かチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckPurposeIsPutoutPoop()
        {
            bool isCheckPurpose = false;
            switch (p_HoloMonBehaveAPI.CurrentBehave)
            {
                // 眠り行動中
                case HoloMonPurposeType.PutoutPoop:
                    isCheckPurpose = true;
                    break;
                default:
                    break;
            }
            return isCheckPurpose;
        }
    }
}