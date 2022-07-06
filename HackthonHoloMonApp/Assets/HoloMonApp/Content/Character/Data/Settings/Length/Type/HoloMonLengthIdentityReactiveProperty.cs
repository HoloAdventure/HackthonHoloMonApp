using System;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Settings.Length
{
    /// <summary>
    /// 個性設定を表すReactiveProperty
    /// </summary>
    [Serializable]
    public class HoloMonLengthIdentityReactiveProperty : ReactiveProperty<HoloMonLengthIdentity>
    {
        public HoloMonLengthIdentityReactiveProperty()
        {
        }
        public HoloMonLengthIdentityReactiveProperty
            (HoloMonLengthIdentity a_HoloMonLengthIdentity) : base(a_HoloMonLengthIdentity)
        {
        }
    }
}