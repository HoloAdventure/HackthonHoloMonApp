using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Data.Conditions.Body;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    public class FieldOfVisionRangeChecker : MonoBehaviour
    {
        /// <summary>
        /// ボディデータの参照
        /// </summary>
        [SerializeField]
        private HoloMonConditionBodyData p_ConditionBodyData;

        /// <summary>
        /// ボディトランスフォームの参照
        /// </summary>
        [SerializeField]
        private HoloMonTransformsBodyData p_TransformsBodyData;

        /// <summary>
        /// 視覚原点
        /// </summary>
        private Transform p_VisionRoot => p_TransformsBodyData.Head;

        /// <summary>
        /// 近距離の定義(不変)
        /// </summary>
        private float p_NearAreaBase = 0.5f;

        /// <summary>
        /// 中距離の定義(不変)
        /// </summary>
        private float p_MiddleAreaBase = 2.5f;

        /// <summary>
        /// 遠距離の定義(不変)
        /// </summary>
        private float p_FarAreaBase = 10.0f;

        /// <summary>
        /// 成長可変距離の定義(デフォルトサイズ時)
        /// </summary>
        private float p_ScaleStretchRange = 0.2f;

        /// <summary>
        /// 近距離視野角
        /// </summary>
        private float p_NearViewAngle = 120;

        /// <summary>
        /// 中距離視野角
        /// </summary>
        private float p_MiddleViewAngle = 100;

        /// <summary>
        /// 遠距離視野角(不変)
        /// </summary>
        private float p_FarViewAngle = 50;


        /// <summary>
        /// 対象オブジェクトの視覚距離を判定する
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public VisionRangeType TargetVisionRangeArea(GameObject a_Object)
        {
            // 視覚原点とオブジェクト間の距離と角度を取得する
            float betweenDistance = TargetDistance(a_Object, false);
            float betweenAngle = TargetAngle(a_Object, false);

            // 近距離か否か
            if (betweenDistance < CurrentNearDiscoveryDistance())
            {
                // 近距離視野角内か否か
                if (betweenAngle < CurrentNearDiscoveryAngle())
                {
                    // 近距離と判定する
                    return VisionRangeType.NearArea;
                }
                else
                {
                    // 視野角外なら見えていない
                    return VisionRangeType.OutRangeArea;
                }
            }
            else
            {
                // 中距離か否か
                if (betweenDistance < CurrentMiddleDiscoveryDistance())
                {
                    // 中距離視野角内か否か
                    if (betweenAngle < CurrentMiddleDiscoveryAngle())
                    {
                        // 中距離と判定する
                        return VisionRangeType.MiddleArea;
                    }
                    else
                    {
                        // 視野角外なら見えていない
                        return VisionRangeType.OutRangeArea;
                    }
                }
                else
                {
                    // 遠距離か否か
                    if (betweenDistance < CurrentFarDiscoveryDistance())
                    {
                        // 遠距離視野角内か否か
                        if (betweenAngle < CurrentFarDiscoveryAngle())
                        {
                            // 遠距離と判定する
                            return VisionRangeType.FarArea;
                        }
                        else
                        {
                            // 視野角外なら見えていない
                            return VisionRangeType.OutRangeArea;
                        }
                    }
                }
            }

            return VisionRangeType.OutRangeArea;
        }

        /// <summary>
        /// 対象オブジェクトとの間に障害物があるかチェックする
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public bool TargetVisionCheckObstacle(GameObject a_Object)
        {
            // 視界の原点座標を取得する
            Vector3 RootPosition = p_VisionRoot.position;

            // オブジェクトの確認座標を取得する
            Vector3 checkObjectPosition = a_Object.transform.position;

            // 視界原点とオブジェクト間のベクトルを算出し、距離と方向ベクトルを取得する
            Vector3 betweenVector = checkObjectPosition - RootPosition;
            float betweenDistance = betweenVector.magnitude;
            Vector3 betweenDirection = betweenVector.normalized;

            // レイキャストの結果
            RaycastHit[] raycastHits = new RaycastHit[3];

            // レイキャストでその方向の衝突オブジェクトを検知する
            int hitCount = Physics.RaycastNonAlloc(RootPosition, betweenDirection, raycastHits, betweenDistance);

            if (hitCount > 1)
            {
                // ヒット数が 1 より多いなら間に障害物(対象以外)がある
                return true;
            }
            // 障害物なし
            return false;
        }

        /// <summary>
        /// 対象オブジェクトと視界原点の距離を計算する
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public float TargetDistance(GameObject a_Object, bool a_IgnoreYAxis)
        {
            // 視界の原点座標を取得する
            Vector3 RootPosition = p_VisionRoot.position;

            // オブジェクトの確認座標を取得する
            Vector3 checkObjectPosition = a_Object.transform.position;

            // Y軸高低差を無視するフラグが指定されていれば
            // Y軸の高さを基準トランスフォームと同じにする
            if (a_IgnoreYAxis)
            {
                checkObjectPosition = new Vector3(
                    a_Object.transform.position.x,
                    p_VisionRoot.position.y,
                    a_Object.transform.position.z
                    );
            }

            // 視界原点と座標間の距離を取得する
            return Vector3.Distance(RootPosition, checkObjectPosition);
        }

        /// <summary>
        /// 対象オブジェクトと視界原点の角度を計算する
        /// </summary>
        public float TargetAngle(GameObject a_Object, bool a_IgnoreYAxis)
        {
            // 視界の原点座標を取得する
            Vector3 RootPosition = p_VisionRoot.position;

            // 視界の方向を取得する
            Vector3 RootForward = p_VisionRoot.forward;

            // オブジェクトの確認座標を取得する
            Vector3 checkObjectPosition = a_Object.transform.position;

            // Y軸高低差を無視するフラグが指定されていれば
            // Y軸の高さを基準トランスフォームと同じにする
            if (a_IgnoreYAxis)
            {
                checkObjectPosition = new Vector3(
                    a_Object.transform.position.x,
                    p_VisionRoot.position.y,
                    a_Object.transform.position.z
                    );
            }

            // 視界原点とオブジェクト間のベクトルを算出し、方向ベクトルを取得する
            Vector3 betweenVector = checkObjectPosition - RootPosition;
            Vector3 betweenDirection = betweenVector.normalized;

            // 視界原点の方向ベクトルを取得する
            Vector3 headDirection = RootForward;

            // 2つの方向ベクトルの角度差(0°～ 180°)を取得する
            return Vector3.Angle(headDirection, betweenDirection);
        }

        #region "private"
        /// <summary>
        /// 現在の近距離の発見距離
        /// </summary>
        /// <returns></returns>
        private float CurrentNearDiscoveryDistance()
        {
            return p_NearAreaBase + (p_ScaleStretchRange * p_ConditionBodyData.Status.GetDefaultBodyHeightRatio);
        }

        /// <summary>
        /// 現在の中距離の発見距離
        /// </summary>
        /// <returns></returns>
        private float CurrentMiddleDiscoveryDistance()
        {
            return p_MiddleAreaBase + (p_ScaleStretchRange * p_ConditionBodyData.Status.GetDefaultBodyHeightRatio);
        }

        /// <summary>
        /// 現在の遠距離の発見距離
        /// </summary>
        /// <returns></returns>
        private float CurrentFarDiscoveryDistance()
        {
            return p_FarAreaBase + (p_ScaleStretchRange * p_ConditionBodyData.Status.GetDefaultBodyHeightRatio);
        }

        /// <summary>
        /// 現在の近距離の発見角度
        /// </summary>
        /// <returns></returns>
        private float CurrentNearDiscoveryAngle()
        {
            return p_NearViewAngle;
        }

        /// <summary>
        /// 現在の中距離の発見角度
        /// </summary>
        /// <returns></returns>
        private float CurrentMiddleDiscoveryAngle()
        {
            return p_MiddleViewAngle;
        }

        /// <summary>
        /// 現在の遠距離の発見角度
        /// </summary>
        /// <returns></returns>
        private float CurrentFarDiscoveryAngle()
        {
            return p_FarViewAngle;
        }
        #endregion
    }
}
