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
    public class EnvironmentMapChange
    {
#if XRPLATFORM_MRTK3
        //EnvironmentMapChangeMRTK3 p_EnvironmentMapChange;
#elif XRPLATFORM_MRTKV2
        EnvironmentMapChangeMRTKv2 p_EnvironmentMapChange;
#endif

        public EnvironmentMapChange(Component a_Component)
        {
#if XRPLATFORM_MRTK3
            //p_EnvironmentMapChange = new EnvironmentMapChangeMRTK3();
#elif XRPLATFORM_MRTKV2
            p_EnvironmentMapChange = new EnvironmentMapChangeMRTKv2();
#endif
        }

        public void OnToggleMapMesh()
        {
#if XRPLATFORM_MRTK3
            // TODO
#elif XRPLATFORM_MRTKV2
            p_EnvironmentMapChange.OnToggleMapMesh();
#endif
        }
    }
}