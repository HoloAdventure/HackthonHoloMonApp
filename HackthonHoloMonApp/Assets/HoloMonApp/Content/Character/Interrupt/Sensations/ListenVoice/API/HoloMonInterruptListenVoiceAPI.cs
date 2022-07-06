using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.AI;

using HoloMonApp.Content.Character.Control;
using HoloMonApp.Content.Character.Control.VisualizeUIs;
using HoloMonApp.Content.Character.View;
using HoloMonApp.Content.Character.Data.Knowledge.Words;

namespace HoloMonApp.Content.Character.Interrupt.Sensations.ListenVoice
{
    /// <summary>
    /// 音声認識によって行動を決定する
    /// </summary>
    public class HoloMonInterruptListenVoiceAPI : MonoBehaviour, HoloMonInterruptIF
    {
        /// <summary>
        /// 共通参照
        /// </summary>
        private HoloMonInterruptReference p_Reference;

        /// <summary>
        /// 初期化
        /// </summary>
        public void AwakeInit(HoloMonInterruptReference reference)
        {
            p_Reference = reference;
        }


        /// <summary>
        /// 開始処理
        /// </summary>
        void Start()
        {
            // 音声聞き取り時の処理を設定する
            p_Reference.View.SensationsListenVoiceAPI
                .ReactivePropertyHoloMonListenWord
                .ObserveOnMainThread()
                .Subscribe(word => {
                    ListenWord(word);
                })
                .AddTo(this);
        }

        /// <summary>
        /// 割込み情報データを作成する
        /// </summary>
        /// <param name="a_ListenWord"></param>
        /// <returns></returns>
        private InterruptInformation MakeInterruptInfo(HoloMonListenWord a_ListenWord)
        {
            return new InterruptInformation(new InterruptListenVoiceData(a_ListenWord));
        }

        /// <summary>
        /// 聞き取った言葉に合わせたアクションを実行する
        /// </summary>
        /// <param name="a_ListenWord"></param>
        private async void ListenWord(HoloMonListenWord a_ListenWord)
        {
            // 言霊の到達UIを表示して完了を待機する
            bool result = await p_Reference.Control.VisualizeUIsWordBubbleAPI.ArriveStartAsync();

            // 割込み情報を作成する
            InterruptInformation interruptInfo = MakeInterruptInfo(a_ListenWord);

            // 現在のアクションに発生した割込み情報を通知する
            bool isProcessed = p_Reference.AI.TransmissionInterrupt(interruptInfo);

            if (isProcessed)
            {
                // アクションで割込み情報が処理された場合は後の処理を行わない
                Debug.Log("Listen Processed !!");

                // 感情のびっくりUIを表示する(完了を待機しない)
                p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
                return;
            }

            bool isPorposed = false;
            switch (a_ListenWord)
            {
                case HoloMonListenWord.Nothing:
                    break;
                case HoloMonListenWord.HoloMonName:
                    // 名前を呼ばれた場合
                    // プレイヤーの方を向く
                    isPorposed = CallReactionHoloMonName();
                    break;
                case HoloMonListenWord.PlayerName:
                    // プレイヤー名を聞いた場合
                    isPorposed = CallReactionPlayerName();
                    break;
                case HoloMonListenWord.Yes:
                    // 肯定の言葉を聞いた場合
                    isPorposed = CallReactionYes();
                    break;
                case HoloMonListenWord.No:
                    // 否定の言葉を聞いた場合
                    isPorposed = CallReactionNo();
                    break;
                case HoloMonListenWord.Cancel:
                    // キャンセルの言葉を聞いた場合
                    isPorposed = CallReactionCancel();
                    break;
                case HoloMonListenWord.ComeHere:
                    // プレイヤーの元に来るように言われた場合
                    isPorposed = CallReactionComeHere();
                    break;
                case HoloMonListenWord.LookMe:
                    // プレイヤーを見るように言われた場合
                    isPorposed = CallReactionLookMe();
                    break;
                case HoloMonListenWord.TurnMe:
                    // プレイヤーの方を向くように言われた場合
                    isPorposed = CallReactionTurnMe();
                    break;
                case HoloMonListenWord.Janken:
                    // じゃんけんを持ちかけられた場合
                    isPorposed = CallReactionJanken();
                    break;
                case HoloMonListenWord.PonSyo:
                    // ぽん or あいこでしょという言葉を聞いた場合
                    isPorposed = CallReactionPonSyo();
                    break;
                case HoloMonListenWord.StartDance:
                    // 踊るように言われた場合
                    isPorposed = CallReactionStartDance();
                    break;
                case HoloMonListenWord.ShitPutout:
                    // うんちするように言われた場合
                    isPorposed = CallReactionShitPutout();
                    break;
                case HoloMonListenWord.Wait:
                    // 待つように言われた場合
                    isPorposed = CallReactionWait();
                    break;
                case HoloMonListenWord.Which:
                    // どっちかと問われた場合
                    isPorposed = CallReactionWhich();
                    break;
                case HoloMonListenWord.Peace:
                    // ピースと言われた場合
                    break;
                case HoloMonListenWord.Status:
                    // ちょうしを聞かれた場合
                    break;
                case HoloMonListenWord.Food:
                    // おにくと言われた場合
                    CallSearchFood();
                    break;
                case HoloMonListenWord.Ball:
                    // ボールと言われた場合
                    CallSearchBall();
                    break;
                case HoloMonListenWord.Shower:
                    // シャワーと言われた場合
                    CallSearchShower();
                    break;
                case HoloMonListenWord.Bed:
                    // ベッドと言われた場合
                    CallSearchBed();
                    break;
                default:
                    break;
            }

            if (isPorposed)
            {
                // 感情のびっくりUIを表示する(完了を待機しない)
                p_Reference.Control.VisualizeUIsEmotionPanelAPI.ExclamationStartAsync();
            }
        }

