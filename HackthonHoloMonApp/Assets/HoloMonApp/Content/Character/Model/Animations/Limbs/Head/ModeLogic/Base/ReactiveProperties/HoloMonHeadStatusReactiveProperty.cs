// Serializable参照のため
using System;

// ReactiveProperty参照のため
using UniRx;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    /// <summary>
    /// ホロモン頭部アクション種別を扱うReactiveProperty
    /// </summary>
    [Serializable]
    public class HoloMonHeadStatusReactiveProperty : ReactiveProperty<HoloMonActionHeadStatus>
    {
        public HoloMonHeadStatusReactiveProperty()
        {
        }
        public HoloMonHeadStatusReactiveProperty(HoloMonActionHeadStatus a_HoloMonHeadStatus) : base(a_HoloMonHeadStatus)
        {
        }
    }
}