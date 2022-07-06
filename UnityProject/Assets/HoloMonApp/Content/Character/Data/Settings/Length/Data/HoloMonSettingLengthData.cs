using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Settings.Length
{
    public class HoloMonSettingLengthData : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのアイデンティティ設定
        /// </summary>
        [SerializeField, Tooltip("ホロモンのアイデンティティ設定")]
        private HoloMonLengthIdentityReactiveProperty p_HoloMonLengthIdentity
            = new HoloMonLengthIdentityReactiveProperty();

        /// <summary>
        /// ホロモンのアイデンティティ設定のReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<HoloMonLengthIdentity> p_IReadOnlyReactivePropertyHoloMonLengthIdentity;

        /// <summary>
        /// ホロモンのアイデンティティ設定のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonLengthIdentity> IReadOnlyReactivePropertyIdentity
            => p_IReadOnlyReactivePropertyHoloMonLengthIdentity
            ?? (p_IReadOnlyReactivePropertyHoloMonLengthIdentity
            = p_HoloMonLengthIdentity.ToSequentialReadOnlyReactiveProperty());

        /// <summary>
        /// ホロモンのアイデンティティ設定のデータ参照変数
        /// </summary>
        public HoloMonLengthIdentity Identity => p_HoloMonLengthIdentity.Value;


        /// <summary>
        /// 設定の初期化
        /// </summary>
        public void InitializeSetting()
        {
        }
    }
}