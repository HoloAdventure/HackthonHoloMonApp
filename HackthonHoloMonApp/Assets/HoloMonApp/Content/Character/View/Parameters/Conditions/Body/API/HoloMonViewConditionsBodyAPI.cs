using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Model.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.Data.Conditions.Body;

namespace HoloMonApp.Content.Character.View.Parameters.Conditions.Body
{
    /// <summary>
    /// ボディコンディションのAPI
    /// </summary>
    public class HoloMonViewConditionsBodyAPI : MonoBehaviour
    {
        /// <summary>
        /// 実体パラメータの参照
        /// </summary>
        [SerializeField, Tooltip("実体パラメータの参照")]
        private HoloMonConditionsBodyAPI p_ConditionsBodyAPI;

        /// <summary>
        /// ホロモンのボディコンディションのReadOnlyReactivePropertyの参照用変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonBodyStatus> ReactivePropertyStatus
            => p_ConditionsBodyAPI.ReactivePropertyStatus;

        /// <summary>
        /// ホロモンのボディコンディションの短縮参照用変数
        /// </summary>
        public HoloMonBodyStatus BodyStatus => p_ConditionsBodyAPI.ReactivePropertyStatus.Value;

        /// <summary>
        /// ホロモンの誕生日時の短縮参照用変数
        /// </summary>
        public DateTime BirthTime => p_ConditionsBodyAPI.BirthTime;

        /// <summary>
        /// ホロモンの日齢の短縮参照用変数
        /// </summary>
        public int AgeOfDay => p_ConditionsBodyAPI.AgeOfDay;

        /// <summary>
        /// ホロモンの大きさの短縮参照用変数
        /// </summary>
        public float BodyHeight => p_ConditionsBodyAPI.BodyHeight;

        /// <summary>
        /// ホロモンの初期の大きさの短縮参照用変数
        /// </summary>
        public float DefaultBodyHeight => p_ConditionsBodyAPI.DefaultBodyHeight;

        /// <summary>
        /// ホロモンのちからの短縮参照用変数
        /// </summary>
        public float BodyPower => p_ConditionsBodyAPI.BodyPower;

        /// <summary>
        /// ホロモンのデフォルトサイズ比率の短縮参照用変数
        /// </summary>
        public float DefaultBodyHeightRatio => p_ConditionsBodyAPI.DefaultBodyHeightRatio;
    }
}