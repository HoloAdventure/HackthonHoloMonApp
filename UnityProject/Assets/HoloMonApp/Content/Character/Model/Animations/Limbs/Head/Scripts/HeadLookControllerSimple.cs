using System.Collections;
using UnityEngine;

using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head
{
	/// <summary>
	/// HeadLookControllerを線形補完を利用して修正
	/// </summary>
	public class HeadLookControllerSimple : MonoBehaviour
	{
		[SerializeField, Tooltip("ボディトランスフォームの参照")]
		private HoloMonTransformsBodyData p_HoloMonTransformsBodyData;

		private Transform rootNode => p_HoloMonTransformsBodyData.Root;
		private Transform firstBone => p_HoloMonTransformsBodyData.Head;
		private Transform lastBone => p_HoloMonTransformsBodyData.Head;

		public SimpleSegment segment;
		public NonAffectedJoints[] nonAffectedJoints;
		public Vector3 headLookVector = Vector3.forward;
		public Vector3 headUpVector = Vector3.up;
		public Vector3 target = Vector3.zero;
		public float effect = 1;
		public bool overrideAnimation = false;

		// overrideAnimationの線形補間用のパラメータ
		public float lerpFloat = 0.0f;
		public float lerpFrameDiff = 0.01f;
        public float lerpFloatMax = 1.0f;

		/// <summary>
		/// 起動処理
		/// </summary>
		void Start()
		{
			Quaternion parentRot = firstBone.parent.rotation;
			Quaternion parentRotInv = Quaternion.Inverse(parentRot);
			segment.referenceLookDir =
				parentRotInv * rootNode.rotation * headLookVector.normalized;
			segment.referenceUpDir =
				parentRotInv * rootNode.rotation * headUpVector.normalized;
			segment.angleH = 0;
			segment.angleV = 0;
			segment.dirUp = segment.referenceUpDir;

			segment.chainLength = 1;
			Transform t = lastBone;
			while (t != firstBone && t != t.root)
			{
				segment.chainLength++;
				t = t.parent;
			}

			segment.origRotations = new Quaternion[segment.chainLength];
			t = lastBone;
			for (int i = segment.chainLength - 1; i >= 0; i--)
			{
				segment.origRotations[i] = t.localRotation;
				t = t.parent;
			}
			if (overrideAnimation)
			{
				// overrideAnimationの線形補間用のパラメータ
				// ON の場合は補正を完全に有効化する
				lerpFloat = lerpFloatMax;
			}
			else
			{
				// overrideAnimationの線形補間用のパラメータ
				// OFF の場合は補正を完全に無効化する
				lerpFloat = 0.0f;
			}
		}

		/// <summary>
		/// 定期実行
		/// </summary>
		void LateUpdate()
		{
			if (Time.deltaTime == 0)
				return;

			// Remember initial directions of joints that should not be affected
			// 影響を受けない関節の最初の方向を覚えておく
			Vector3[] jointDirections = new Vector3[nonAffectedJoints.Length];
			for (int i = 0; i < nonAffectedJoints.Length; i++)
			{
				foreach (Transform child in nonAffectedJoints[i].joint)
				{
					jointDirections[i] = child.position - nonAffectedJoints[i].joint.position;
					break;
				}
			}

			// Handle each segment
			// セグメントを処理する
			Transform t = lastBone;

			// overrideAnimationの線形補間用のパラメータ
			if (overrideAnimation)
			{
				// ON の場合はMax値になるまで徐々に補正を強くしていく
				if (lerpFloat < lerpFloatMax)
				{
					lerpFloat += lerpFrameDiff;
				}
				else
				{
					lerpFloat = lerpFloatMax;
				}
			}
			else
			{
				// OFF の場合は0.0fになるまで徐々に補正を弱くしていく
				if (lerpFloat > 0.0f)
				{
					lerpFloat -= lerpFrameDiff;
				}
				else
				{
					lerpFloat = 0.0f;
				}
			}

			// overrideAnimationの線形補間用のパラメータ
			// 補正値に基づいて ON/OFF 状態の線形補完を行う
			for (int i = segment.chainLength - 1; i >= 0; i--)
			{
				t.localRotation = Quaternion.Lerp(t.localRotation, segment.origRotations[i], lerpFloat);
				t = t.parent;
			}

			Quaternion parentRot = firstBone.parent.rotation;
			Quaternion parentRotInv = Quaternion.Inverse(parentRot);

			// Desired look direction in world space
			// ワールド空間での見る方向
			Vector3 lookDirWorld = (target - lastBone.position).normalized;

			// Desired look directions in neck parent space
			// 首の親空間での見る方向
			Vector3 lookDirGoal = (parentRotInv * lookDirWorld);

			// Get the horizontal and vertical rotation angle to look at the target
			// 水平回転角度と垂直回転角度を取得してターゲットを見る
			float hAngle = AngleAroundAxis(
				segment.referenceLookDir, lookDirGoal, segment.referenceUpDir
			);

			Vector3 rightOfTarget = Vector3.Cross(segment.referenceUpDir, lookDirGoal);

			Vector3 lookDirGoalinHPlane =
				lookDirGoal - Vector3.Project(lookDirGoal, segment.referenceUpDir);

			float vAngle = AngleAroundAxis(
				lookDirGoalinHPlane, lookDirGoal, rightOfTarget
			);

			// Handle threshold angle difference, bending multiplier, and max angle difference here
			// 敷居値角度差、曲げ乗数、最大角度差を処理する
			float hAngleThr = Mathf.Max(
				0, Mathf.Abs(hAngle) - segment.thresholdAngleDifference
			) * Mathf.Sign(hAngle);

			float vAngleThr = Mathf.Max(
				0, Mathf.Abs(vAngle) - segment.thresholdAngleDifference
			) * Mathf.Sign(vAngle);

			hAngle = Mathf.Max(
				Mathf.Abs(hAngleThr) * Mathf.Abs(segment.bendingMultiplier),
				Mathf.Abs(hAngle) - segment.maxAngleDifference
			) * Mathf.Sign(hAngle) * Mathf.Sign(segment.bendingMultiplier);

			vAngle = Mathf.Max(
				Mathf.Abs(vAngleThr) * Mathf.Abs(segment.bendingMultiplier),
				Mathf.Abs(vAngle) - segment.maxAngleDifference
			) * Mathf.Sign(vAngle) * Mathf.Sign(segment.bendingMultiplier);

			// Handle max bending angle here
			// 最大曲げ角度を処理する
			hAngle = Mathf.Clamp(hAngle, -segment.maxBendingAngle, segment.maxBendingAngle);
			vAngle = Mathf.Clamp(vAngle, -segment.maxBendingAngle, segment.maxBendingAngle);

			Vector3 referenceRightDir =
				Vector3.Cross(segment.referenceUpDir, segment.referenceLookDir);

			// Lerp angles
			// 角度のLerp処理を行う
			segment.angleH = Mathf.Lerp(
				segment.angleH, hAngle, Time.deltaTime * segment.responsiveness
			);
			segment.angleV = Mathf.Lerp(
				segment.angleV, vAngle, Time.deltaTime * segment.responsiveness
			);

			// Get direction
			// 見る方向を取得する
			lookDirGoal = Quaternion.AngleAxis(segment.angleH, segment.referenceUpDir)
				* Quaternion.AngleAxis(segment.angleV, referenceRightDir)
				* segment.referenceLookDir;

			// Make look and up perpendicular
			// 垂直な上方向を取得する
			Vector3 upDirGoal = segment.referenceUpDir;
			Vector3.OrthoNormalize(ref lookDirGoal, ref upDirGoal);

			// Interpolated look and up directions in neck parent space
			// 首の親空間の補間された見る方向と上方向
			Vector3 lookDir = lookDirGoal;
			segment.dirUp = Vector3.Slerp(segment.dirUp, upDirGoal, Time.deltaTime * 5);
			Vector3.OrthoNormalize(ref lookDir, ref segment.dirUp);

			// Look rotation in world space
			// ワールド空間での見る回転角
			Quaternion lookRot = (
				(parentRot * Quaternion.LookRotation(lookDir, segment.dirUp))
				* Quaternion.Inverse(
					parentRot * Quaternion.LookRotation(
						segment.referenceLookDir, segment.referenceUpDir
					)
				)
			);

			// Distribute rotation over all joints in segment
			// セグメント内のすべてのジョイントに回転を配分する
			Quaternion dividedRotation =
				Quaternion.Slerp(Quaternion.identity, lookRot, effect / segment.chainLength);
			t = lastBone;
			for (int i = 0; i < segment.chainLength; i++)
			{
				t.rotation = dividedRotation * t.rotation;
				t = t.parent;
			}

			// Handle non affected joints
			// 影響を受けないジョイントを処理する
			for (int i = 0; i < nonAffectedJoints.Length; i++)
			{
				Vector3 newJointDirection = Vector3.zero;

				foreach (Transform child in nonAffectedJoints[i].joint)
				{
					newJointDirection = child.position - nonAffectedJoints[i].joint.position;
					break;
				}

				Vector3 combinedJointDirection = Vector3.Slerp(
					jointDirections[i], newJointDirection, nonAffectedJoints[i].effect
				);

				nonAffectedJoints[i].joint.rotation = Quaternion.FromToRotation(
					newJointDirection, combinedJointDirection
				) * nonAffectedJoints[i].joint.rotation;
			}
		}

		/// <summary>
		/// The angle between dirA and dirB around axis
		/// 軸の周りの dirA と dirB の間の角度を計算する
		/// </summary>
		/// <param name="dirA"></param>
		/// <param name="dirB"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
		{
			// Project A and B onto the plane orthogonal target axis
			// 平面のターゲット軸に投影する A と B のベクトルを算出する 
			dirA -= Vector3.Project(dirA, axis);
			dirB -= Vector3.Project(dirB, axis);

			// Find (positive) angle between A and B
			// A と B の間の (正の) 角度を計算する
			float angle = Vector3.Angle(dirA, dirB);

			// Return angle multiplied with 1 or -1
			// 戻り角に 1 または -1 を掛ける
			return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
		}
	}
}
