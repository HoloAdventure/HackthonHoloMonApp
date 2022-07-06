#if XRPLATFORM_MRTKV2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CoreSystemへのアクセスのため
using Microsoft.MixedReality.Toolkit;
// InputSystem情報の取得のため
using Microsoft.MixedReality.Toolkit.Input;
// Handedness情報の取得のため
using Microsoft.MixedReality.Toolkit.Utilities;

namespace HoloMonApp.Content.XRPlatform.MRTKv2
{
    public class HandTrackingPointerMRTKv2
    {
        public HandTrackingPointerMRTKv2()
        {
        }

        public bool GetHandPointerTargetPose(HandTrackingHandedness a_Handedness, out Pose a_TargetPose)
        {
            bool result = false;

            result = GetHandPointerTargetPoseMRTKv2(DecordHandnessMRTKv2(a_Handedness), out a_TargetPose);

            return result;
        }

        #region "MRTKv2 private"
        private Handedness DecordHandnessMRTKv2(HandTrackingHandedness a_Handedness)
        {
            Handedness convertHandedness = Handedness.None;
            switch (a_Handedness)
            {
                case HandTrackingHandedness.None:
                    break;
                case HandTrackingHandedness.Left:
                    convertHandedness = Handedness.Left;
                    break;
                case HandTrackingHandedness.Right:
                    convertHandedness = Handedness.Right;
                    break;
                case HandTrackingHandedness.Both:
                    convertHandedness = Handedness.Both;
                    break;
                case HandTrackingHandedness.Other:
                    convertHandedness = Handedness.Other;
                    break;
                case HandTrackingHandedness.Any:
                    convertHandedness = Handedness.Any;
                    break;
            }
            return convertHandedness;
        }

        private bool GetHandPointerTargetPoseMRTKv2(Handedness a_TargetHandedness, out Pose a_TargetPose)
        {
            a_TargetPose = new Pose();

            ShellHandRayPointer handRayPointer = DetectionTargetHandRay(a_TargetHandedness);
            if (handRayPointer == null) return false;

            Vector3? position = handRayPointer?.BaseCursor?.Position;
            Quaternion? rotation = handRayPointer?.BaseCursor?.Rotation;
            if ((position == null) || (rotation == null)) return false;

            a_TargetPose = new Pose(position ?? Vector3.zero, rotation ?? Quaternion.identity);
            return true;
        }

        /// <summary>
        /// MRTKのInputSystemから監視対象のハンドレイポインターを取得する
        /// </summary>
        /// <returns></returns>
        private ShellHandRayPointer DetectionTargetHandRay(Handedness a_TargetHandedness)
        {
            ShellHandRayPointer handRayPointer = null;

            // 現在のInputSystemに対象が存在するかチェックする
            foreach (IMixedRealityInputSource inputSource in CoreServices.InputSystem.DetectedInputSources)
            {
                foreach (IMixedRealityPointer pointer in inputSource.Pointers)
                {
                    // ハンドレイでなければ対象外
                    if (pointer.GetType() != typeof(ShellHandRayPointer)) continue;
                    // 指定のハンドタイプでなければ対象外
                    if (((ShellHandRayPointer)pointer).Handedness != a_TargetHandedness) continue;

                    handRayPointer = (ShellHandRayPointer)pointer;
                }
            }

            return handRayPointer;
        }
        #endregion
    }
}
#endif