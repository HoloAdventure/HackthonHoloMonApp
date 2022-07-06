using System;
using UnityEngine.Events;

namespace HoloMonApp.Content.XRPlatform.MRTK3
{
    [Serializable]
    public class SpeechCommandEvent
    {
        public string Keyword;

        public UnityEvent KickEvent;
    }
}
