using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.ObjectInteraction.HoldItem
{
    /// <summary>
    /// アイテム保持API
    /// </summary>
    public class HoloMonHoldItemAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("ホロモンアイテム保持用コンポーネント")]
        private HoloMonItemHolder p_ItemHolder;

        /// <summary>
        /// アイテムを保持位置に固定する
        /// </summary>
        /// <param name="a_HoldObject"></param>
        /// <returns></returns>
        public bool PinnedHoldItem(GameObject a_HoldObject)
        {
            // アイテムを保持位置に固定する
            bool result = p_ItemHolder.SetHoldItem(a_HoldObject);
            return result;
        }

        /// <summary>
        /// 保持中のアイテムを解放する
        /// </summary>
        public bool ReleaseHoldItem()
        {
            // 保持中のアイテムを解放する
            bool result = p_ItemHolder.ReleaseHoldItem();
            return result;
        }

        /// <summary>
        /// 保持中のアイテムを消失させる
        /// </summary>
        public bool DisappearHoldItem()
        {
            // 保持中のアイテムを消失させる
            bool result = p_ItemHolder.DisappearHoldItem();
            return result;
        }
    }
}