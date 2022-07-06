using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Sensations.FieldOfVision;

namespace HoloMonApp.Content.Character.Model.Sensations.FieldOfVision
{
    /// <summary>
    /// 視界内オブジェクトリストを表すReactiveDictionary
    /// </summary>
    [Serializable]
    public class VisionObjectWrapReactiveProperty : ReactiveProperty<VisionObjectWrap>
    {
        public VisionObjectWrapReactiveProperty()
        {
        }
        public VisionObjectWrapReactiveProperty(VisionObjectWrap a_VisionObjectWrap) : base(a_VisionObjectWrap)
        {
        }
    }
}
