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
    public class HoloMonAnimationsController : MonoBehaviour
    {
        /// <summary>
        /// ホロモンアニメーションデータ
        /// </summary>
        [SerializeField]
        private HoloMonAnimationsBodyData p_AnimationsBodyData;

        // アニメーションパラメータの設定関数
        public void SetParamLookAroundMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamLookAroundMode(a_bool);
        }
        public void SetParamTrackingMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamTrackingMode(a_bool);
        }
        public void SetParamTurnMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamTurnMode(a_bool);
        }
        public void SetParamSitDownMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamSitDownMode(a_bool);
        }
        public void SetParamJankenMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamJankenMode(a_bool);
        }
        public void SetParamFoodMealMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamFoodMealMode(a_bool);
        }
        public void SetParamSleepMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamSleepMode(a_bool);
        }
        public void SetParamDanceMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamDanceMode(a_bool);
        }
        public void SetParamPoopPutoutMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamPoopPutoutMode(a_bool);
        }
        public void SetParamHungUpMode(bool a_bool)
        {
            p_AnimationsBodyData.SetParamHungUpMode(a_bool);
        }
        public void SetParamHoldItemOption(bool a_bool)
        {
            p_AnimationsBodyData.SetParamHoldItemOption(a_bool);
        }

        public void SetParamSpeed(float a_float)
        {
            p_AnimationsBodyData.SetParamSpeed(a_float);
        }
        public void SetParamRotate(float a_float)
        {
            p_AnimationsBodyData.SetParamRotate(a_float);
        }
        public void SetParamJankenPose(JankenPose a_pose)
        {
            p_AnimationsBodyData.SetParamJankenPose(a_pose);
        }
        public void SetParamSleepPose(SleepPose a_pose)
        {
            p_AnimationsBodyData.SetParamSleepPose(a_pose);
        }
        public void SetParamJankenResult(JankenResult a_result)
        {
            p_AnimationsBodyData.SetParamJankenResult(a_result);
        }
        public void SetParamDanceType(DanceType a_type)
        {
            p_AnimationsBodyData.SetParamDanceType(a_type);
        }
        public void SetParamReactionPose(ReactionPose a_pose)
        {
            p_AnimationsBodyData.SetParamReactionPose(a_pose);
        }
    }
}