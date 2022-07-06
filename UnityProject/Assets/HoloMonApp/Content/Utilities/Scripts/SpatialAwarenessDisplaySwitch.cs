using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.UtilitiesSpace
{
    public class SpatialAwarenessDisplaySwitch : MonoBehaviour
    {
        public void OnToggleMapMesh()
        {
            HoloMonXRPlatform.Instance.EnvironmentMap.Change.OnToggleMapMesh();
        }
    }
}