using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;

using HoloMonApp.Content.Character.Access;

namespace HoloMonApp.Content.MenuViewSpace
{
    public class ShowStatusPower : MonoBehaviour
    {
        [SerializeField]
        private HoloMonAccessAPI p_HoloMonAccessAPI;

        [SerializeField, Tooltip("テキスト出力先")]
        private TextMeshPro p_TextField;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            p_HoloMonAccessAPI.View.ConditionsBodyAPI.ReactivePropertyStatus
                .ObserveOnMainThread()
                .Subscribe(status => {
                    float power = status.BodyPower.Value;
                    p_TextField.text = power.ToString() + " PJ";
                })
                .AddTo(this);
        }
    }
}