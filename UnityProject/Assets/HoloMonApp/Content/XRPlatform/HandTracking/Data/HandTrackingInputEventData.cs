using System.Collections.Generic;

namespace HoloMonApp.Content.XRPlatform
{
    public class HandTrackingInputEventData
    {
        public uint SourceId { get; }

        public HandTrackingHandedness Handedness { get; }

        public IDictionary<HandTrackingHandJoint, HandTrackingJointPose> InputData;

        public HandTrackingInputEventData(uint a_SourceId, HandTrackingHandedness a_Handedness,
            IDictionary<HandTrackingHandJoint, HandTrackingJointPose> a_InputData)
        {
            SourceId = a_SourceId;
            Handedness = a_Handedness;
            InputData = a_InputData;
        }
    }
}