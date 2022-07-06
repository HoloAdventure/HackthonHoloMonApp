using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.AI;
using UniRx;

namespace HoloMonApp.Content.MenuViewSpace
{
    public class SwitchControlMenuContainer : MonoBehaviour
    {
        [SerializeField, Tooltip("通常時の切り替えメニュー対象")]
        private List<GameObject> p_SwitchNormalMenuObjects;

        [SerializeField, Tooltip("現在のインデックス番号")]
        private int p_ActiveIndex;

        // Start is called before the first frame update
        void Start()
        {
            // 最初にトップのオブジェクトを表示するためインデックスを-1にする
            p_ActiveIndex = -1;

            // 表示ボタンの切り替えを行う
            SwitchButton();
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// 表示ボタンの切り替えを行う
        /// </summary>
        public void SwitchButton()
        {
            // 次の表示インデックス番号を取得する
            int showIndex = p_ActiveIndex + 1;

            if (showIndex >= CurrentTargetList().Count)
            {
                // インデックスを超えたら1つめに戻る
                showIndex = 0;
            }

            ActiveIndexButton(showIndex);
        }

        /// <summary>
        /// 指定インデックスのボタンを表示する
        /// </summary>
        public void ActiveIndexButton(int a_Index)
        {
            // サイズを超えるものは処理しない
            if (a_Index >= CurrentTargetList().Count) return;

            // 一旦すべてのオブジェクトを無効化する
            AllInactive();

            // 指定のインデックスオブジェクトを有効化する
            CurrentTargetList()[a_Index].SetActive(true);

            // 現在の表示インデックスを設定する
            p_ActiveIndex = a_Index;
        }

        /// <summary>
        /// 非表示時にボタンの再表示を行う
        /// </summary>
        public void TryActive()
        {
            // 非表示状態であれば表示切替を行う
            if(p_ActiveIndex == -1)
            {
                SwitchButton();
            }
        }

        /// <summary>
        /// 全てのオブジェクトを無効化する
        /// </summary>
        public void AllInactive()
        {
            foreach (GameObject obj in CurrentTargetList())
            {
                obj.SetActive(false);
            }
            // インデックス番号を初期化する
            p_ActiveIndex = -1;
        }


        /// <summary>
        /// 指定の表示ボタンへの切り替えを行う
        /// </summary>
        public void ShowTargetButton(GameObject targetObject)
        {
            // 指定インデックス
            int showIndex = -1;

            // 全てのオブジェクトの中から一致するオブジェクトをチェックする
            for(int index = 0; index < CurrentTargetList().Count; index++)
            {
                if(targetObject == CurrentTargetList()[index])
                {
                    showIndex = index;
                }
            }

            if (showIndex >= 0)
            {
                // 一旦すべてのオブジェクトを無効化する
                AllInactive();

                CurrentTargetList()[showIndex].SetActive(true);

                // 現在の表示インデックスを設定する
                p_ActiveIndex = showIndex;
            }
        }

        /// <summary>
        /// 現在参照するゲームオブジェクトリストの取得
        /// </summary>
        private List<GameObject> CurrentTargetList()
        {
            return p_SwitchNormalMenuObjects;
        }
    }
}