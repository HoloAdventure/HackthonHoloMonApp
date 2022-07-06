using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.XRPlatform
{
    public class EnvironmentMapAPI
    {
        public EnvironmentMapAccess Access { get; }

        public EnvironmentMapChange Change { get; }

        public EnvironmentMapHandler Handler { get; }

        public EnvironmentMapAPI(Component a_Component)
        {
            Access = new EnvironmentMapAccess(a_Component);
            Change = new EnvironmentMapChange(a_Component);
            Handler = new EnvironmentMapHandler(a_Component);
        }
    }
}