using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace HoloMonApp.Content.Character.Data.Conditions.Body
{
    /// <summary>
    /// ホロモンのボディコンディションを表すReactiveProperty
    /// </summary>
    [Serializable]
    public class HoloMonBodyStatusReactiveProperty : ReactiveProperty<HoloMonBodyStatus>
    {
        public HoloMonBodyStatusReactiveProperty()
        {
        }
        public HoloMonBodyStatusReactiveProperty(HoloMonBodyStatus a_HoloMonBodyStatus) : base(a_HoloMonBodyStatus)
        {
        }
    }
}
