using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Settings.Personal
{
    public class HoloMonSettingPersonalData : MonoBehaviour
    {
        /// <summary>
        /// ホロモンのアイデンティティ設定
        /// </summary>
        [SerializeField, Tooltip("ホロモンのアイデンティティ設定")]
        private HoloMonPersonalIdentityReactiveProperty p_HoloMonPersonalIdentity
            = new HoloMonPersonalIdentityReactiveProperty();

        /// <summary>
        /// ホロモンのアイデンティティ設定のReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<HoloMonPersonalIdentity> p_IReadOnlyReactivePropertyHoloMonPersonalIdentity;

        /// <summary>
        /// ホロモンのアイデンティティ設定のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonPersonalIdentity> IReadOnlyReactivePropertyIdentity
            => p_IReadOnlyReactivePropertyHoloMonPersonalIdentity
            ?? (p_IReadOnlyReactivePropertyHoloMonPersonalIdentity
            = p_HoloMonPersonalIdentity.ToSequentialReadOnlyReactiveProperty());

        /// <summary>
        /// ホロモンのアイデンティティ設定のデータ参照変数
        /// </summary>
        public HoloMonPersonalIdentity Identity => p_HoloMonPersonalIdentity.Value;


        /// <summary>
        /// 設定の初期化
        /// </summary>
        public void InitializeSetting()
        {
        }
    }
}