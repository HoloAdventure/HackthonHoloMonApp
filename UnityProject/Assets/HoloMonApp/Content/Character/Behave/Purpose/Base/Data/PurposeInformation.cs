using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.Purpose
{
    [Serializable]
    public class PurposeInformation
    {
        /// <summary>
        /// ホロモンの目的情報
        /// </summary>
        [SerializeField]
        private PurposeDataInterface p_PurposeData;

        [SerializeField, Tooltip("ホロモン目的種別(確認用)")]
        private HoloMonPurposeType p_HoloMonPurposeType;

        /// <summary>
        /// ホロモン目的種別
        /// </summary>
        public HoloMonPurposeType HoloMonPurposeType => p_PurposeData.GetPurposeType();

        /// <summary>
        /// ホロモン目的個別データ(dynamic)
        /// </summary>
        public PurposeDataInterface PurposeData => p_PurposeData.GetPurposeData();

        public PurposeInformation(PurposeDataInterface a_Data)
        {
            p_PurposeData = a_Data;
            p_HoloMonPurposeType = a_Data.GetPurposeType();
        }
    }
}