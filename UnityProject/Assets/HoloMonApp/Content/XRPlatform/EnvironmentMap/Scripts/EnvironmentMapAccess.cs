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
    public class EnvironmentMapAccess
    {
#if XRPLATFORM_MRTK3
        private EnvironmentMapAccessMRTK3 p_EnvironmentMapAccess;
#elif XRPLATFORM_MRTKV2
        private EnvironmentMapAccessMRTKv2 p_EnvironmentMapAccess;
#endif

        public EnvironmentMapAccess(Component a_Component)
        {
#if XRPLATFORM_MRTK3
            p_EnvironmentMapAccess = new EnvironmentMapAccessMRTK3();
#elif XRPLATFORM_MRTKV2
            p_EnvironmentMapAccess = new EnvironmentMapAccessMRTKv2();
#endif
        }

    public int GetEnvironmentMapLayer()
        {
            int layer = -1;
#if XRPLATFORM_MRTK3
            layer = p_EnvironmentMapAccess.SpatialAwarenessLayer;
#elif XRPLATFORM_MRTKV2
            layer = p_EnvironmentMapAccess.SpatialAwarenessLayer;
#endif
            return layer;
        }
    }
}