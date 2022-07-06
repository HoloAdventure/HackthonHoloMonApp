using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.MenuViewSpace
{
    public class SwitchOnOffObjectController : MonoBehaviour
    {
        [SerializeField, Tooltip("現在のスイッチ状態")]
        private bool p_SwitchFlg;

        [SerializeField, Tooltip("True時オブジェクト")]
        private GameObject p_TrueObject;

        [SerializeField, Tooltip("False時オブジェクト")]
        private GameObject p_FalseObject;

        /// <summary>
        /// 状態の切り替え
        /// </summary>
        /// <param name="a_OnOff"></param>
        public void ObjectSwitch(bool a_OnOff)
        {
            p_SwitchFlg = a_OnOff;

            p_TrueObject.SetActive(a_OnOff);
            p_FalseObject.SetActive(!a_OnOff);
        }
    }
}