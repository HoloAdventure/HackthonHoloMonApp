using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Subscribeを行うため、UniRxを参照
using UniRx;

using HoloMonApp.Content.Character.AI;

namespace HoloMonApp.Content.DebugInfoSpace
{
    public class ShowDebugInfoSwitch : MonoBehaviour
    {
        [SerializeField, Tooltip("デバッグ情報表示オブジェクトリスト")]
        private List<GameObject> p_ShowDebugInfoObjectList;

        [SerializeField, Tooltip("デバッグ表示フラグ")]
        private bool p_ShowDebugFlg;

        /// <summary>
        /// デバッグ表示フラグの参照
        /// </summary>
        public bool ShowDebugFlg => p_ShowDebugFlg;

        private void Start()
        {
            this.ChangeShowDebugInfoFlg(p_ShowDebugFlg);
        }

        /// <summary>
        /// デバッグ情報表示フラグ変更に合わせた切り替え
        /// </summary>
        /// <param name="a_RenderingFlg"></param>
        public void ChangeShowDebugInfoFlg(bool a_ShowFlg)
        {
            Debug.Log(this.GetType() + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + ","
                + "a_ShowFlg:" + a_ShowFlg.ToString());

            if (a_ShowFlg)
            {
                EnableDebugMode();
            }
            else
            {
                DisableDebugMode();
            }
        }

        private void EnableDebugMode()
        {
            // 全てのデバッグ用オブジェクトをアクティブにする
            foreach (GameObject showDebugInfoObj in p_ShowDebugInfoObjectList)
            {
                showDebugInfoObj.SetActive(true);
            }
            p_ShowDebugFlg = true;
        }

        private void DisableDebugMode()
        {
            // 全てのデバッグ用オブジェクトを非アクティブにする
            foreach (GameObject showDebugInfoObj in p_ShowDebugInfoObjectList)
            {
                showDebugInfoObj.SetActive(false);
            }
            p_ShowDebugFlg = false;
        }
    }
}