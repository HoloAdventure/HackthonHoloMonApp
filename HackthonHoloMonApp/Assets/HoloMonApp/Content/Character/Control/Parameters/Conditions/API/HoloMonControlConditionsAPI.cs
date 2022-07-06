using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Control.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.Control.Parameters.Conditions.Life;

namespace HoloMonApp.Content.Character.Control.Parameters.Conditions
{
    /// <summary>
    /// コンディションデータAPI
    /// </summary>
    public class HoloMonControlConditionsAPI : MonoBehaviour
    {
        /// <summary>
        /// ボディコンディションの参照
        /// </summary>
        [SerializeField, Tooltip("ボディコンディションの参照")]
        private HoloMonControlConditionsBodyAPI p_Body;
        public HoloMonControlConditionsBodyAPI Body => p_Body;

        /// <summary>
        /// ライフコンディションの参照
        /// </summary>
        [SerializeField, Tooltip("ライフコンディションの参照")]
        private HoloMonControlConditionsLifeAPI p_Life;
        public HoloMonControlConditionsLifeAPI Life => p_Life;
    }
}