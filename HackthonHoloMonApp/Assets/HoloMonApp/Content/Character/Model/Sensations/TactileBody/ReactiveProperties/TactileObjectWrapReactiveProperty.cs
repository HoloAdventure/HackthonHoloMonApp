using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.Model.Sensations.TactileBody
{
    /// <summary>
    /// 触覚内オブジェクトリストを表すReactiveDictionary
    /// </summary>
    [Serializable]
    public class TactileObjectWrapReactiveProperty : ReactiveProperty<TactileObjectWrap>
    {
        public TactileObjectWrapReactiveProperty()
        {
        }
        public TactileObjectWrapReactiveProperty(TactileObjectWrap a_TactileObjectWrap) : base(a_TactileObjectWrap)
        {
        }
    }
}
