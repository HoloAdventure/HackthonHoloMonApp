using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if XRPLATFORM_MRTK3
using HoloMonApp.Content.XRPlatform.MRTK3;
#elif XRPLATFORM_MRTKV2
using HoloMonApp.Content.XRPlatform.MRTKv2;
#endif

namespace HoloMonApp.Content.XRPlatform
{
    public class EnvironmentMapHandler
    {
#if XRPLATFORM_MRTK3
        //EnvironmentMapHandlerMRTK3 p_EnvironmentMapHandler;
#elif XRPLATFORM_MRTKV2
        EnvironmentMapHandlerMRTKv2 p_EnvironmentMapHandler;
#endif

        public EnvironmentMapHandler(Component a_Component)
        {
#if XRPLATFORM_MRTK3
            //p_EnvironmentMapHandler = new EnvironmentMapHandlerMRTK3();
            //p_EnvironmentMapHandler.OnAdded += OnObservationAdded;
            //p_EnvironmentMapHandler.OnRemoved += OnObservationRemoved;
            //p_EnvironmentMapHandler.OnUpdated += OnObservationUpdated;
#elif XRPLATFORM_MRTKV2
            p_EnvironmentMapHandler = new EnvironmentMapHandlerMRTKv2();
            p_EnvironmentMapHandler.OnAdded += OnObservationAdded;
            p_EnvironmentMapHandler.OnRemoved += OnObservationRemoved;
            p_EnvironmentMapHandler.OnUpdated += OnObservationUpdated;
#endif
        }

        public Action<EnvironmentMapEventData> OnAdded;
        public Action<EnvironmentMapEventData> OnRemoved;
        public Action<EnvironmentMapEventData> OnUpdated;

        public void OnObservationAdded(EnvironmentMapEventData eventData)
        {
            OnAdded?.Invoke(eventData);
        }

        public void OnObservationRemoved(EnvironmentMapEventData eventData)
        {
            OnRemoved?.Invoke(eventData);
        }

        public void OnObservationUpdated(EnvironmentMapEventData eventData)
        {
            OnUpdated?.Invoke(eventData);
        }
    }
}
