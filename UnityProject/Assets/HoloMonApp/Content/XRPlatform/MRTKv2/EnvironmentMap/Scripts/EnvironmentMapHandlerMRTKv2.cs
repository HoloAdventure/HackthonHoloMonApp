#if XRPLATFORM_MRTKV2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// CoreSystemへのアクセスのため
using Microsoft.MixedReality.Toolkit;
// 空間認識情報の取得のため
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

namespace HoloMonApp.Content.XRPlatform.MRTKv2
{
    using SpatialAwarenessHandler = IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessMeshObject>;

    public class EnvironmentMapHandlerMRTKv2 : SpatialAwarenessHandler
    {
        public EnvironmentMapHandlerMRTKv2()
        {
            CoreServices.SpatialAwarenessSystem?.RegisterHandler<SpatialAwarenessHandler>(this);
        }

        public Action<EnvironmentMapEventData> OnAdded;
        public Action<EnvironmentMapEventData> OnRemoved;
        public Action<EnvironmentMapEventData> OnUpdated;

        public void OnObservationAdded(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            EnvironmentMapEventData data = new EnvironmentMapEventData {
                Id = eventData.Id,
                MapObject = eventData.SpatialObject.GameObject,
            };
            OnAdded?.Invoke(data);
        }

        public void OnObservationRemoved(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            EnvironmentMapEventData data = new EnvironmentMapEventData
            {
                Id = eventData.Id,
                MapObject = eventData.SpatialObject.GameObject,
            };
            OnRemoved?.Invoke(data);
        }

        public void OnObservationUpdated(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            EnvironmentMapEventData data = new EnvironmentMapEventData
            {
                Id = eventData.Id,
                MapObject = eventData.SpatialObject.GameObject,
            };
            OnUpdated?.Invoke(data);
        }
    }
}
#endif