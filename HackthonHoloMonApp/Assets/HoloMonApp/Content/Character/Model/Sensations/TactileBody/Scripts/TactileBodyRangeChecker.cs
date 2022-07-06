using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Data.Conditions.Body;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;
using HoloMonApp.Content.Character.Data.Transforms.Body;

namespace HoloMonApp.Content.Character.Model.Sensations.TactileBody
{
    public class TactileBodyRangeChecker : MonoBehaviour
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
        /// 触覚原点
        /// </summary>
        private Transform p_TactileRoot => p_TransformsBodyData.Head;

        /// <summary>
        /// タッチ距離の定義(不変)
        /// </summary>
        private float p_TactileAreaBase = 0.0f;

        /// <summary>
        /// 成長可変距離の定義(デフォルトサイズ時)
        /// </summary>
        private float p_ScaleStretchRange = 0.2f;

        /// <summary>
        /// 対象オブジェクトの触覚距離を判定する
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public TactileRangeType TargetTactileRangeArea(GameObject a_Object)
        {
            // 触覚原点とオブジェクト間のベクトルを算出し、距離と方向ベクトルを取得する
            float betweenDistance = TargetDistance(a_Object, false);

            // 距離チェック
            // 強制発見距離を現在のスケールを基に計算する
            float forcedDiscoveryDistance = CurrentForcedDiscoveryDistance();

            // 強制発見距離か否か
            if (betweenDistance < forcedDiscoveryDistance)
            {
                // 強制発見距離内なら接触エリア種別を判定する
                return TactileRangeType.TactileArea;
            }

            // 発見距離外ならアウトレンジ種別を判定する
            return TactileRangeType.OutRangeArea;
        }

        /// <summary>
        /// 対象オブジェクトと触覚原点の距離を計算する
        /// </summary>
        /// <param name="a_Object"></param>
        /// <returns></returns>
        public float TargetDistance(GameObject a_Object, bool a_IgnoreYAxis)
        {
            // 触覚の原点座標を取得する
            Vector3 RootPosition = p_TactileRoot.position;

            // オブジェクトの確認座標を取得する
            Vector3 checkObjectPosition = a_Object.transform.position;

            // Y軸高低差を無視するフラグが指定されていれば
            // Y軸の高さを基準トランスフォームと同じにする
            if (a_IgnoreYAxis)
            {
                checkObjectPosition = new Vector3(
                    a_Object.transform.position.x,
                    p_TactileRoot.position.y,
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
            Vector3 RootPosition = p_TactileRoot.position;

            // 視界の方向を取得する
            Vector3 RootForward = p_TactileRoot.forward;

            // オブジェクトの確認座標を取得する
            Vector3 checkObjectPosition = a_Object.transform.position;

            // Y軸高低差を無視するフラグが指定されていれば
            // Y軸の高さを基準トランスフォームと同じにする
            if (a_IgnoreYAxis)
            {
                checkObjectPosition = new Vector3(
                    a_Object.transform.position.x,
                    p_TactileRoot.position.y,
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
        /// 現在の発見距離
        /// </summary>
        /// <returns></returns>
        private float CurrentForcedDiscoveryDistance()
        {
            return p_TactileAreaBase + (p_ScaleStretchRange * p_ConditionBodyData.Status.GetDefaultBodyHeightRatio);
        }
        #endregion
    }
}