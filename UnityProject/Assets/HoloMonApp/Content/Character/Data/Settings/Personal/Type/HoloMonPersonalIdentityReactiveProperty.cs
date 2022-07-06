using System;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Settings.Personal
{
    /// <summary>
    /// 個性設定を表すReactiveProperty
    /// </summary>
    [Serializable]
    public class HoloMonPersonalIdentityReactiveProperty : ReactiveProperty<HoloMonPersonalIdentity>
    {
        public HoloMonPersonalIdentityReactiveProperty()
        {
        }
        public HoloMonPersonalIdentityReactiveProperty
            (HoloMonPersonalIdentity a_HoloMonPersonalIdentity) : base(a_HoloMonPersonalIdentity)
        {
        }
    }
}