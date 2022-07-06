using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTransform : MonoBehaviour
{
    [SerializeField, Tooltip("追跡対象のトランスフォーム")]
    private Transform p_TargetTransform;

    private void LateUpdate()
    {
        this.transform.position = p_TargetTransform.position;
        this.transform.rotation = p_TargetTransform.rotation;
        this.transform.localScale = p_TargetTransform.localScale;
    }
}
