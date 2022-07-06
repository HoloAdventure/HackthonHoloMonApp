using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Behave.ModeLogic.TrackingTargetNavMesh
{
    [Serializable]
    public class ModeLogicTrackingTargetNavMeshReturn : ModeLogicReturnInterface
    {
        public HoloMonActionMode GetActionMode()
        {
            return HoloMonActionMode.TrackingTargetNavMesh;
        }

        public ModeLogicReturnInterface GetModeLogicReturn()
        {
            return this;
        }
    }
}
