#if XRPLATFORM_MRTKV2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CoreSystemへのアクセスのため
using Microsoft.MixedReality.Toolkit;
// 空間認識情報の取得のため
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

namespace HoloMonApp.Content.XRPlatform.MRTKv2
{
    public class EnvironmentMapAccessMRTKv2
    {
        public int SpatialAwarenessLayer;

        public EnvironmentMapAccessMRTKv2()
        {
            // 空間認識のオブザーバを取得する
            IMixedRealitySpatialAwarenessMeshObserver SpatialAwarenessMeshObserver =
                CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();

            // オブザーバからレイヤー番号を取得する
            SpatialAwarenessLayer = SpatialAwarenessMeshObserver.MeshPhysicsLayer;
        }
    }
}
#endif