using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Words;
using HoloMonApp.Content.Character.Model.Sensations.ListenVoice;

namespace HoloMonApp.Content.Character.View.Sensations.ListenVoice
{
    /// <summary>
    /// 声認識システムAPI
    /// </summary>
    public class HoloMonViewListenVoiceAPI : MonoBehaviour
    {
        /// <summary>
        /// APIの参照
        /// </summary>
        [SerializeField]
        private HoloMonListenVoiceAPI p_ListenVoiceAPI;

        /// <summary>
        /// ホロモンが認識した言葉(最新)のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonListenWord> ReactivePropertyHoloMonListenWord
            => p_ListenVoiceAPI.IReadOnlyReactivePropertyHoloMonListenWord;
    }
}