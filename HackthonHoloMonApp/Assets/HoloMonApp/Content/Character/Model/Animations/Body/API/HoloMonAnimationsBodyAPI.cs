using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Data.Animations.Body;

namespace HoloMonApp.Content.Character.Model.Animations.Body
{
    /// <summary>
    /// アニメーションAPI
    /// </summary>
    public class HoloMonAnimationsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンアニメーションコントローラの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンアニメーションコントローラの参照")]
        private HoloMonAnimationsController p_AnimationsController;

        /// <summary>
        /// ホロモンアニメーションハンドラの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンアニメーションハンドラの参照")]
        private HoloMonAnimationsHandler p_AnimationsHandler;

        /// <summary>
        /// ステートマシンの変化トリガー参照用
        /// </summary>
        public ObservableStateMachineTrigger AnimationTrigger => p_AnimationsHandler.AnimationTrigger;


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
        }



        /// <summary>
        /// アニメーションを見回すモードに設定する
        /// </summary>
        public void StartLookAroundMode()
        {
            // アニメーションを見回りモードに設定する
            p_AnimationsController.SetParamLookAroundMode(true);
        }

        /// <summary>
        /// アニメーションを追跡モードに設定する
        /// </summary>
        public void StartTrackingMode()
        {
            // 追跡に関する各設定を初期化する
            p_AnimationsController.SetParamSpeed(0.0f);

            // アニメーションを追跡モードに設定する
            p_AnimationsController.SetParamTrackingMode(true);
        }

        /// <summary>
        /// アニメーションを回転モードに設定する
        /// </summary>
        public void StartTurnMode()
        {
            // 回転に関する各設定を初期化する
            p_AnimationsController.SetParamRotate(0.0f);

            // アニメーションを回転モードに設定する
            p_AnimationsController.SetParamTurnMode(true);
        }

        /// <summary>
        /// アニメーションをおすわりモードに設定する
        /// </summary>
        public void StartSitDownMode()
        {
            // アニメーションをおすわりモードに設定する
            p_AnimationsController.SetParamSitDownMode(true);
        }

        /// <summary>
        /// アニメーションをじゃんけんモードに設定する
        /// </summary>
        public void StartJankenMode()
        {
            // じゃんけんに関する各設定を初期化する
            p_AnimationsController.SetParamJankenPose(JankenPose.Nothing);
            p_AnimationsController.SetParamJankenResult(JankenResult.Nothing);

            // アニメーションをじゃんけんモードに設定する
            p_AnimationsController.SetParamJankenMode(true);
        }

        /// <summary>
        /// アニメーションを食事モードに設定する
        /// </summary>
        public void StartFoodMealMode()
        {
            // アニメーションを食事モードに設定する
            p_AnimationsController.SetParamFoodMealMode(true);
        }

        /// <summary>
        /// アニメーションを眠りモードに設定する
        /// </summary>
        public void StartSleepMode()
        {
            // 眠りに関する各設定を初期化する
            p_AnimationsController.SetParamSleepPose(SleepPose.Nothing);

            // アニメーションを眠りモードに設定する
            p_AnimationsController.SetParamSleepMode(true);
        }

        /// <summary>
        /// アニメーションをダンスモードに設定する
        /// </summary>
        public void StartDanceMode()
        {
            // ダンスに関する各設定を初期化する
            p_AnimationsController.SetParamDanceType(DanceType.Nothing);

            // アニメーションをダンスモードに設定する
            p_AnimationsController.SetParamDanceMode(true);
        }

        /// <summary>
        /// アニメーションをうんちモードに設定する
        /// </summary>
        public void StartPoopPutoutMode()
        {
            // アニメーションをうんちモードに設定する
            p_AnimationsController.SetParamPoopPutoutMode(true);
        }

        /// <summary>
        /// アニメーションを掴まれモードに設定する
        /// </summary>
        public void StartHungUpMode()
        {
            // アニメーションを掴まれモードに設定する
            p_AnimationsController.SetParamHungUpMode(true);
        }

        /// <summary>
        /// アニメーションのアイテム保持オプションを変更する
        /// </summary>
        public void ChangeHoldItemOption(bool a_OnOff)
        {
            // アイテム保持オプションを設定する
            p_AnimationsController.SetParamHoldItemOption(a_OnOff);
        }


        /// <summary>
        /// アニメーションをスタンバイモードに設定する
        /// </summary>
        public void ReturnStandbyMode()
        {
            // 各設定を初期化する
            p_AnimationsController.SetParamLookAroundMode(false);
            p_AnimationsController.SetParamTrackingMode(false);
            p_AnimationsController.SetParamTurnMode(false);
            p_AnimationsController.SetParamSitDownMode(false);
            p_AnimationsController.SetParamJankenMode(false);
            p_AnimationsController.SetParamSleepMode(false);
            p_AnimationsController.SetParamDanceMode(false);
            p_AnimationsController.SetParamFoodMealMode(false);
            p_AnimationsController.SetParamPoopPutoutMode(false);
            p_AnimationsController.SetParamHungUpMode(false);

            p_AnimationsController.SetParamSpeed(0.0f);
            p_AnimationsController.SetParamRotate(0.0f);

            p_AnimationsController.SetParamHoldItemOption(false);

            p_AnimationsController.SetParamJankenPose(JankenPose.Nothing);
            p_AnimationsController.SetParamSleepPose(SleepPose.Nothing);
            p_AnimationsController.SetParamJankenResult(JankenResult.Nothing);
            p_AnimationsController.SetParamDanceType(DanceType.Nothing);
            p_AnimationsController.SetParamReactionPose(ReactionPose.Nothing);
        }


        /// <summary>
        /// モデルの現在速度を設定する
        /// </summary>
        public void SetModelSpeed(float a_Speed)
        {
            // アニメーションの速度パラメータを設定する
            p_AnimationsController.SetParamSpeed(a_Speed);
        }

        /// <summary>
        /// モデルの現在回転速度(1秒あたりの度数)を設定する
        /// </summary>
        public void SetModelRotate(float a_Rotate)
        {
            // 回転値を作成する
            // 0.0f(毎秒90度) ～ 1.0f(毎秒180度)で判定する
            float rotateValue = (a_Rotate - 90.0f) / 90.0f;
            if (rotateValue < 0.0f)
            {
                rotateValue = 0.0f;
            }

            // アニメーションの回転速度パラメータを設定する
            p_AnimationsController.SetParamRotate(rotateValue);
        }


        /// <summary>
        /// じゃんけんポーズを設定する
        /// </summary>
        public void SetJankenPose(JankenPose a_JankenPose)
        {
            // アニメーションのじゃんけんポーズを設定する
            p_AnimationsController.SetParamJankenPose(a_JankenPose);
        }

        public void SetSleepPose(SleepPose a_SleepPose)
        {
            // アニメーションの眠りポーズを設定する
            p_AnimationsController.SetParamSleepPose(a_SleepPose);
        }

        /// <summary>
        /// じゃんけん結果を設定する
        /// </summary>
        public void SetJankenResult(JankenResult a_JankenResult)
        {
            // アニメーションのじゃんけんポーズを設定する
            p_AnimationsController.SetParamJankenResult(a_JankenResult);
        }


        /// <summary>
        /// ダンス種別を設定する
        /// </summary>
        public void SetDanceType(DanceType a_DanceType)
        {
            // アニメーションのダンス種別を設定する
            p_AnimationsController.SetParamDanceType(a_DanceType);
        }

        /// <summary>
        /// リアクションポーズを設定する
        /// </summary>
        public void SetReactionPose(ReactionPose a_ReactionPose)
        {
            // アニメーションのリアクションポーズを設定する
            p_AnimationsController.SetParamReactionPose(a_ReactionPose);
        }
    }
}