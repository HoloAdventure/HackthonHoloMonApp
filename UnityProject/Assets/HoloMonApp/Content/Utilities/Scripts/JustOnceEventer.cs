using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HoloMonApp.Content.UtilitiesSpace
{
    public class JustOnceEventer : MonoBehaviour
    {
        [SerializeField, Tooltip("アプリ起動後一度だけ発生可能なイベント")]
        private UnityEvent JustOnceEvents;

        /// <summary>
        /// 実行済みフラグ
        /// </summary>
        private bool p_Executed = false;

        /// <summary>
        /// アプリ起動後の一回目の呼び出しのみイベントが実行される
        /// </summary>
        public void CallEvents()
        {
            if (p_Executed) return;

            JustOnceEvents.Invoke();
            p_Executed = true;
        }
    }
}