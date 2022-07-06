using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Data.Transforms.Body;
using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToBillBoard
{
    public class HoloMonBillboard : MonoBehaviour
    {
        public enum PivotAxisType
        {
            XY = 0,
            Y = 1,
            X = 2,
            Z = 3,
            XZ = 4,
            YZ = 5,
            Free = 6
        }

        /// <summary>
        /// The axis about which the object will rotate.
        /// </summary>
        public PivotAxisType PivotAxis
        {
            get { return pivotAxis; }
            set { pivotAxis = value; }
        }

        private Camera TargetMainCamera => HoloMonXRPlatform.Instance.MainCamera.Access.GetMainCamera();

        [SerializeField, Tooltip("ボディトランスフォームの参照")]
        private HoloMonTransformsBodyData p_HoloMonTransformsBodyData;

        [Tooltip("Specifies the axis about which the object will rotate.")]
        [SerializeField]
        private PivotAxisType pivotAxis = PivotAxisType.XY;

        // 有効状態
        public bool isActive = false;

        // Y軸反転
        public bool isYAxisInverse = false;

        // 線形補間用のパラメータ
        public float lerpFloat = 0.05f;

        // 現在正面を向いているか
        [SerializeField, Tooltip("現在正面を向いているか")]
        private bool isFront = false;

        // 正面判定の敷居値(度数)
        [SerializeField, Tooltip("正面判定の敷居値(度数)")]
        private float isFrontThreshold = 1.0f;

        // ターゲットとの相対角度
        [SerializeField, Tooltip("ターゲットとの相対角度")]
        private float diffTargetAngle = 0.0f;

        /// <summary>
        /// The target we will orient to. If no target is specified, the main camera will be used.
        /// </summary>
        public Transform TargetTransform
        {
            get { return targetTransform; }
            set { targetTransform = value; }
        }

        [Tooltip("Specifies the target we will orient to. If no target is specified, the main camera will be used.")]
        [SerializeField]
        private Transform targetTransform;

        private void Start()
        {
        }

        private void OnEnable()
        {
            if (targetTransform == null)
            {
                targetTransform = TargetMainCamera.transform;
            }
        }

        /// <summary>
        /// Keeps the object facing the camera.
        /// </summary>
        private void Update()
        {
            if (targetTransform == null)
            {
                return;
            }

            // Get a Vector that points from the target to the main camera.
            Vector3 directionToTarget = targetTransform.position - p_HoloMonTransformsBodyData.Root.position;

            bool useCameraAsUpVector = true;

            // Adjust for the pivot axis.
            switch (pivotAxis)
            {
                case PivotAxisType.X:
                    directionToTarget.x = 0.0f;
                    useCameraAsUpVector = false;
                    break;

                case PivotAxisType.Y:
                    directionToTarget.y = 0.0f;
                    useCameraAsUpVector = false;
                    break;

                case PivotAxisType.Z:
                    directionToTarget.x = 0.0f;
                    directionToTarget.y = 0.0f;
                    break;

                case PivotAxisType.XY:
                    useCameraAsUpVector = false;
                    break;

                case PivotAxisType.XZ:
                    directionToTarget.x = 0.0f;
                    break;

                case PivotAxisType.YZ:
                    directionToTarget.y = 0.0f;
                    break;

                case PivotAxisType.Free:
                default:
                    // No changes needed.
                    break;
            }

            // If we are right next to the camera the rotation is undefined. 
            if (directionToTarget.sqrMagnitude < 0.001f)
            {
                return;
            }

            Quaternion lookAtRotation = new Quaternion();
            // Calculate and apply the rotation required to reorient the object
            if (useCameraAsUpVector)
            {
                lookAtRotation = Quaternion.LookRotation(-directionToTarget, TargetMainCamera.transform.up);
            }
            else
            {
                lookAtRotation = Quaternion.LookRotation(-directionToTarget);
            }

            // 後ろ方向(Y軸反転)を向く
            if(isYAxisInverse)
            {
                lookAtRotation.eulerAngles = new Vector3(
                    lookAtRotation.eulerAngles.x,
                    lookAtRotation.eulerAngles.y + 180.0f,
                    lookAtRotation.eulerAngles.z
                );
            }

            // Lerpを利用した有効無効の切り替え
            if (isActive)
            {
                // 線形補間用のパラメータ
                p_HoloMonTransformsBodyData.Root.rotation = Quaternion.Lerp(p_HoloMonTransformsBodyData.Root.rotation, lookAtRotation, lerpFloat);
            }

            // 現在のターゲットとの相対角度を取得する
            diffTargetAngle = Quaternion.Angle(p_HoloMonTransformsBodyData.Root.rotation, lookAtRotation);

            // 現在正面を向いているか否か
            if (diffTargetAngle < isFrontThreshold)
            {
                isFront = true;
            }
            else
            {
                isFront = false;
            }
        }

        /// <summary>
        /// 現在正面を向いているか否か
        /// </summary>
        /// <returns></returns>
        public bool IsFront()
        {
            return isFront;
        }

        /// <summary>
        /// 現在のターゲットとの相対角度を取得する
        /// </summary>
        /// <returns></returns>
        public float CurrentDiffTargetAngle()
        {
            return diffTargetAngle;
        }
    }
}