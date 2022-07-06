using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.ItemSpace
{
    [RequireComponent(typeof(ObjectFeatures))]
    public class ItemStatusController : MonoBehaviour
    {
        /// <summary>
        /// オブジェクトの特徴参照
        /// </summary>
        private ObjectFeatures p_ObjectFeatures;
        private ObjectFeatures TargetObjectFeatures =>
            p_ObjectFeatures ?? (p_ObjectFeatures = this.GetComponent<ObjectFeatures>());


        /// <summary>
        /// アイテム状態
        /// </summary>
        [SerializeField, Tooltip("アイテム状態")]
        private ObjectStatusItem p_StatusItem;

        /// <summary>
        /// 接地判定距離
        /// </summary>
        [SerializeField, Tooltip("接地判定距離")]
        private float p_FloorDistance;

        /// <summary>
        /// ホールド有無
        /// </summary>
        [SerializeField, Tooltip("ホールド有無")]
        private bool p_IsHold;


        // Start is called before the first frame update
        void Start()
        {
            // 接地判定距離を保存
            CalcFoorDistance();
        }

        // Update is called once per frame
        void Update()
        {
            if (!p_IsHold)
            {
                // ホールド状態でなければ接地状態チェック
                bool onFloor = CheckOnFloor();
                if (onFloor)
                {
                    ChangeStatusItem(ObjectStatusItem.Item_Floor);
                }
                else
                {
                    ChangeStatusItem(ObjectStatusItem.Item_Air);
                }
            }
        }

        /// <summary>
        /// ホロモンによるアイテム保持
        /// </summary>
        public void NoticeHoldByHoloMon(bool onoff)
        {
            if (onoff)
            {
                p_IsHold = true;
                ChangeStatusItem(ObjectStatusItem.Item_HoloMonHold);
            }
            else
            {
                p_IsHold = false;
            }
        }


        /// <summary>
        /// プレイヤーによるアイテム保持
        /// </summary>
        public void NoticeHoldByPlayer(bool onoff)
        {
            if (onoff)
            {
                p_IsHold = true;
                ChangeStatusItem(ObjectStatusItem.Item_PlayerHold);
            }
            else
            {
                p_IsHold = false;
            }
        }

        /// <summary>
        /// 接地距離の取得
        /// </summary>
        private void CalcFoorDistance()
        {
            BoxCollider boxCollider = this.GetComponent<BoxCollider>();

            if (boxCollider != null)
            {
                p_FloorDistance = Mathf.Max(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z) * 1.1f;
            }
            else
            {
                SphereCollider sphereCollider = this.GetComponent<SphereCollider>();

                if (sphereCollider != null)
                {
                    p_FloorDistance = sphereCollider.radius * 1.1f;
                }
                else
                {
                    p_FloorDistance = 0;
                }
            }
        }

        /// <summary>
        /// 接地判定チェック
        /// </summary>
        private bool CheckOnFloor()
        {
            bool onFloor = false;

            // レイキャストの結果
            RaycastHit[] raycastHits = new RaycastHit[2];

            Vector3 rootPosition = this.gameObject.transform.position;
            float currentCheckDistance = p_FloorDistance * this.transform.lossyScale.y;
            Vector3 checkDirection = Vector3.down;

            // レイキャストでその方向の衝突オブジェクトを検知する
            int hitCount = Physics.RaycastNonAlloc(rootPosition, checkDirection, raycastHits, currentCheckDistance);

            if (hitCount > 0)
            {
                // ヒット数が 0 より多いなら間に障害物(床)があるので接地している
                onFloor = true;
            }

            return onFloor;
        }

        /// <summary>
        /// オブジェクト特徴の変更
        /// </summary>
        private bool ChangeStatusItem(ObjectStatusItem a_StatusItem)
        {
            bool isChanged = false;

            isChanged = TargetObjectFeatures.DataWrap.ChangeStatusItem(a_StatusItem);
            p_StatusItem = a_StatusItem;

            return isChanged;
        }
    }
}
