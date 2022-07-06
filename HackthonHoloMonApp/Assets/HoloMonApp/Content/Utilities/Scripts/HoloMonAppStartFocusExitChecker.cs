using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HoloMonApp.Content.UtilitiesSpace
{
    // アプリのフォーカスが外れた場合にアプリを完全終了する
    public class HoloMonAppStartFocusExitChecker : MonoBehaviour
    {
        [SerializeField]
        UnityEvent StartEvent;

        [SerializeField]
        UnityEvent ExitEvent;

        /// <summary>
        /// 起動時のイベント
        /// </summary>
        private void Start()
        {
            StartEvent.Invoke();
        }

        /// <summary>
        /// フォーカス変更時のイベント
        /// </summary>
        /// <param name="focus"></param>
        private void OnApplicationFocus(bool focus)
        {
            // フォーカスが失われた場合はアプリを自動で完全終了する
            if (focus == false)
            {
                ExitEvent.Invoke();
#if WINDOWS_UWP
                Windows.ApplicationModel.Core.CoreApplication.Exit();
#else
                Application.Quit();
#endif
            }
        }
    }
}