#if XRPLATFORM_MRTK3
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HoloMonApp.Content.XRPlatform.MRTK3
{
    public class SpeechInputHandlerMRTK3 : MonoBehaviour
    {
        [SerializeField]
        private List<SpeechCommandEvent> p_SpeechCommandEventList;

        void Start()
        {
            // Get the first running phrase recognition subsystem.
            PhraseRecognitionSubsystem phraseRecognitionSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<PhraseRecognitionSubsystem>();

            // If we found one...
            if (phraseRecognitionSubsystem != null)
            {
                foreach(SpeechCommandEvent speechCommandEvent in p_SpeechCommandEventList)
                {
                    string keyword = speechCommandEvent.Keyword;
                    UnityEvent kickEvent = speechCommandEvent.KickEvent;

                    // Register a phrase and its associated action with the subsystem
                    phraseRecognitionSubsystem.CreateOrGetEventForPhrase(keyword).AddListener(() => kickEvent.Invoke());
                }
            }
        }
        /*
        public UnityEvent testEvent;

        // Start is called before the first frame update
        void Start()
        {
            // Get the first running phrase recognition subsystem.
            var phraseRecognitionSubsystems = XRSubsystemHelpers.GetAllRunningSubsystems<PhraseRecognitionSubsystem>();

            foreach (var phraseRecognitionSubsystem in phraseRecognitionSubsystems)
            {
                // If we found one...
                if (phraseRecognitionSubsystem != null)
                {
                    // Register a phrase and its associated action with the subsystem
                    phraseRecognitionSubsystem.CreateOrGetEventForPhrase("じゃんけん").AddListener(() => testEvent.Invoke());
                    Debug.Log("Test set !!!!!!!!!!!!!!");
                }
            }
        }

        public void EventAwake()
        {
            Debug.Log("Phrase recognized !!!!!");
        }
        */
    }
}
#endif
