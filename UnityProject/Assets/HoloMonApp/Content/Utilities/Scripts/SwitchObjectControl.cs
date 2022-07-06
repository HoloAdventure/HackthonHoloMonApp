using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.UtilitiesSpace
{
    public class SwitchObjectControl : MonoBehaviour
    {
        [SerializeField, Tooltip("切り替え対象オブジェクトリスト")]
        private List<GameObject> p_SwitchObjectList;

        [SerializeField, Tooltip("現在のインデックス番号")]
        private int p_ActiveIndex;

        // Start is called before the first frame update
        void Start()
        {
            // 指定のインデックスのオブジェクトのみを有効化する
            ActiveIndexObject(p_ActiveIndex);
        }

        /// <summary>
        /// 次の表示インデックスオブジェクトに表示を切り替える
        /// </summary>
        public void Next()
        {
            // 次の表示インデックス番号を取得する
            int showIndex = p_ActiveIndex + 1;
            if (showIndex >= p_SwitchObjectList.Count) showIndex = 0;

            // 指定のインデックスのオブジェクトのみを有効化する
            ActiveIndexObject(showIndex);
        }

        /// <summary>
        /// 次の表示インデックスオブジェクトに表示を切り替える
        /// </summary>
        public void Prev()
        {
            // 1つ前の表示インデックス番号を取得する
            int showIndex = p_ActiveIndex - 1;
            if (showIndex < 0) showIndex = p_SwitchObjectList.Count - 1;

            // 指定のインデックスのオブジェクトのみを有効化する
            ActiveIndexObject(showIndex);
        }

        /// <summary>
        /// 指定インデックスのオブジェクトに表示を切り替える
        /// </summary>
        /// <param name="a_Index"></param>
        public void ActiveIndexObject(int a_Index)
        {
            if (a_Index > p_SwitchObjectList.Count - 1) return;

            // 全てのオブジェクトを一旦非表示にする
            Clear();

            // 指定のインデックスオブジェクトを表示する
            p_SwitchObjectList[a_Index].SetActive(true);

            p_ActiveIndex = a_Index;
        }

        /// <summary>
        /// 全オブジェクトを非表示にする
        /// </summary>
        public void Clear()
        {
            foreach (GameObject obj in p_SwitchObjectList)
            {
                obj.SetActive(false);
            }
            p_ActiveIndex = -1;
        }
    }
}