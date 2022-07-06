using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.VisualizeUIs.WordBubble
{
    public class RotateAroundTargetGravitation : MonoBehaviour
    {
        /// <summary>
        /// 移動完了時イベント
        /// </summary>
        public Action MoveEndEvent;

        [Tooltip("開始位置オブジェクト")]
        public GameObject StartObject;

        [Tooltip("ターゲットオブジェクト")]
        public GameObject TargetObject;

        [Tooltip("開始時の回転速度係数")]
        public float SpeedFactor;

        [Tooltip("到達までの秒数")]
        public float LerpTime;

        /// <summary>
        /// 現在の経過時間
        /// </summary>
        private float timeElapsed;

        /// <summary>
        /// 開始時の2点間の距離
        /// </summary>
        private float p_RadiusDistance;

        /// <summary>
        /// 回転軸
        /// </summary>
        private Vector3 p_RotateAxis;

        /// <summary>
        /// 移動中フラグ
        /// </summary>
        private bool p_MoveFlg;

        private void Start()
        {
            // 回転軸はY軸(縦軸)に固定する
            p_RotateAxis = Vector3.up;
        }

        // Update is called once per frame
        void Update()
        {
            if (!p_MoveFlg) return;

            // 設定が不足している場合は処理しない
            if (StartObject == null || TargetObject == null　||
                SpeedFactor <= 0.0f || LerpTime <= 0.0f) return;

            // 経過時間を記録する
            timeElapsed += Time.deltaTime;

            // 各オブジェクト位置を取得する
            Vector3 targetPosition = TargetObject.transform.position;
            Vector3 selfPosition = this.transform.position;

            // 現在の距離を取得する
            float currentRadiusDistance = Vector3.Distance(selfPosition, targetPosition);

            if (Vector3.Distance(targetPosition, selfPosition) < 0.05f)
            {
                // 移動終了イベントを呼び出し
                MoveEndEvent?.Invoke();
                p_MoveFlg = false;
                return;
            }

            // 距離が近づくほど回転速度を早くする
            float currentSpeedFactor = (SpeedFactor * Time.deltaTime) * (p_RadiusDistance / currentRadiusDistance);

            // Leap 関数を使って時間経過で徐々にターゲットに近づいていく
            this.transform.position = Vector3.Lerp(selfPosition, targetPosition, (timeElapsed / LerpTime));

            // 指定オブジェクトを中心に回転する
            this.transform.RotateAround(
                targetPosition,
                p_RotateAxis,
                360.0f / (1.0f / currentSpeedFactor) * Time.deltaTime
                );
        }

        public bool MoveStart()
        {
            // 既に実行中の場合は開始しない
            if (p_MoveFlg) return false;

            // 設定が不足している場合は開始しない
            if (StartObject == null || TargetObject == null ||
                SpeedFactor <= 0.0f || LerpTime <= 0.0f) return false;

            // 各オブジェクト位置を取得する
            Vector3 startPosition = StartObject.transform.position;
            Vector3 targetPosition = TargetObject.transform.position;

            // 初期位置はスタートオブジェクトと同じ位置に設定する
            this.transform.position = startPosition;

            // 開始時の距離を取得する
            p_RadiusDistance = Vector3.Distance(startPosition, targetPosition);

            // 経過時間をリセットする
            timeElapsed = 0.0f;

            // 移動中フラグをONにする
            p_MoveFlg = true;

            return true;
        }
    }
}