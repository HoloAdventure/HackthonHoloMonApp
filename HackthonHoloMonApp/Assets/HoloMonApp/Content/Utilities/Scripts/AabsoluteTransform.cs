using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.UtilitiesSpace
{
    public class AabsoluteTransform : MonoBehaviour
    {
        [SerializeField, Tooltip("絶対ポジションの有効無効")]
        private bool EnablePosition = false;

        [SerializeField, Tooltip("絶対ローテーションの有効無効")]
        private bool EnableRotation = false;

        [SerializeField, Tooltip("絶対スケールの有効無効")]
        private bool EnableScale = false;

        [SerializeField, Tooltip("絶対ポジション")]
        private Vector3 AbsoluteWorldPosition = Vector3.zero;

        [SerializeField, Tooltip("絶対ローテーション")]
        private Vector3 AbsoluteWorldRotation = Vector3.zero;

        [SerializeField, Tooltip("絶対スケール")]
        private Vector3 AbsoluteWorldScale = Vector3.one;

        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {

        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            if(EnablePosition)
            {
                this.transform.position = AbsoluteWorldPosition;
            }
            if(EnableRotation)
            {
                this.transform.eulerAngles = AbsoluteWorldRotation;
            }
            if(EnableScale)
            {
                this.transform.localScale = new Vector3(
                    (AbsoluteWorldScale.x / this.transform.lossyScale.x) * this.transform.localScale.x,
                    (AbsoluteWorldScale.y / this.transform.lossyScale.y) * this.transform.localScale.y,
                    (AbsoluteWorldScale.z / this.transform.lossyScale.z) * this.transform.localScale.z
                    );
            }
        }
    }
}