using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.UtilitiesSpace
{
    public class TrackingTarget : MonoBehaviour
    {
        [SerializeField, Tooltip("追跡対象トランスフォーム")]
        private Transform p_TargetTransform;

        // Start is called before the first frame update
        void Start()
        {
            Tracking();
        }

        // Update is called once per frame
        void Update()
        {
            Tracking();
        }

        private void Tracking()
        {
            this.transform.position = p_TargetTransform.position;
            this.transform.rotation = p_TargetTransform.rotation;
        }
    }
}
