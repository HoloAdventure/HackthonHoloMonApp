using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;
using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.Sensations.TactileBody
{
    public class TactileBodyConditionSearcher : MonoBehaviour
    {
        /// <summary>
        /// 距離チェッカーの参照
        /// </summary>
        [SerializeField]
        private TactileBodyRangeChecker p_RangeChecker;

        /// <summary>
        /// 触覚内オブジェクトコレクションに同一のゲームオブジェクトが存在するか探す
        /// </summary>
        public TactileObjectWrap CheckCollectionByGameObject(
            IEnumerable<TactileObjectWrap> a_CheckCollectionData,
            GameObject a_Object
            )
        {
            // 選択オブジェクトの参照
            TactileObjectWrap targetTactileObjectWrap = null;

            foreach (TactileObjectWrap checkTactileObjectWrap in a_CheckCollectionData)
            {
                if (checkTactileObjectWrap.Object == a_Object)
                {
                    targetTactileObjectWrap = checkTactileObjectWrap;
                    break;
                }
            }
            return targetTactileObjectWrap;
        }

        /// <summary>
        /// 触覚オブジェクトコレクションから指定条件が一致する
        /// かつ、距離が最も近いオブジェクトを探す
        /// </summary>
        public TactileObjectWrap CheckCollectionByNearDistance(
            IEnumerable<TactileObjectWrap> a_CheckCollectionData,
            bool a_CheckObjectName, string a_ObjectName,
            bool a_CheckNearDistance, float a_NearDistance,
            bool a_CheckInsideAngle, float a_InsideAngle,
            bool a_CheckObjectUnderstandType, ObjectUnderstandType a_ObjectUnderstandType
            )
        {
            // 選択オブジェクトの参照
            TactileObjectWrap targetTactileObjectWrap = null;

            // 選択オブジェクトの距離
            float targetDistance = 0.0f;

            foreach (TactileObjectWrap checkTactileObjectWrap in a_CheckCollectionData)
            {
                // 各条件を満たすかチェックする
                if (a_CheckObjectName)
                {
                    if (!CheckObjectName(a_ObjectName, checkTactileObjectWrap)) continue;
                }
                if (a_CheckNearDistance)
                {
                    if (!CheckObjectNearDistance(a_NearDistance, checkTactileObjectWrap)) continue;
                }
                if (a_CheckInsideAngle)
                {
                    if (!CheckObjectInsideAngle(a_InsideAngle, checkTactileObjectWrap)) continue;
                }
                if (a_CheckObjectUnderstandType)
                {
                    if (!CheckObjectUnderstandType(a_ObjectUnderstandType, checkTactileObjectWrap)) continue;
                }

                // オブジェクトまでの距離を取得する
                float checkDistance = p_RangeChecker.TargetDistance(checkTactileObjectWrap.Object, false);

                // 1つ目はチェック無し
                if (targetTactileObjectWrap == null)
                {
                    // 選択状態を保存する
                    targetTactileObjectWrap = checkTactileObjectWrap;
                    targetDistance = checkDistance;

                    continue;
                }

                // 2つ目以降は比較して距離の近い方を選択する
                if (checkDistance < targetDistance)
                {
                    // 選択状態を保存する
                    targetTactileObjectWrap = checkTactileObjectWrap;
                    targetDistance = checkDistance;

                    continue;
                }
            }

            return targetTactileObjectWrap;
        }

        #region "private"
        /// <summary>
        /// オブジェクト名をチェックする
        /// </summary>
        private bool CheckObjectName(string a_CheckName, TactileObjectWrap a_CheckObjectWrap)
        {
            if (a_CheckName == null) return false;
            if (a_CheckName != a_CheckObjectWrap.CurrentName()) return false;
            return true;
        }

        /// <summary>
        /// オブジェクト種別をチェックする
        /// </summary>
        private bool CheckObjectUnderstandType(ObjectUnderstandType a_CheckUnderstandType, TactileObjectWrap a_CheckObjectWrap)
        {
            if (a_CheckUnderstandType != a_CheckObjectWrap.CurrentFeatures().ObjectUnderstandType) return false;
            return true;
        }

        /// <summary>
        /// 距離が指定距離より近いかチェックする
        /// </summary>
        private bool CheckObjectNearDistance(float a_NearDistance, TactileObjectWrap a_CheckObjectWrap)
        {
            if (a_NearDistance < p_RangeChecker.TargetDistance(a_CheckObjectWrap.Object, false)) return false;
            return true;
        }

        /// <summary>
        /// 距離が指定距離より遠いかチェックする
        /// </summary>
        private bool CheckObjectFarDistance(float a_FarDistance, TactileObjectWrap a_CheckObjectWrap)
        {
            if (a_FarDistance > p_RangeChecker.TargetDistance(a_CheckObjectWrap.Object, false)) return false;
            return true;
        }

        /// <summary>
        /// 角度が指定角度より内側かチェックする
        /// </summary>
        private bool CheckObjectInsideAngle(float a_InsideAngle, TactileObjectWrap a_CheckObjectWrap)
        {
            if (a_InsideAngle < p_RangeChecker.TargetAngle(a_CheckObjectWrap.Object, false)) return false;
            return true;
        }
        #endregion
    }
}