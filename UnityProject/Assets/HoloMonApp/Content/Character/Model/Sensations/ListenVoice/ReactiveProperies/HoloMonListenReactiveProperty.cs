using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ReactiveProperty参照のため
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Words;

namespace HoloMonApp.Content.Character.Model.Sensations.ListenVoice
{
    /// <summary>
    /// ホロモンが認識可能な言葉を扱うReactiveProperty
    /// </summary>
    [Serializable]
    public class HoloMonListenReactiveProperty : ReactiveProperty<HoloMonListenWord>
    {
        public HoloMonListenReactiveProperty()
        {
        }
        public HoloMonListenReactiveProperty(HoloMonListenWord a_HoloMonListenWord) : base(a_HoloMonListenWord)
        {
        }
    }
}