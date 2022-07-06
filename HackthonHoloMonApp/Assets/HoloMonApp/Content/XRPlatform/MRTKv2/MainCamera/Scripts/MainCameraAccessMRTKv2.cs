#if XRPLATFORM_MRTKV2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraCacheへのアクセスのため
using Microsoft.MixedReality.Toolkit.Utilities;

namespace HoloMonApp.Content.XRPlatform.MRTKv2
{
    public class MainCameraAccessMRTKv2
    {
        public MainCameraAccessMRTKv2()
        { }

        public Camera GetMainCamera()
        {
            return CameraCache.Main;
        }
    }
}
#endif