using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if XRPLATFORM_MRTK3
using HoloMonApp.Content.XRPlatform.MRTK3;
#elif XRPLATFORM_MRTKV2
using HoloMonApp.Content.XRPlatform.MRTKv2;
#endif

namespace HoloMonApp.Content.XRPlatform
{
    public class HandTrackingPointer
    {
#if XRPLATFORM_MRTK3
        //HandTrackingPointerMRTK3 p_HandTrackingPointer;
#elif XRPLATFORM_MRTKV2
        HandTrackingPointerMRTKv2 p_HandTrackingPointer;
#endif

        public HandTrackingPointer(Component a_Component)
        {
#if XRPLATFORM_MRTK3
            //p_HandTrackingPointer = new HandTrackingPointerMRTK3();
#elif XRPLATFORM_MRTKV2
            p_HandTrackingPointer = new HandTrackingPointerMRTKv2();
#endif
        }

        public bool GetHandPointerTargetPose(HandTrackingHandedness a_Handedness, out Pose a_TargetPose)
        {
            bool result = false;
#if XRPLATFORM_MRTK3
            // TODO
            a_TargetPose = new Pose();
#elif XRPLATFORM_MRTKV2
            result = p_HandTrackingPointer.GetHandPointerTargetPose(a_Handedness, out a_TargetPose);
#else
            a_TargetPose = new Pose();
#endif
            return result;
        }
    }
}
