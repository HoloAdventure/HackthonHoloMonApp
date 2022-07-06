using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoloMonApp.Content.Player
{
    /// <summary>
    /// 頭部ジェスチャー(Yes/No)の判定クラス
    /// </summary>
    public class HeadGestureRecognizer : MonoBehaviour
    {
        /// <summary>
        /// 顔ジェスチャーの識別状態
        /// </summary>
        [Serializable]
        public enum HeadGestureStatus
        {
            /// <summary>
            /// 無し(デフォルト)
            /// </summary>
            Nothing = 0,
            /// <summary>
            /// 頷く
            /// </summary>
            Nod,
            /// <summary>
            /// 首振り
            /// </summary>
            Shake,
            /// <summary>
            /// 首傾げ(左方向)
            /// </summary>
            TiltLeft,
            /// <summary>
            /// 首傾げ(右方向)
            /// </summary>
            TiltRight,
        }

        /// <summary>
        /// 回転ポーズのサンプリングデータ型
        /// </summary>
        public struct PoseSample
        {
            // タイムスタンプ
            public readonly float Timestamp;
            // 回転方向
            public Quaternion Orientation;
            // オイラー角
            public Vector3 EulerAngles;

            public PoseSample(float timestamp, Quaternion orientation)
            {
                Timestamp = timestamp;
                Orientation = orientation;

                EulerAngles = orientation.eulerAngles;
                EulerAngles.x = WrapDegree(EulerAngles.x);
                EulerAngles.y = WrapDegree(EulerAngles.y);
                EulerAngles.z = WrapDegree(EulerAngles.z);
            }

            /// <summary>
            /// オイラー角を 180度 ～ -180度 の範囲に変換する
            /// </summary>
            public float WrapDegree(float degree)
            {
                if (degree > 180f)
                {
                    return degree - 360f;
                }
                return degree;
            }
        }


        /// <summary>
        /// 通常時イベント
        /// </summary>
        public Action EventNothing;

        /// <summary>
        /// 頷きイベント
        /// </summary>
        public Action EventNod;

        /// <summary>
        /// 首振りイベント
        /// </summary>
        public Action EventShake;

        /// <summary>
        /// 首傾げ(左方向)イベント
        /// </summary>
        public Action EventTiltLeft;

        /// <summary>
        /// 首傾げ(右方向)イベント
        /// </summary>
        public Action EventTiltRight;


        /// <summary>
        /// 回転ポーズのキュー
        /// </summary>
        public readonly Queue<PoseSample> PoseSamples = new Queue<PoseSample>();


        [SerializeField, Tooltip("イベントのインターバル時間(秒)")]
        private float p_RecognitionInterval = 0.5f;

        [SerializeField, Tooltip("顔ジェスチャーの識別状態")]
        private HeadGestureStatus p_CurrentStatus;

        /// <summary>
        /// 顔ジェスチャーの識別状態
        /// </summary>
        public HeadGestureStatus CurrentStatus => p_CurrentStatus;

        /// <summary>
        /// 前回のジェスチャー発生時刻
        /// </summary>
        private float prevGestureTime;

        /// <summary>
        /// ジェスチャーの実行フラグ
        /// </summary>
        private bool p_GesturedFlg;

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            // 現在の自身のローカル回転を取得する
            var orientation = this.gameObject.transform.localRotation;

            // 回転情報をタイムスタンプとともにキューに保存する
            PoseSamples.Enqueue(new PoseSample(Time.time, orientation));
            if (PoseSamples.Count >= 120)
            {
                // キューの保存は 120 個まで
                PoseSamples.Dequeue();
            }

            // 前回のジェスチャーイベントからインターバル時間の間
            // 新たなジェスチャー判定は行わない
            if (!(prevGestureTime < Time.time - p_RecognitionInterval)) return;

            // ジェスチャーの判定フラグをオフにする
            p_GesturedFlg = false;

            // 頷きジェスチャーの判定を行う
            if (!p_GesturedFlg)
            {
                if (IsRecognizeNod())
                {
                    // 現在の状態を変更する
                    p_CurrentStatus = HeadGestureStatus.Nod;

                    // イベントを実行する
                    EventNod?.Invoke();

                    // ジェスチャーの判定時間を記録し、フラグをオンにする
                    prevGestureTime = Time.time;
                    p_GesturedFlg = true;
                }
            }

            // 首振りジェスチャーの判定を行う
            if (!p_GesturedFlg)
            {
                if (IsRecognizeShake())
                {
                    // 現在の状態を変更する
                    p_CurrentStatus = HeadGestureStatus.Shake;

                    // イベントを実行する
                    EventShake?.Invoke();

                    // ジェスチャーの判定時間を記録し、フラグをオンにする
                    prevGestureTime = Time.time;
                    p_GesturedFlg = true;
                }
            }

            // 首傾げ(左方向)ジェスチャーの判定を行う
            if (!p_GesturedFlg)
            {
                if (IsRecognizeTiltLeft())
                {
                    // 現在の状態を変更する
                    p_CurrentStatus = HeadGestureStatus.TiltLeft;

                    // イベントを実行する
                    EventTiltLeft?.Invoke();

                    // ジェスチャーの判定時間を記録し、フラグをオンにする
                    prevGestureTime = Time.time;
                    p_GesturedFlg = true;
                }
            }

            // 首傾げ(右方向)ジェスチャーの判定を行う
            if (!p_GesturedFlg)
            {
                if (IsRecognizeTiltRight())
                {
                    // 現在の状態を変更する
                    p_CurrentStatus = HeadGestureStatus.TiltRight;

                    // イベントを実行する
                    EventTiltRight?.Invoke();

                    // ジェスチャーの判定時間を記録し、フラグをオンにする
                    prevGestureTime = Time.time;
                    p_GesturedFlg = true;
                }
            }

            // 全てのジェスチャー判定が失敗したか
            if (!p_GesturedFlg)
            {
                // 現在の状態を変更する
                p_CurrentStatus = HeadGestureStatus.Nothing;

                // イベントを実行する
                EventNothing?.Invoke();
            }
        }

        /// <summary>
        /// 指定時間範囲の回転ポーズを取得する
        /// </summary>
        IEnumerable<PoseSample> Range(float startTime, float endTime) =>
            PoseSamples.Where(sample =>
                sample.Timestamp < Time.time - startTime &&
                sample.Timestamp >= Time.time - endTime);

        /// <summary>
        /// 頷き判定チェック
        /// </summary>
        private bool IsRecognizeNod()
        {
            bool isNod = false;
            try
            {
                // 0.4秒前から0.2秒前までの間の縦回転の平均を取得する
                var averagePitch = Range(0.2f, 0.4f).Average(sample => sample.EulerAngles.x);
                // 0.2秒前から現在までの間の縦回転の最大値(正方向：下回転)を取得する
                var maxPitch = Range(0.01f, 0.2f).Max(sample => sample.EulerAngles.x);
                // 最新の縦回転の角度を取得する
                var pitch = PoseSamples.Last().EulerAngles.x;

                // 下方向の最大回転角が平均の回転角より5度以上で
                // かつ、最新の回転角が下方向の最大回転角より2.5度以上戻っているか
                if (!(maxPitch - averagePitch > 5.0f)
                    || !(maxPitch - pitch > 2.5f)) return isNod;

                Debug.Log("Nod last : " + pitch + ", average : " + averagePitch
                    + ", max : " + maxPitch);

                // 頷きが発生したと判定する
                isNod = true;
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
            return isNod;
        }

        private bool IsRecognizeShake()
        {
            bool isShake = false;
            try
            {
                // 0.4秒前から0.2秒前までの間の横回転の平均を取得する
                var averageYaw = Range(0.2f, 0.4f).Average(sample => sample.EulerAngles.y);
                // 0.2秒前から現在までの間の横回転の最大値(正方向：右回転)を取得する
                var maxYaw = Range(0.01f, 0.2f).Max(sample => sample.EulerAngles.y);
                // 0.2秒前から現在までの間の横回転の最小値(負方向：左回転)を取得する
                var minYaw = Range(0.01f, 0.2f).Min(sample => sample.EulerAngles.y);
                // 最新の横回転の角度を取得する
                var yaw = PoseSamples.Last().EulerAngles.y;

                // 最大の回転角が平均の回転角より10度以上大きくない場合、首振りではない
                if (!(maxYaw - averageYaw > 5.0f) ||
                    !(averageYaw - minYaw > 5.0f)) return isShake;

                Debug.Log("Shake last : " + yaw + ", average : " + averageYaw
                    + ", max : " + maxYaw + ", min : " + minYaw);

                // 首振りが発生したと判定する
                isShake = true;
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
            return isShake;
        }

        /// <summary>
        /// 首傾げ(左方向)判定チェック
        /// </summary>
        private bool IsRecognizeTiltLeft()
        {
            bool isTilt = false;
            try
            {
                // 0.4秒前から現在までの間の正面回転の平均を取得する
                var averageTilt = Range(0.01f, 0.4f).Average(sample => sample.EulerAngles.z);
                // 最新の正面回転の角度を取得する
                var tilt = PoseSamples.Last().EulerAngles.z;

                // 正面方向の平均の回転角が20度以上であるか
                if (!(averageTilt > 20.0f)) return isTilt;

                Debug.Log("Tilt last : " + tilt + ", average : " + averageTilt);

                // 頷きが発生したと判定する
                isTilt = true;
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
            return isTilt;
        }

        /// <summary>
        /// 首傾げ(右方向)判定チェック
        /// </summary>
        private bool IsRecognizeTiltRight()
        {
            bool isTilt = false;
            try
            {
                // 0.4秒前から現在までの間の正面回転の平均を取得する
                var averageTilt = Range(0.01f, 0.4f).Average(sample => sample.EulerAngles.z);
                // 最新の正面回転の角度を取得する
                var tilt = PoseSamples.Last().EulerAngles.z;

                // 正面方向の平均の回転角が-20度以下であるか
                if (!(averageTilt < -20.0f)) return isTilt;

                Debug.Log("Tilt last : " + tilt + ", average : " + averageTilt);

                // 頷きが発生したと判定する
                isTilt = true;
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
            return isTilt;
        }
    }
}