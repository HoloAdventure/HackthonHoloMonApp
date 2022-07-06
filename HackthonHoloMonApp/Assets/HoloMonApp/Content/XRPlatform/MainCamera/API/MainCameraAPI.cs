using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.XRPlatform
{
    public class MainCameraAPI
    {
        public MainCameraAccess Access { get; }

        public MainCameraAPI(Component a_Component)
        {
            Access = new MainCameraAccess(a_Component);
        }
    }
}