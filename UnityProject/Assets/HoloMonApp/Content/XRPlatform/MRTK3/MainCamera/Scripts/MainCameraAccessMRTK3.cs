#if XRPLATFORM_MRTK3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraCacheへのアクセスのため
using Microsoft.MixedReality.Toolkit;

namespace HoloMonApp.Content.XRPlatform.MRTK3
{
    public class MainCameraAccessMRTK3
    {
        public MainCameraAccessMRTK3(){ }

        public Camera GetMainCamera()
        {
            return CameraCache.Main;
        }
    }
}
#endif
