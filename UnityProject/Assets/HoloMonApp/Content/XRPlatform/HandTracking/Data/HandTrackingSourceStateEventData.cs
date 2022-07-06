using System;

namespace HoloMonApp.Content.XRPlatform
{
    public class HandTrackingSourceStateEventData
    {
        public DateTime EventTime { get; }
        public uint SourceId { get; }

        public HandTrackingSourceStateEventData(DateTime a_EventTime, uint a_SourceId)
        {
            EventTime = a_EventTime;
            SourceId = a_SourceId;
        }
    }
}