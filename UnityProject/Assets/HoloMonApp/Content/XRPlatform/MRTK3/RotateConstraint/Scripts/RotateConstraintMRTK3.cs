using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.XRPlatform.MRTK3
{
    public class RotateConstraintMRTK3 : MonoBehaviour
    {
        private enum RotateType
        {
            X, Y, Z,
        }

        [SerializeField]
        private RotateType p_RotateType;

        // Update is called once per frame
        void LateUpdate()
        {
            switch (p_RotateType)
            {
                case RotateType.X:
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 0.0f, 0.0f);
                    break;
                case RotateType.Y:
                    this.transform.eulerAngles = new Vector3(0.0f, this.transform.eulerAngles.y, 0.0f);
                    break;
                case RotateType.Z:
                    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.transform.eulerAngles.z);
                    break;
            }
        }
    }
}
