using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// コライダー処理のため
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.XRPlatform;
using HoloMonApp.Content.Character.Data.Sensations.TactileBody;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Model.Sensations.TactileBody
{
    [RequireComponent(typeof(SphereCollider))]
    public class TactileBodyRegister : MonoBehaviour
    {
        /// <summary>
        /// チェック範囲
        /// </summary>
        private enum CheckRange
        {
            /// <summary>
            /// 全方向
            /// </summary>
            AllRange,
            /// <summary>
            /// 上方向
            /// </summary>
            UpRange,
            /// <summary>
            /// 前方方向
            /// </summary>
            FrontRange,
        }

        /// <summary>
        /// 空間認識レイヤー
        /// </summary>
        private int p_SpatialAwarenessLayer => HoloMonXRPlatform.Instance.EnvironmentMap.Access.GetEnvironmentMapLayer();

        /// <summary>
        /// 範囲内の触覚オブジェクト監視の参照
        /// </summary>
        [SerializeField]
        private TactileBodyMonitor p_Monitor;

        /// <summary>
        /// 触覚内オブジェクト範囲の参照
        /// </summary>
        [SerializeField]
        private TactileBodyRangeChecker p_RangeChecker;


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

                        // オブジェクトが触れているか
                        bool isTouched = TouchTactileObject(stayObject);
                        if (!isTouched) continue;

                        // 検出オブジェクトをコレクションに登録する
                        bool isRegisted = RegistTactileObject(stayObjectFeatureData);

                        if (isRegisted)
                        {
                            // オブジェクトを記録する
                            objectIDList.Add(stayObjectFeatureData.ObjectID);
                        }

                        // 更新を行ったか
                        isUpdate = isUpdate || isRegisted;
                    }

                    // 未検出オブジェクトをコレクションから除去する
                    bool isRemove = RemoveTactileObject(objectIDList);

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
        /// ゲームオブジェクトが触れているか判定する
        /// </summary>
        private bool TouchTactileObject(GameObject a_TactileObject)
        {
            // オブジェクトのレイヤー番号を取得する
            int layernumber = a_TactileObject.layer;

            if (layernumber == p_SpatialAwarenessLayer)
            {
                // 空間認識レイヤーの場合は無視する
                return false;
            }

            // 触覚範囲の判定を取得する
            TactileRangeType targetTactileRangeType = p_RangeChecker.TargetTactileRangeArea(a_TactileObject);

            // 発見距離か否か
            if (targetTactileRangeType == TactileRangeType.OutRangeArea)
            {
                // 発見距離外なら触れていないと判定する
                return false;
            }

            // 見えていると判定する
            return true;
        }


        /// <summary>
        /// 未登録のゲームオブジェクトをコレクションに登録する
        /// </summary>
        private bool RegistTactileObject(ObjectFeatureWrap a_VisionObjectFeatureData)
        {
            bool isResisted = false;

            // 視界オブジェクト情報を作成する
            TactileObjectWrap tactileObjectWrap = new TactileObjectWrap(a_VisionObjectFeatureData);

            // オブジェクトの存在チェック
            bool isContain = p_Monitor.ContainsKey(a_VisionObjectFeatureData.ObjectID);

            if (!isContain)
            {
                // コレクションに追加する
                p_Monitor.Add(tactileObjectWrap);
            }

            isResisted = true;

            return isResisted;
        }

        /// <summary>
        /// 未検出のゲームオブジェクトをコレクションから除去する
        /// </summary>
        private bool RemoveTactileObject(List<int> a_CheckObjectIDList)
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
