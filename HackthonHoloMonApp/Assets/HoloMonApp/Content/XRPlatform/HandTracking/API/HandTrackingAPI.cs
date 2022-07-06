using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.XRPlatform
{
    public class HandTrackingAPI
    {
        public HandTrackingHandler Handler { get; }

        public HandTrackingPointer Pointer { get; }

        public HandTrackingAPI(Component a_Component)
        {
            Handler = new HandTrackingHandler(a_Component);
            Pointer = new HandTrackingPointer(a_Component);
        }
    }
}