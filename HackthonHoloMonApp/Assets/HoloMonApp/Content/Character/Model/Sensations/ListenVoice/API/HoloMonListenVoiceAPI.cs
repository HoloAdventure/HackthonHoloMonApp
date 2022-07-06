using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Words;

namespace HoloMonApp.Content.Character.Model.Sensations.ListenVoice
{
    /// <summary>
    /// 声認識システムAPI
    /// </summary>
    public class HoloMonListenVoiceAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンが認識した言葉(最新)
        /// </summary>
        [SerializeField, Tooltip("ホロモンが認識した言葉(最新)")]
        private HoloMonListenReactiveProperty p_HoloMonListenWord
            = new HoloMonListenReactiveProperty(HoloMonListenWord.Nothing);

        /// <summary>
        /// ホロモンが認識した言葉(最新)のReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<HoloMonListenWord> p_IReadOnlyReactivePropertyHoloMonListenWord;

        /// <summary>
        /// ホロモンが認識した言葉(最新)のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonListenWord> IReadOnlyReactivePropertyHoloMonListenWord
            => p_IReadOnlyReactivePropertyHoloMonListenWord
            ?? (p_IReadOnlyReactivePropertyHoloMonListenWord
            = p_HoloMonListenWord.ToSequentialReadOnlyReactiveProperty());


        /// <summary>
        /// ホロモンが今まで認識した言葉のリスト
        /// </summary>
        [SerializeField, Tooltip("ホロモンが今まで認識した言葉のリスト")]
        private List<HoloMonListenWord> p_UntilNowListenWords = new List<HoloMonListenWord>();


        /// <summary>
        /// 言葉の受付
        /// </summary>
        /// <param name="a_ListenWord"></param>
        private void ReceptionListenWord(HoloMonListenWord a_ListenWord)
        {
            // 変数の登録とリストへの追加を行う
            // 同じ言葉の設定を行う場合も SetValueAndForceNotify で強制的に通知を飛ばす
            p_HoloMonListenWord.SetValueAndForceNotify(a_ListenWord);
            p_UntilNowListenWords.Add(a_ListenWord);
            if (p_UntilNowListenWords.Count > 10) p_UntilNowListenWords.RemoveAt(0);
        }

        /// <summary>
        /// ホロモンの名前を呼ばれた場合
        /// </summary>
        public void ListenHoloMonName()
        {
            ReceptionListenWord(HoloMonListenWord.HoloMonName);
        }

        /// <summary>
        /// プレイヤー名を聞いた場合
        /// </summary>
        public void ListenPlayerName()
        {
            ReceptionListenWord(HoloMonListenWord.PlayerName);
        }

        /// <summary>
        /// 肯定・許可の言葉を聞いた場合
        /// </summary>
        public void ListenYes()
        {
            ReceptionListenWord(HoloMonListenWord.Yes);
        }

        /// <summary>
        /// 否定・拒否の言葉を聞いた場合
        /// </summary>
        public void ListenNo()
        {
            ReceptionListenWord(HoloMonListenWord.No);
        }

        /// <summary>
        /// キャンセルの言葉を聞いた場合
        /// </summary>
        public void ListenCancel()
        {
            ReceptionListenWord(HoloMonListenWord.Cancel);
        }

        /// <summary>
        /// プレイヤーの元に来るように言われた場合
        /// </summary>
        public void ListenComeHere()
        {
            ReceptionListenWord(HoloMonListenWord.ComeHere);
        }

        /// <summary>
        /// プレイヤーを見るように言われた場合
        /// </summary>
        public void ListenLookMe()
        {
            ReceptionListenWord(HoloMonListenWord.LookMe);
        }

        /// <summary>
        /// プレイヤーの方向を向くように言われた場合
        /// </summary>
        public void ListenTurnMe()
        {
            ReceptionListenWord(HoloMonListenWord.TurnMe);
        }

        /// <summary>
        /// じゃんけんを持ちかけられた場合
        /// </summary>
        public void ListenJanken()
        {
            ReceptionListenWord(HoloMonListenWord.Janken);
        }

        /// <summary>
        /// ぽん or あいこでしょという言葉を聞いた場合
        /// </summary>
        public void ListenPonSyo()
        {
            ReceptionListenWord(HoloMonListenWord.PonSyo);
        }

        /// <summary>
        /// 踊るように言われた場合
        /// </summary>
        public void ListenStartDance()
        {
            ReceptionListenWord(HoloMonListenWord.StartDance);
        }

        /// <summary>
        /// うんちするように言われた場合
        /// </summary>
        public void ListenShitPutout()
        {
            ReceptionListenWord(HoloMonListenWord.ShitPutout);
        }

        /// <summary>
        /// 待つように言われた場合
        /// </summary>
        public void ListenWait()
        {
            ReceptionListenWord(HoloMonListenWord.Wait);
        }

        /// <summary>
        /// どっちかと問われた場合
        /// </summary>
        public void ListenWhich()
        {
            ReceptionListenWord(HoloMonListenWord.Which);
        }

        /// <summary>
        /// ピースと言われた場合
        /// </summary>
        public void ListenPeace()
        {
            ReceptionListenWord(HoloMonListenWord.Peace);
        }

        /// <summary>
        /// ちょうしを聞かれた場合
        /// </summary>
        public void ListenStatus()
        {
            ReceptionListenWord(HoloMonListenWord.Status);
        }

        /// <summary>
        /// おにくと言われた場合
        /// </summary>
        public void ListenFood()
        {
            ReceptionListenWord(HoloMonListenWord.Food);
        }

        /// <summary>
        /// ボールと言われた場合
        /// </summary>
        public void ListenBall()
        {
            ReceptionListenWord(HoloMonListenWord.Ball);
        }

        /// <summary>
        /// シャワーと言われた場合
        /// </summary>
        public void ListenShower()
        {
            ReceptionListenWord(HoloMonListenWord.Shower);
        }

        /// <summary>
        /// ベッドと言われた場合
        /// </summary>
        public void ListenBed()
        {
            ReceptionListenWord(HoloMonListenWord.Bed);
        }
    }
}