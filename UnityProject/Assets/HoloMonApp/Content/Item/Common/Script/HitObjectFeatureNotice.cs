using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

// イベント通知のため
using UnityEngine.Events;

// コライダー処理のため
using UniRx;
using UniRx.Triggers;

// ToList使用のため
using System.Linq;

using HoloMonApp.Content.XRPlatform;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.ItemSpace
{
    public class HitObjectFeatureNotice : MonoBehaviour
    {
        /// <summary>
        /// 空間認識レイヤー
        /// </summary>
        private int p_SpatialAwarenessLayer => HoloMonXRPlatform.Instance.EnvironmentMap.Access.GetEnvironmentMapLayer();

        [Serializable]
        private class HitObjectOfFeaturesNoticeEvent : UnityEvent<GameObject>{ }

        /// <summary>
        /// 識別情報あり接触オブジェクトリスト
        /// </summary>
        private Dictionary<int, GameObject> p_HitObjectOfFeaturesDictionary
            = new Dictionary<int, GameObject>();

        [SerializeField, Tooltip("識別情報あり接触オブジェクトリスト(Editor確認用)")]
        private List<GameObject> p_HitObjectOfFeaturesList;

        [SerializeField, Tooltip("ヒットした識別情報ありオブジェクトの通知")]
        private HitObjectOfFeaturesNoticeEvent p_HitAddObjectOfFeaturesNoticeEvent;


        // Start is called before the first frame update
        void Start()
        {
            // コライダーの OnTriggerStay イベントに対する処理を定義する
            this.OnTriggerStayAsObservable()                    // OnTriggerStayイベント
                .BatchFrame()                                   // フレーム毎に値をまとめる
                .ObserveOnMainThread()                          // メインスレッドで行う
                .Subscribe(colliderlist =>
                {
                    // 現在衝突中のコライダーをチェックする
                    bool isUpdated = CheckColliderObjects(colliderlist);
                    // エディター時はリストを更新する
                    if (isUpdated) UpdateList();
                })
                .AddTo(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateList()
        {
# if UNITY_EDITOR
            // Editor確認用
            if (EditorApplication.isPlaying)
            {
                p_HitObjectOfFeaturesList = p_HitObjectOfFeaturesDictionary.Values.ToList();
            }
# endif
        }

        /// <summary>
        /// 衝突中のコライダーをチェックする
        /// </summary>
        private bool CheckColliderObjects(IList<Collider> a_ColliderList)
        {
            // 検出したオブジェクトのリスト
            List<GameObject> objectList = new List<GameObject>();

            // 衝突オブジェクトの登録を行う
            bool isRegisted = false;
            foreach (Collider collider in a_ColliderList)
            {
                GameObject checkObject = collider?.gameObject;
                if (checkObject == null) continue;

                // チェック対象か否か
                if (CheckHitObject(checkObject))
                {
                    // 検出オブジェクトをコレクションに登録する
                    if (RegistHitObject(checkObject))
                    {
                        // 登録したオブジェクトを記録する
                        objectList.Add(checkObject);
                        // ヒットを通知する
                        p_HitAddObjectOfFeaturesNoticeEvent.Invoke(checkObject);
                        isRegisted = true;
                    }
                }
            }

            // 未検出オブジェクトをコレクションから除去する
            bool isRemove = RemoveHitObject(objectList);

            return isRegisted || isRemove;
        }

        /// <summary>
        /// ゲームオブジェクトが触れているか判定する
        /// </summary>
        private bool CheckHitObject(GameObject a_TouchObject)
        {
            // オブジェクトのレイヤー番号を取得する
            int layernumber = a_TouchObject.layer;

            if (layernumber == p_SpatialAwarenessLayer)
            {
                // 空間認識レイヤーの場合は無視する
                return false;
            }

            // 触れていると判定する
            return true;
        }


        /// <summary>
        /// 未登録のゲームオブジェクトをコレクションに登録する
        /// </summary>
        private bool RegistHitObject(GameObject a_RegistObject)
        {
            // オブジェクトの種別理解情報を取得する
            ObjectFeatures registObjectFeatures = a_RegistObject.GetComponent<ObjectFeatures>();

            // 種別理解情報が付与されているか否か
            if (registObjectFeatures == null) return false;

            // 登録済みか否か
            if (!p_HitObjectOfFeaturesDictionary.ContainsKey(a_RegistObject.GetInstanceID()))
            {
                // コレクションに追加する
                p_HitObjectOfFeaturesDictionary.Add(a_RegistObject.GetInstanceID(), a_RegistObject);
            }

            // 登録対象だった場合は登録済みの場合でも true を返す
            return true;
        }

        /// <summary>
        /// 未検出のゲームオブジェクトをコレクションから除去する
        /// </summary>
        private bool RemoveHitObject(List<GameObject> a_CheckObjectList)
        {
            // 削除を行ったか否か
            bool isRemoved = false;

            // オブジェクトInstanceIDのリストに全て変換する
            List<int> checkIdList = a_CheckObjectList.ConvertAll(obj => obj.GetInstanceID());

            foreach (int key in p_HitObjectOfFeaturesDictionary.Keys.ToList())
            {
                if (!checkIdList.Contains(key))
                {
                    // 検出されていなければコレクションから削除する
                    p_HitObjectOfFeaturesDictionary.Remove(key);
                    isRemoved = true;
                }
            }

            return isRemoved;
        }
    }
}