        /// <summary>
        /// 名前を呼ばれた場合のリアクション
        /// </summary>
        private bool CallReactionHoloMonName()
        {
            // プレイヤーの方を向く
            return p_Reference.AI.RequestLookPlayer();
        }

        /// <summary>
        /// プレイヤー名を聞いた場合のリアクション
        /// </summary>
        private bool CallReactionPlayerName()
        {
            return false;
        }

        /// <summary>
        /// 肯定の言葉を聞いた場合のリアクション
        /// </summary>
        private bool CallReactionYes()
        {
            return false;
        }

        /// <summary>
        /// 否定の言葉を聞いた場合のリアクション
        /// </summary>
        private bool CallReactionNo()
        {
            return false;
        }

        /// <summary>
        /// キャンセルの言葉を聞いた場合のリアクション
        /// </summary>
        private bool CallReactionCancel()
        {
            // 待機状態に切り替える
            return p_Reference.AI.RequestStandby();
        }

        /// <summary>
        /// プレイヤーの元に来るように言われた場合のリアクション
        /// </summary>
        private bool CallReactionComeHere()
        {
            // プレイヤーを追跡する
            return p_Reference.AI.RequestTrackingPlayer();
        }

        /// <summary>
        /// プレイヤーを見るように言われた場合のリアクション
        /// </summary>
        private bool CallReactionLookMe()
        {
            return false;
        }

        /// <summary>
        /// プレイヤーの方向を向くように言われた場合のリアクション
        /// </summary>
        private bool CallReactionTurnMe()
        {
            // プレイヤーの方向を向く
            return p_Reference.AI.RequestLookPlayer();
        }

        /// <summary>
        /// じゃんけんを持ちかけられた場合のリアクション
        /// </summary>
        private bool CallReactionJanken()
        {
            // じゃんけんモードに切り替える
            return p_Reference.AI.RequestJanken();
        }

        /// <summary>
        /// ぽん or あいこでしょという言葉を聞いた場合のリアクション
        /// </summary>
        private bool CallReactionPonSyo()
        {
            return false;
        }

        /// <summary>
        /// 踊るように言われた場合のリアクション
        /// </summary>
        private bool CallReactionStartDance()
        {
            // ダンスモードに切り替える
            return p_Reference.AI.RequestDance();
        }

        /// <summary>
        /// うんちするように言われた場合のリアクション
        /// </summary>
        private bool CallReactionShitPutout()
        {
            // うんちモードに切り替える
            return p_Reference.AI.RequestPutoutShit();
        }

        /// <summary>
        /// 待つように言われた場合のリアクション
        /// </summary>
        private bool CallReactionWait()
        {
            // 待てモードに切り替える
            return p_Reference.AI.RequestStayWait();
        }

        /// <summary>
        /// どっちと問われた場合のリアクション
        /// </summary>
        private bool CallReactionWhich()
        {
            return false;
        }

        /// <summary>
        /// おにくと言われた場合のリアクション
        /// </summary>
        private bool CallSearchFood()
        {
            // おにく捜索モードに切り替える
            return p_Reference.AI.RequestSearchFood();
        }

        /// <summary>
        /// ボールと言われた場合のリアクション
        /// </summary>
        private bool CallSearchBall()
        {
            // ボール捜索モードに切り替える
            return p_Reference.AI.RequestSearchBall();
        }

        /// <summary>
        /// シャワーと言われた場合のリアクション
        /// </summary>
        private bool CallSearchShower()
        {
            // シャワー捜索モードに切り替える
            return p_Reference.AI.RequestSearchShower();
        }

        /// <summary>
        /// ベッドと言われた場合のリアクション
        /// </summary>
        private bool CallSearchBed()
        {
            // ベッド捜索モードに切り替える
            return p_Reference.AI.RequestSearchBed();
        }
    }
}