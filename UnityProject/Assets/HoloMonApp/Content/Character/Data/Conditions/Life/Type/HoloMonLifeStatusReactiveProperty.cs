using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Conditions.Life
{
    /// <summary>
    /// ホロモンのライフコンディションを表すReactiveProperty
    /// </summary>
    [Serializable]
    public class HoloMonLifeStatusReactiveProperty : ReactiveProperty<HoloMonLifeStatus>
    {
        public HoloMonLifeStatusReactiveProperty()
        {
        }
        public HoloMonLifeStatusReactiveProperty(HoloMonLifeStatus a_HoloMonLifeStatus) : base(a_HoloMonLifeStatus)
        {
        }
    }
}
