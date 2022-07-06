using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose.CatchBall
{
    [Serializable]
    public class PurposeCatchBallData : PurposeDataInterface
    {
        [SerializeField, Tooltip("ボールオブジェクト種別情報")]
        private ObjectFeatureWrap p_BallObjectData;

        /// <summary>
        /// ボールオブジェクト
        /// </summary>
        public GameObject BallObject => p_BallObjectData?.GameObject;

        public PurposeCatchBallData(ObjectFeatureWrap a_BallObjectData)
        {
            p_BallObjectData = a_BallObjectData;
        }

        public HoloMonPurposeType GetPurposeType()
        {
            return HoloMonPurposeType.CatchBall;
        }

        public PurposeDataInterface GetPurposeData()
        {
            return this;
        }
    }
}