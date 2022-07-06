using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace HoloMonApp.Content.Character.Data.Animations.Body
{
    /// <summary>
    /// ボディアニメーションDATA
    /// </summary>
    public class HoloMonAnimationsBodyData : MonoBehaviour
    {
        [SerializeField, Tooltip("全体(Body)アニメーションの参照")]
        private Animator p_Body;
        public Animator Body => p_Body;


        /// <summary>
        /// ホロモンアニメーション情報
        /// </summary>
        [SerializeField, Tooltip("ホロモンアニメーション情報")]
        private HoloMonAnimationsBodyRawData p_HoloMonAnimationInfo;
        public HoloMonAnimationsBodyRawData AnimationInfo => p_HoloMonAnimationInfo;


        // アニメーションのステート名(BaseLayer)
        const string state_StandbyWait = "Base Layer.StandbyWait";

        const string state_LookAroundMode = "Base Layer.LookAroundMode";
        const string state_SleepMode = "Base Layer.SleepMode";
        const string state_ShitPutoutMode = "Base Layer.ShitPutoutMode";
        const string state_SitDownMode = "Base Layer.SitDownMode";
        const string state_FoodMealMode = "Base Layer.FoodMealMode";

        // アニメーションのパラメータ名
        const string param_Awake = "Bool_Awake";

        const string param_LookAroundMode = "Bool_LookAroundMode";
        const string param_TrackingMode = "Bool_TrackingMode";
        const string param_TurnMode = "Bool_TurnMode";
        const string param_SitDownMode = "Bool_SitDownMode";
        const string param_JankenMode = "Bool_JankenMode";
        const string param_FoodMealMode = "Bool_FoodMealMode";
        const string param_SleepMode = "Bool_SleepMode";
        const string param_DanceMode = "Bool_DanceMode";
        const string param_ShitPutoutMode = "Bool_ShitPutoutMode";
        const string param_ReactionMode = "Bool_ReactionMode";
        const string param_FriendLookMode = "Bool_FriendLookMode";
        const string param_HungUpMode = "Bool_HungUpMode";

        const string param_AnimationMode = "Int_AnimationMode";

        const string param_Speed = "Float_Speed";
        const string param_Rotate = "Float_Rotate";

        const string param_HoldItemOption = "Bool_HoldItemOption";

        const string param_JankenPose = "Int_JankenPose";
        const string param_SleepPose = "Int_SleepPose";
        const string param_JankenResult = "Int_JankenResult";
        const string param_DanceType = "Int_DanceType";
        const string param_ReactionType = "Int_ReactionPose";
        const string param_FriendLookType = "Int_FriendLookPose";

        // アニメーションパラメータの設定関数
        public void ApplyRawData(HoloMonAnimationsBodyRawData a_HoloMonAnimationInfo)
        {

        }

        public void SetParamAnimationMode(AnimationMode a_AnimationMode)
        {
            p_HoloMonAnimationInfo.AnimationMode = a_AnimationMode;
            p_Body.SetInteger(param_AnimationMode, (int)a_AnimationMode);
        }

        public void CancelParamAnimationMode(AnimationMode a_AnimationMode)
        {

        }

        public void SetParamLookAroundMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.LookAround : AnimationMode.Standby);
        }
        public void SetParamTrackingMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.Tracking : AnimationMode.Standby);
        }
        public void SetParamTurnMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.Turn : AnimationMode.Standby);
        }
        public void SetParamSitDownMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.SitDown : AnimationMode.Standby);
        }
        public void SetParamJankenMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.Janken : AnimationMode.Standby);
        }
        public void SetParamFoodMealMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.FoodMeal : AnimationMode.Standby);
        }
        public void SetParamSleepMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.Sleep : AnimationMode.Standby);
        }
        public void SetParamDanceMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.Dance : AnimationMode.Standby);
        }
        public void SetParamPoopPutoutMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.PoopPutout : AnimationMode.Standby);
        }
        public void SetParamHungUpMode(bool a_bool)
        {
            SetParamAnimationMode(a_bool ? AnimationMode.HungUp : AnimationMode.Standby);
        }

        public void SetParamSpeed(float a_float)
        {
            p_HoloMonAnimationInfo.Speed = a_float;
            p_Body.SetFloat(param_Speed, a_float);
        }
        public void SetParamRotate(float a_float)
        {
            p_HoloMonAnimationInfo.Rotate = a_float;
            p_Body.SetFloat(param_Rotate, a_float);
        }
        public void SetParamHoldItemOption(bool a_bool)
        {
            p_HoloMonAnimationInfo.HoldItemOption = a_bool;
            p_Body.SetBool(param_HoldItemOption, a_bool);
        }
        public void SetParamJankenPose(JankenPose a_pose)
        {
            p_HoloMonAnimationInfo.JankenPose = a_pose;
            p_Body.SetInteger(param_JankenPose, (int)a_pose);
        }
        public void SetParamSleepPose(SleepPose a_pose)
        {
            p_HoloMonAnimationInfo.SleepPose = a_pose;
            p_Body.SetInteger(param_SleepPose, (int)a_pose);
        }
        public void SetParamJankenResult(JankenResult a_result)
        {
            p_HoloMonAnimationInfo.JankenResult = a_result;
            p_Body.SetInteger(param_JankenResult, (int)a_result);
        }
        public void SetParamDanceType(DanceType a_type)
        {
            p_HoloMonAnimationInfo.DanceTyep = a_type;
            p_Body.SetInteger(param_DanceType, (int)a_type);
        }
        public void SetParamReactionPose(ReactionPose a_pose)
        {
            p_HoloMonAnimationInfo.ReactionPose = a_pose;
            p_Body.SetInteger(param_ReactionType, (int)a_pose);
        }

        /*
        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // 現在のアニメーションステート情報を取得する
            AnimatorStateInfo currentAnimpLayer00 = p_Animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo currentAnimpLayer01 = p_Animator.GetCurrentAnimatorStateInfo(1);
            AnimatorStateInfo currentAnimpLayer02 = p_Animator.GetCurrentAnimatorStateInfo(2);

            // レイヤー0でステート遷移中か否か
            if(p_Animator.IsInTransition(0))
            {

            }

            // 取得したステート情報がベースレイヤーのStandbyWaitか否か
            if(currentAnimpLayer00.IsName("Base Layer.StandbyWait"))
            {

            }

            // 取得したステート情報のタグがStandbyか否か
            if (currentAnimpLayer00.IsTag("Standby"))
            {

            }
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
        }
        */
    }
}