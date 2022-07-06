using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.View.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.View.Parameters.Conditions.Life;

namespace HoloMonApp.Content.Character.View.Parameters.Conditions
{
    /// <summary>
    /// コンディションデータAPI
    /// </summary>
    public class HoloMonViewConditionsAPI : MonoBehaviour
    {
        /// <summary>
        /// ボディコンディションの参照
        /// </summary>
        [SerializeField, Tooltip("ボディコンディションの参照")]
        private HoloMonViewConditionsBodyAPI p_Body;
        public HoloMonViewConditionsBodyAPI Body => p_Body;

        /// <summary>
        /// ライフコンディションの参照
        /// </summary>
        [SerializeField, Tooltip("ライフコンディションの参照")]
        private HoloMonViewConditionsLifeAPI p_Life;
        public HoloMonViewConditionsLifeAPI Life => p_Life;
    }
}