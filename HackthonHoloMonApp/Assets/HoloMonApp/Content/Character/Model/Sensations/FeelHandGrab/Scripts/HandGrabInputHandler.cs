using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.Sensations.FeelHandGrab
{
    public class HandGrabInputHandler : MonoBehaviour
    {
        [SerializeField, Tooltip("ハンドグラブ状態の参照")]
        private HoloMonFeelHandGrabAPI p_HoloMonHandGrabFeelSingleton;

        /// <summary>
        /// ハンドグラブの検出
        /// (現状は ObjectManipulator からの呼び出しで実行)
        /// </summary>
        /// <param name="a_IsGrabbed"></param>
        public void NoticeHandGrab(bool a_IsGrabbed)
        {
            p_HoloMonHandGrabFeelSingleton.HandGrabFeelHoloMon(a_IsGrabbed);
        }
    }
}