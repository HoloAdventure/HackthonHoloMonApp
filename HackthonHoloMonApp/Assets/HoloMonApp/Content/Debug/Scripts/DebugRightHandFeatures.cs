using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.DebugInfoSpace
{
    public class DebugRightHandFeatures : MonoBehaviour
    {
        [SerializeField, Tooltip("テキスト出力先")]
        private Text p_DebugText;

        [SerializeField, Tooltip("参照するオブジェクトの特徴")]
        private ObjectFeatures p_ObjectFeatures;

        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            if (p_ObjectFeatures != null)
            {
                p_ObjectFeatures.ObserveEveryValueChanged(x => x.DataWrap.UnderstandInformation) // 特徴値の変化をキャッチする
                    .Subscribe(x =>
                    {
                        p_DebugText.text = p_ObjectFeatures.DataWrap.UnderstandInformation.ToString();
                    })
                    .AddTo(this);
            }
        }
    }
}