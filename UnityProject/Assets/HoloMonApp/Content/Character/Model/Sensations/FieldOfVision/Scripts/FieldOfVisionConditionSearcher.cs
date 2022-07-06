using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    public class FieldOfVisionConditionSearcher : MonoBehaviour
    {
        /// <summary>
        /// 距離チェッカーの参照
        /// </summary>
        [SerializeField]
        private FieldOfVisionRangeChecker p_RangeChecker;

        /// <summary>
        /// 視界内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// </summary>
        public VisionObjectWrap CheckCollectionByGameObject(
            IEnumerable<VisionObjectWrap> a_CheckCollectionData,
            GameObject a_Object
            )
        {
            // 選択オブジェクトの参照
            VisionObjectWrap targetVisionObjectWrap = null;

            foreach (VisionObjectWrap checkVisionObjectWrap in a_CheckCollectionData)
            {
                if (checkVisionObjectWrap.Object == a_Object)
                {
                    targetVisionObjectWrap = checkVisionObjectWrap;
                    break;
                }
            }
            return targetVisionObjectWrap;
        }

        /// <summary>
        /// 視界オブジェクトコレクションから指定条件が一致する
        /// かつ、距離が最も近いオブジェクトを探す
        /// </summary>
        public VisionObjectWrap CheckCollectionByNearDistance(
            IEnumerable<VisionObjectWrap> a_CheckCollectionData,
            bool a_CheckObjectName, string a_ObjectName,
            bool a_CheckNearDistance, float a_NearDistance,
            bool a_CheckInsideAngle, float a_InsideAngle,
            bool a_CheckObjectUnderstandType, ObjectUnderstandType a_ObjectUnderstandType
            )
        {
            // 選択オブジェクトの参照
            VisionObjectWrap targetVisionObjectWrap = null;

            // 選択オブジェクトの距離
            float targetDistance = 0.0f;

            foreach (VisionObjectWrap checkVisionObjectWrap in a_CheckCollectionData)
            {
                // 各条件を満たすかチェックする
                if (a_CheckObjectName)
                {
                    if (!CheckObjectName(a_ObjectName, checkVisionObjectWrap)) continue;
                }
                if (a_CheckNearDistance)
                {
                    if (!CheckObjectNearDistance(a_NearDistance, checkVisionObjectWrap)) continue;
                }
                if (a_CheckInsideAngle)
                {
                    if (!CheckObjectInsideAngle(a_InsideAngle, checkVisionObjectWrap)) continue;
                }
                if (a_CheckObjectUnderstandType)
                {
                    if (!CheckObjectUnderstandType(a_ObjectUnderstandType, checkVisionObjectWrap)) continue;
                }

                // オブジェクトまでの距離を取得する
                float checkDistance = p_RangeChecker.TargetDistance(checkVisionObjectWrap.Object, false);

                // 1つ目はチェック無し
                if (targetVisionObjectWrap == null)
                {
                    // 選択状態を保存する
                    targetVisionObjectWrap = checkVisionObjectWrap;
                    targetDistance = checkDistance;

                    continue;
                }

                // 2つ目以降は比較して距離の近い方を選択する
                if (checkDistance < targetDistance)
                {
                    // 選択状態を保存する
                    targetVisionObjectWrap = checkVisionObjectWrap;
                    targetDistance = checkDistance;

                    continue;
                }
            }

            return targetVisionObjectWrap;
        }

        /// <summary>
        /// 視界オブジェクトコレクションから指定条件が一致する
        /// かつ、優先度が最も近いオブジェクトを探す
        /// 優先度が同じ場合は距離の近いものを選択する
        /// </summary>
        public VisionObjectWrap CheckCollectionByTypePriority(
            IEnumerable<VisionObjectWrap> a_CheckCollectionData,
            IEnumerable<ObjectUnderstandTypePriority> a_CheckObjectUnderstandTypePriorityList,
            bool a_CheckObjectName, string a_ObjectName,
            bool a_CheckNearDistance, float a_NearDistance,
            bool a_CheckInsideAngle, float a_InsideAngle
            )
        {
            // 選択オブジェクトの参照
            VisionObjectWrap targetVisionObjectWrap = null;

            // 選択オブジェクトの距離
            float targetDistance = 0.0f;

            // 選択オブジェクトの優先度
            int targetPriority = 0;

            foreach (VisionObjectWrap checkVisionObjectWrap in a_CheckCollectionData)
            {
                // 各条件を満たすかチェックする
                if (a_CheckObjectName)
                {
                    if (!CheckObjectName(a_ObjectName, checkVisionObjectWrap)) continue;
                }
                if (a_CheckNearDistance)
                {
                    if (!CheckObjectNearDistance(a_NearDistance, checkVisionObjectWrap)) continue;
                }
                if (a_CheckInsideAngle)
                {
                    if (!CheckObjectInsideAngle(a_InsideAngle, checkVisionObjectWrap)) continue;
                }

                // チェック対象のオブジェクト理解種別を取得する
                ObjectUnderstandType checkType = checkVisionObjectWrap.CurrentFeatures().ObjectUnderstandType;

                // 優先度が設定されていれば優先度の値を取得する
                int checkPriority = 0;
                foreach (ObjectUnderstandTypePriority typePriority in a_CheckObjectUnderstandTypePriorityList)
                {
                    if (checkType == typePriority.ObjectUnderstandType)
                    {
                        checkPriority = typePriority.Priority;
                        break;
                    }
                }

                // オブジェクトまでの距離を取得する
                float checkDistance = p_RangeChecker.TargetDistance(checkVisionObjectWrap.Object, false);

                // 1つ目はチェック無し
                if (targetVisionObjectWrap == null)
                {
                    // 選択状態を保存する
                    targetVisionObjectWrap = checkVisionObjectWrap;
                    targetDistance = checkDistance;
                    targetPriority = checkPriority;

                    continue;
                }

                // 2つ目以降は比較して優先度が高ければ選択する
                if (checkPriority > targetPriority)
                {
                    // 選択状態を保存する
                    targetVisionObjectWrap = checkVisionObjectWrap;
                    targetDistance = checkDistance;
                    targetPriority = checkPriority;

                    continue;
                }

                // もし優先度が同じならば近い方を選択する
                if (checkPriority == targetPriority)
                {
                    if (checkDistance < targetDistance)
                    {
                        // 選択状態を保存する
                        targetVisionObjectWrap = checkVisionObjectWrap;
                        targetDistance = checkDistance;
                        targetPriority = checkPriority;

                        continue;
                    }
                }
            }

            return targetVisionObjectWrap;
        }

        #region "private"
        /// <summary>
        /// オブジェクト名をチェックする
        /// </summary>
        private bool CheckObjectName(string a_CheckName, VisionObjectWrap a_CheckObjectWrap)
        {
            if (a_CheckName != a_CheckObjectWrap.CurrentName()) return false;
            return true;
        }

        /// <summary>
        /// オブジェクト種別をチェックする
        /// </summary>
        private bool CheckObjectUnderstandType(ObjectUnderstandType a_CheckUnderstandType, VisionObjectWrap a_CheckObjectWrap)
        {
            if (a_CheckUnderstandType != a_CheckObjectWrap.CurrentFeatures().ObjectUnderstandType) return false;
            return true;
        }

        /// <summary>
        /// 距離が指定距離より近いかチェックする
        /// </summary>
        private bool CheckObjectNearDistance(float a_NearDistance, VisionObjectWrap a_CheckObjectWrap)
        {
            if (a_NearDistance < p_RangeChecker.TargetDistance(a_CheckObjectWrap.Object, false)) return false;
            return true;
        }

        /// <summary>
        /// 距離が指定距離より遠いかチェックする
        /// </summary>
        private bool CheckObjectFarDistance(float a_FarDistance, VisionObjectWrap a_CheckObjectWrap)
        {
            if (a_FarDistance > p_RangeChecker.TargetDistance(a_CheckObjectWrap.Object, false)) return false;
            return true;
        }

        /// <summary>
        /// 角度が指定角度より内側かチェックする
        /// </summary>
        private bool CheckObjectInsideAngle(float a_InsideAngle, VisionObjectWrap a_CheckObjectWrap)
        {
            if (a_InsideAngle < p_RangeChecker.TargetAngle(a_CheckObjectWrap.Object, false)) return false;
            return true;
        }
        #endregion
    }
}