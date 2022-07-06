using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.SitDown
{
    [Serializable]
    public class ModeLogicSitDownData : ModeLogicDataInterface
    {
        [SerializeField, Tooltip("注目オブジェクト")]
        private GameObject p_LookObject;

        /// <summary>
        /// 注目オブジェクト
        /// </summary>
        public GameObject LookObject => p_LookObject;

        [SerializeField, Tooltip("待機の近距離解除の距離設定（-1 時チェックしない）")]
        private float p_CheckNearDistance;

        /// <summary>
        /// 待機の近距離解除の距離設定（-1 時チェックしない）
        /// </summary>
        public float CheckNearDistance => p_CheckNearDistance;

        [SerializeField, Tooltip("待機の遠距離解除の距離設定（-1 時チェックしない）")]
        private float p_CheckFarDistance;

        /// <summary>
        /// 待機の遠距離解除の距離設定（-1 時チェックしない）
        /// </summary>
        public float CheckFarDistance => p_CheckFarDistance;

        [SerializeField, Tooltip("待機の合図解除のチェック有無")]
        private bool p_CheckReleaseSignal;

        /// <summary>
        /// 待機の合図解除のチェック有無
        /// </summary>
        public bool CheckReleaseSignal => p_CheckReleaseSignal;

        public ModeLogicSitDownData(
            GameObject a_LookObject,
            float a_CheckNearDistance,
            float a_CheckFarDistance,
            bool a_CheckReleaseSignal)
        {
            p_LookObject = a_LookObject;
            p_CheckNearDistance = a_CheckNearDistance;
            p_CheckFarDistance = a_CheckFarDistance;
            p_CheckReleaseSignal = a_CheckReleaseSignal;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.SitDown;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
