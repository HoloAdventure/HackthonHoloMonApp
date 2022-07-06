using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Model.Animations.Body;
using HoloMonApp.Content.Character.Data.Animations.Body;

namespace HoloMonApp.Content.Character.Control.Animations.Body
{
    /// <summary>
    /// アニメーションAPI
    /// </summary>
    public class HoloMonControlAnimationsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonAnimationsBodyAPI p_AnimationsAPI;


        /// <summary>
        /// アニメーションをスタンバイモードに設定する
        /// </summary>
        public void ReturnStandbyMode()
        {
            p_AnimationsAPI.ReturnStandbyMode();
        }

        /// <summary>
        /// アニメーションを見回すモードに設定する
        /// </summary>
        public void StartLookAroundMode()
        {
            p_AnimationsAPI.StartLookAroundMode();
        }

        /// <summary>
        /// アニメーションを追跡モードに設定する
        /// </summary>
        public void StartTrackingMode()
        {
            p_AnimationsAPI.StartTrackingMode();
        }

        /// <summary>
        /// アニメーションを回転モードに設定する
        /// </summary>
        public void StartTurnMode()
        {
            p_AnimationsAPI.StartTurnMode();
        }

        /// <summary>
        /// アニメーションをおすわりモードに設定する
        /// </summary>
        public void StartSitDownMode()
        {
            p_AnimationsAPI.StartSitDownMode();
        }

        /// <summary>
        /// アニメーションをじゃんけんモードに設定する
        /// </summary>
        public void StartJankenMode()
        {
            p_AnimationsAPI.StartJankenMode();
        }

        /// <summary>
        /// アニメーションを食事モードに設定する
        /// </summary>
        public void StartFoodMealMode()
        {
            p_AnimationsAPI.StartFoodMealMode();
        }

        /// <summary>
        /// アニメーションを眠りモードに設定する
        /// </summary>
        public void StartSleepMode()
        {
            p_AnimationsAPI.StartSleepMode();
        }

        /// <summary>
        /// アニメーションをダンスモードに設定する
        /// </summary>
        public void StartDanceMode()
        {
            p_AnimationsAPI.StartDanceMode();
        }

        /// <summary>
        /// アニメーションをうんちモードに設定する
        /// </summary>
        public void StartPoopPutoutMode()
        {
            p_AnimationsAPI.StartPoopPutoutMode();
        }

        /// <summary>
        /// アニメーションを掴まれモードに設定する
        /// </summary>
        public void StartHungUpMode()
        {
            p_AnimationsAPI.StartHungUpMode();
        }


        /// <summary>
        /// モデルの現在速度を設定する
        /// </summary>
        public void SetModelSpeed(float a_Speed)
        {
            p_AnimationsAPI.SetModelSpeed(a_Speed);
        }

        /// <summary>
        /// モデルの現在回転速度(1秒あたりの度数)を設定する
        /// </summary>
        public void SetModelRotate(float a_Rotate)
        {
            p_AnimationsAPI.SetModelRotate(a_Rotate);
        }


        /// <summary>
        /// じゃんけんポーズを設定する
        /// </summary>
        public void SetJankenPose(JankenPose a_JankenPose)
        {
            p_AnimationsAPI.SetJankenPose(a_JankenPose);
        }

        public void SetSleepPose(SleepPose a_SleepPose)
        {
            p_AnimationsAPI.SetSleepPose(a_SleepPose);
        }

        /// <summary>
        /// じゃんけん結果を設定する
        /// </summary>
        public void SetJankenResult(JankenResult a_JankenResult)
        {
            p_AnimationsAPI.SetJankenResult(a_JankenResult);
        }


        /// <summary>
        /// ダンス種別を設定する
        /// </summary>
        public void SetDanceType(DanceType a_DanceType)
        {
            p_AnimationsAPI.SetDanceType(a_DanceType);
        }

        /// <summary>
        /// リアクションポーズを設定する
        /// </summary>
        public void SetReactionPose(ReactionPose a_ReactionPose)
        {
            p_AnimationsAPI.SetReactionPose(a_ReactionPose);
        }

        /// <summary>
        /// アニメーションのアイテム保持オプションを変更する
        /// </summary>
        public void ChangeHoldItemOption(bool a_OnOff)
        {
            p_AnimationsAPI.ChangeHoldItemOption(a_OnOff);
        }
    }
}