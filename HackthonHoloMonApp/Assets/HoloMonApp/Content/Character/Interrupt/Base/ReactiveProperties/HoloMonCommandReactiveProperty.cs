// Serializable参照のため
using System;

// ReactiveProperty参照のため
using UniRx;

namespace HoloMonApp.Content.Character.Interrupt
{
    /// <summary>
    /// ホロモンコマンドを扱うReactiveProperty
    /// </summary>
    [Serializable]
    public class HoloMonCommandReactiveProperty : ReactiveProperty<HoloMonCommand>
    {
        public HoloMonCommandReactiveProperty()
        {
        }
        public HoloMonCommandReactiveProperty(HoloMonCommand a_HoloMonCommand) : base(a_HoloMonCommand)
        {
        }
    }
}