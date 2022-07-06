using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh
{
    [Serializable]
    public class ModeLogicTrackingTargetNavMeshData : ModeLogicDataInterface
    {
        [SerializeField, Tooltip("追跡オブジェクト")]
        private GameObject p_TargetObject;

        /// <summary>
        /// 追跡オブジェクト
        /// </summary>
        public GameObject TargetObject => p_TargetObject;

        [SerializeField, Tooltip("追跡設定")]
        private TrackingSettingNavMesh p_TrackingSetting;

        /// <summary>
        /// 追跡設定
        /// </summary>
        public TrackingSettingNavMesh TrackingSetting => p_TrackingSetting;

        [SerializeField, Tooltip("アイテム保持設定")]
        private HoldItemSetting p_HoldItemSetting;

        /// <summary>
        /// アイテム保持設定
        /// </summary>
        public HoldItemSetting HoldItemSetting => p_HoldItemSetting;

        public ModeLogicTrackingTargetNavMeshData(GameObject a_TargetObject,
            TrackingSettingNavMesh a_TrackingSetting, HoldItemSetting a_HoldItemSetting)
        {
            p_TargetObject = a_TargetObject;
            p_TrackingSetting = a_TrackingSetting;
            p_HoldItemSetting = a_HoldItemSetting;
        }

        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.TrackingTargetNavMesh;
        }

        public ModeLogicDataInterface GetModeLogicData()
        {
            return this;
        }
    }
}
