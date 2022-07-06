using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Model.ObjectInteraction.HoldItem;

namespace HoloMonApp.Content.Character.Control.ObjectInteraction.HoldItem
{
    /// <summary>
    /// アイテム保持API
    /// </summary>
    public class HoloMonControlHoldItemAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonHoldItemAPI p_HoldItemAPI;

        /// <summary>
        /// アイテムを保持位置に固定する
        /// </summary>
        /// <param name="a_HoldObject"></param>
        /// <returns></returns>
        public bool PinnedHoldItem(GameObject a_HoldObject)
        {
            bool result = p_HoldItemAPI.PinnedHoldItem(a_HoldObject);
            return result;
        }

        /// <summary>
        /// 保持中のアイテムを解放する
        /// </summary>
        public bool ReleaseHoldItem()
        {
            bool result = p_HoldItemAPI.ReleaseHoldItem();
            return result;
        }

        /// <summary>
        /// 保持中のアイテムを消失させる
        /// </summary>
        public bool DisappearHoldItem()
        {
            bool result = p_HoldItemAPI.DisappearHoldItem();
            return result;
        }
    }
}