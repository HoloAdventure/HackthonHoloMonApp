using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.AI;
using UniRx;

namespace HoloMonApp.Content.MenuViewSpace
{
    public class DebugSwitchControlMenuContainer : MonoBehaviour
    {
        [SerializeField, Tooltip("通常時の切り替えメニュー対象")]
        private List<GameObject> p_SwitchNormalMenuObjects;

        [SerializeField, Tooltip("デバッグ時の切り替えメニュー対象")]
        private List<GameObject> p_SwitchDebugMenuObjects;

        [SerializeField, Tooltip("現在のインデックス番号")]
        private int p_ActiveIndex;

        [SerializeField, Tooltip("現在のデバッグフラグ")]
        private int p_DebugModeFlg = 0;


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
        /// デバッグモードのフラグの加算処理を行う
        /// </summary>
        public void DebugModeFlgAdd()
        {
            // フラグへの加算を行う
            p_DebugModeFlg += 2;

            // 5以上になればリセット
            if (p_DebugModeFlg > 5) p_DebugModeFlg = 0;
        }

        /// <summary>
        /// デバッグモードのフラグの減算処理を行う
        /// </summary>
        public void DebugModeFlgSub()
        {
            // フラグへの減算を行う
            p_DebugModeFlg -= 1;

            // 0以下になればリセット
            if (p_DebugModeFlg < 0) p_DebugModeFlg = 0;
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
            // デバッグモードか否かで参照するゲームオブジェクトリストを切り替える
            // フラグが 5 のときのみ、デバッグモードと判定する
            if(p_DebugModeFlg == 5)
            {
                return p_SwitchDebugMenuObjects;
            }
            else
            {
                return p_SwitchNormalMenuObjects;
            }
        }
    }
}