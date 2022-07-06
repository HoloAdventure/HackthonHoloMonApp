using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Data.Settings.Length;
using HoloMonApp.Content.Character.Data.Conditions.Body;

namespace HoloMonApp.Content.Character.Model.Parameters.Settings.Length
{
    /// <summary>
    /// 個性設定API
    /// </summary>
    public class HoloMonSettingsLengthAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンの個別設定データの参照
        /// </summary>
        [SerializeField, Tooltip("ホロモンの個別設定データの参照")]
        private HoloMonSettingLengthData p_HoloMonSettingLengthData;

        /// <summary>
        /// ホロモンのアイデンティティのReadOnlyReactivePropertyの参照用変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonLengthIdentity> ReactivePropertyStatus
            => p_HoloMonSettingLengthData.IReadOnlyReactivePropertyIdentity;

        /// <summary>
        /// ホロモンのボディコンディションデータの参照
        /// </summary>
        [SerializeField]
        private HoloMonConditionBodyData p_HoloMonConditionBodyData;



        /// <summary>
        /// ホロモンの縦幅(基本サイズ1.0f時)の短縮参照用変数
        /// </summary>
        public float HoloMonHeightLength => p_HoloMonSettingLengthData.Identity.HoloMonHeightLength;

        /// <summary>
        /// ホロモンの縦幅(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualHeightLength =>
            p_HoloMonSettingLengthData.Identity.HoloMonHeightLength *
            p_HoloMonConditionBodyData.Status.BodyHeight.Value;


        /// <summary>
        /// ホロモンの横幅(基本サイズ1.0f時)の短縮参照用変数
        /// </summary>
        public float HoloMonWideLength => p_HoloMonSettingLengthData.Identity.HoloMonWideLength;

        /// <summary>
        /// ホロモンの横幅(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualWideLength =>
            p_HoloMonSettingLengthData.Identity.HoloMonWideLength *
            p_HoloMonConditionBodyData.Status.BodyHeight.Value;


        /// <summary>
        /// ホロモンの奥幅(基本サイズ1.0f時)の短縮参照用変数
        /// </summary>
        public float HoloMonDepthLength => p_HoloMonSettingLengthData.Identity.HoloMonDepthLength;

        /// <summary>
        /// ホロモンの奥幅(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualDepthLength =>
            p_HoloMonSettingLengthData.Identity.HoloMonDepthLength *
            p_HoloMonConditionBodyData.Status.BodyHeight.Value;


        /// <summary>
        /// ホロモンの手が届く距離(基本サイズ1.0f時)の短縮参照用変数
        /// </summary>
        public float HoloMonHandReachableLength => p_HoloMonSettingLengthData.Identity.HoloMonHandReachableLength;

        /// <summary>
        /// ホロモンの手が届く距離(実寸サイズ)の短縮参照用変数
        /// </summary>
        public float HoloMonActualHandReachableLength =>
            p_HoloMonSettingLengthData.Identity.HoloMonHandReachableLength *
            p_HoloMonConditionBodyData.Status.BodyHeight.Value;



        /// <summary>
        /// 設定の初期化
        /// </summary>
        public void InitializeSetting()
        {
            p_HoloMonSettingLengthData.InitializeSetting();
        }
    }
}