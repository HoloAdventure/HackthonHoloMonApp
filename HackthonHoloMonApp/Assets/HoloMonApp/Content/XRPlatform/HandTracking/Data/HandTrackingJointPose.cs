using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.XRPlatform
{
    public class HandTrackingJointPose
    {
        public HandTrackingJointPose(Vector3 position)
        {
            Posing = new Pose(position, Quaternion.identity);
        }
        public HandTrackingJointPose(Quaternion rotation)
        {
            Posing = new Pose(Vector3.zero, rotation);
        }
        public HandTrackingJointPose(Vector3 position, Quaternion rotation)
        {
            Posing = new Pose(position, rotation);
        }

        public static HandTrackingJointPose ZeroIdentity { get; }
        public Pose Posing { get; }
        public Vector3 Position => Posing.position;
        public Quaternion Rotation => Posing.rotation;
        public Vector3 Forward => Posing.forward; 
        public Vector3 Up => Posing.up;
        public Vector3 Right => Posing.right;

        public bool Equals(HandTrackingJointPose obj)
        {
            return Posing.Equals(obj.Posing);
        }
    }
}
