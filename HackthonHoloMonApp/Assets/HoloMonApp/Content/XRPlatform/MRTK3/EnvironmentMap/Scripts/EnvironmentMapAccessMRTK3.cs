#if XRPLATFORM_MRTK3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.XRPlatform.MRTK3
{
    public class EnvironmentMapAccessMRTK3
    {
        public int SpatialAwarenessLayer;

        public EnvironmentMapAccessMRTK3()
        {
            // 空間認識のオブザーバを取得する
            //IMixedRealitySpatialAwarenessMeshObserver SpatialAwarenessMeshObserver =
            //    CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();

            // オブザーバからレイヤー番号を取得する
            //SpatialAwarenessLayer = SpatialAwarenessMeshObserver.MeshPhysicsLayer;
            SpatialAwarenessLayer = 31;
        }
    }
}
#endif
