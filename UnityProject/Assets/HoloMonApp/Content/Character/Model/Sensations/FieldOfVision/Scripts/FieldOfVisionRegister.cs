using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// コライダー処理のため
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.XRPlatform;
using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    [RequireComponent(typeof(SphereCollider))]
    public class FieldOfVisionRegister : MonoBehaviour
    {
        /// <summary>
        /// 空間認識レイヤー
        /// </summary>
        private int p_SpatialAwarenessLayer => HoloMonXRPlatform.Instance.EnvironmentMap.Access.GetEnvironmentMapLayer();

        /// <summary>
        /// 視界内の視界オブジェクトコレクション
        /// </summary>
        [SerializeField]
        private FieldOfVisionMonitor p_Monitor;

        /// <summary>
        /// 触覚内オブジェクト範囲の参照
        /// </summary>
        [SerializeField]
        private FieldOfVisionRangeChecker p_RangeChecker;


        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
            // コライダーの OnTriggerStay イベントに対する処理を定義する
            this.OnTriggerStayAsObservable()                    // OnTriggerStayイベント
                .BatchFrame()                                   // フレーム毎に値をまとめる
                .ObserveOnMainThread()                          // メインスレッドで行う
                .Subscribe(colliderlist =>
                {
                    // 更新があったか
                    bool isUpdate = false;

                    // 検出したオブジェクトのIDリスト
                    List<int> objectIDList = new List<int>();

                    foreach (Collider collider in colliderlist)
                    {
                        if (collider == null) continue;

                        GameObject stayObject = collider.gameObject;
                        if (stayObject == null) continue;

                        // オブジェクトに種別情報があるか
                        ObjectFeatureWrap stayObjectFeatureData = GetObjectFeatures(stayObject);
                        if (stayObjectFeatureData == null) continue;

                        // オブジェクトが見えているか
                        bool isSeen = SeemVisionObject(stayObject, stayObjectFeatureData);
                        if (!isSeen) continue;

                        // 検出オブジェクトをコレクションに登録する
                        bool isRegisted = RegistVisionObject(stayObjectFeatureData);

                        if (isRegisted)
                        {
                            // オブジェクトIDを記録する
                            objectIDList.Add(stayObjectFeatureData.ObjectID);
                        }

                        // 更新を行ったか
                        isUpdate = isUpdate || isRegisted;
                    }

                    // 未検出オブジェクトをコレクションから除去する
                    bool isRemove = RemoveVisionObject(objectIDList);

                    // 更新を行ったか
                    isUpdate = isUpdate || isRemove;
                })
                .AddTo(this);
        }

        /// <summary>
        /// 定期処理
        /// </summary>
        void Update()
        {
            // 視認オブジェクトの状態更新をチェックする
            p_Monitor.CheckStatusUpdate();
        }

        /// <summary>
        /// ゲームオブジェクトが見えているか判定する
        /// </summary>
        private bool SeemVisionObject(GameObject a_VisionObject, ObjectFeatureWrap targetObjectFeature)
        {
            // オブジェクトのレイヤー番号を取得する
            int layernumber = a_VisionObject.layer;

            if (layernumber == p_SpatialAwarenessLayer)
            {
                // 空間認識レイヤーの場合は無視する
                return false;
            }

            // 視覚範囲の判定を取得する
            VisionRangeType targetVisionRangeType = p_RangeChecker.TargetVisionRangeArea(a_VisionObject);
            if (targetVisionRangeType == VisionRangeType.OutRangeArea)
            {
                // 発見距離外なら見えていないと判定する
                return false;
            }
            /*
            // 手は障害物判定を行わない
            if ((targetObjectFeature.UnderstandInformation.ObjectUnderstandType != ObjectUnderstandType.FriendLeftHand) &&
                (targetObjectFeature.UnderstandInformation.ObjectUnderstandType != ObjectUnderstandType.FriendRightHand))
            {
                // 障害物の有無を取得する
                bool CheckObstacle = p_RangeChecker.TargetVisionCheckObstacle(a_VisionObject);
                if (CheckObstacle)
                {
                    // 障害物ありなら見えていないと判定する
                    return false;
                }
            }
            */
            // 問題がなければ見えていると判定する
            return true;
        }


        /// <summary>
        /// 未登録のゲームオブジェクトをコレクションに登録する
        /// </summary>
        private bool RegistVisionObject(ObjectFeatureWrap a_VisionObjectFeatureData)
        {
            bool isResisted = false;

            // 視界オブジェクト情報を作成する
            VisionObjectWrap visionObjectWrap = new VisionObjectWrap(a_VisionObjectFeatureData);

            // オブジェクトの存在チェック
            bool isContain = p_Monitor.ContainsKey(a_VisionObjectFeatureData.ObjectID);

            if (!isContain)
            {
                // コレクションに追加する
                p_Monitor.Add(visionObjectWrap);
            }

            isResisted = true;

            return isResisted;
        }

        /// <summary>
        /// 未検出のゲームオブジェクトをコレクションから除去する
        /// </summary>
        private bool RemoveVisionObject(List<int> a_CheckObjectIDList)
        {
            bool isRemoved = p_Monitor.RemoveWithoutList(a_CheckObjectIDList);

            return isRemoved;
        }

        /// <summary>
        /// オブジェクトの理解種別情報を取得する
        /// </summary>
        public ObjectFeatureWrap GetObjectFeatures(GameObject a_Object)
        {
            return a_Object?.GetComponent<ObjectFeatures>()?.DataWrap;
        }
    }
}
