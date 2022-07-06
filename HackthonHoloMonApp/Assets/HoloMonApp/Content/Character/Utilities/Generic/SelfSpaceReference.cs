using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HoloMonApp.Content.Character.Utilities.Generic
{
    public class SelfSpaceReference<T>
    {
        /// <summary>
        /// 保持中のインスタンス
        /// </summary>
        private T p_SelfSpaceInstance;

        /// <summary>
        /// 参照用のインスタンス
        /// </summary>
        public T SelfSpace(Component component)
        {
            if (p_SelfSpaceInstance == null && p_SearchForSelfSpaceInstance)
            {
                p_SearchForSelfSpaceInstance = false;

                // 自身のオブジェクトまたは親オブジェクトからインスタンスを捜索する
                var searchInstance = component.transform.GetComponentInParent<T>();
                if (searchInstance != null)
                {
                    p_SelfSpaceInstance = searchInstance;
                }
                else
                {
                    Debug.LogErrorFormat("Expected exactly {0} but found {1}.", p_SelfSpaceInstance.GetType().ToString(), searchInstance);
                }
            }
            return p_SelfSpaceInstance;
        }

        /// <summary>
        /// インスタンスの検索実行フラグ
        /// </summary>
        private bool p_SearchForSelfSpaceInstance = true;
    }
}