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
    /// <summary>
    /// カメラ参照の切り替えクラス
    /// </summary>
    public class MainCameraAccess
    {
#if XRPLATFORM_MRTK3
        private MainCameraAccessMRTK3 p_MainCameraAccess;
#elif XRPLATFORM_MRTKV2
        private MainCameraAccessMRTKv2 p_MainCameraAccess;
#endif

        public MainCameraAccess(Component a_Component)
        {
#if XRPLATFORM_MRTK3
            p_MainCameraAccess = new MainCameraAccessMRTK3();
#elif XRPLATFORM_MRTKV2
            p_MainCameraAccess = new MainCameraAccessMRTKv2();
#endif
    }


    public Camera GetMainCamera()
        {
            Camera camera = null;
#if XRPLATFORM_MRTK3
            camera = p_MainCameraAccess.GetMainCamera();
#elif XRPLATFORM_MRTKV2
            camera = p_MainCameraAccess.GetMainCamera();
#endif
            return camera;
        }
    }
}
