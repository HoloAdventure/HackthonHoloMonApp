using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Interrupt.Commands
{
    /// <summary>
    /// コマンドによって行動目的を決定する
    /// </summary>
    public class HoloMonInterruptCommandsAPI : MonoBehaviour, HoloMonInterruptIF
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
        /// ホロモンが認識したコマンド(最新)
        /// </summary>
        [SerializeField, Tooltip("ホロモンが認識したコマンド(最新)")]
        private HoloMonCommandReactiveProperty p_HoloMonCommand
            = new HoloMonCommandReactiveProperty(HoloMonCommand.Nothing);

        /// <summary>
        /// ホロモンが認識したコマンド(最新)のReadOnlyReactivePropertyの保持変数
        /// </summary>
        private IReadOnlyReactiveProperty<HoloMonCommand> p_IReadOnlyReactivePropertyHoloMonCommand;

        /// <summary>
        /// ホロモンが認識したコマンド(最新)のReadOnlyReactivePropertyの参照変数
        /// </summary>
        public IReadOnlyReactiveProperty<HoloMonCommand> IReadOnlyReactivePropertyHoloMonCommand
            => p_IReadOnlyReactivePropertyHoloMonCommand
            ?? (p_IReadOnlyReactivePropertyHoloMonCommand = p_HoloMonCommand.ToSequentialReadOnlyReactiveProperty());


        /// <summary>
        /// ホロモンが今まで認識したコマンドのリスト
        /// </summary>
        [SerializeField, Tooltip("ホロモンが今まで認識したコマンドのリスト")]
        private List<HoloMonCommand> p_UntilNowCommands = new List<HoloMonCommand>();


        /// <summary>
        /// 時刻判定(分刻み)のトリガー
        /// </summary>
        private IDisposable p_MinuteTimeTrigger;


        /// <summary>
        /// ホロモンAIの有効無効の切り替え
        /// </summary>
        public void ChangeHoloMonEnableAI(bool a_OnOff)
        {
            // AIの有効無効の切り替えコマンドとして分類する
            ReceptionCommand(HoloMonCommand.ChangeEnableAI);

            // ホロモンAIにAIの有効無効の切り替え要求を行う
            p_Reference.AI.ChangeEnableAI(a_OnOff);
        }

        /// <summary>
        /// NavMeshパスライン描画の切り替え
        /// </summary>
        public void SwitchHoloMonNavMeshPathLine()
        {
            // ステータス変更コマンドとして分類する
            ReceptionCommand(HoloMonCommand.ChnageStatus);

            // TODO: 未実装
        }

        /// <summary>
        /// 遠距離(Far)操作の切り替え
        /// </summary>
        public void SwitchHoloMonFarManipulation()
        {
            // ステータス変更コマンドとして分類する
            ReceptionCommand(HoloMonCommand.ChnageStatus);

            // TODO: 未実装
        }

        /// <summary>
        /// ホロモンの位置を指定のトランスフォーム座標にリセットする
        /// </summary>
        public void ResetPosition(Transform a_ResetPosition)
        {
            // 位置リセットコマンドとして分類する
            ReceptionCommand(HoloMonCommand.ResetPosition);

            // 直接リセットを行う
            p_Reference.Control.BodyComponentsToRigidbodyAPI.ResetVelocity();
            p_Reference.Control.BodyComponentsToTransformUtilityAPI.SetPosition(a_ResetPosition.position);
            p_Reference.Control.SupportItemsStandAPI.ResetPosition(a_ResetPosition.position);
        }

        /// <summary>
        /// ホロモンスケールの変更
        /// </summary>
        public void ChangeHoloMonModelScale(float a_Scale)
        {
            // スケール変更コマンドとして分類する
            ReceptionCommand(HoloMonCommand.ChnageStatus);

            // 直接スケールを変更する
            p_Reference.Control.ConditionsBodyAPI.SetHeight(a_Scale);
        }
        

        /// <summary>
        /// スタンバイさせる
        /// </summary>
        public void Standby()
        {
            // スタンバイコマンドとして分類する
            ReceptionCommand(HoloMonCommand.Standby);

            // ホロモンAIにスタンバイの要求を行う
            p_Reference.AI.RequestStandby();
        }

        /// <summary>
        /// 追跡の開始
        /// </summary>
        public void StartComeHere()
        {
            // 追跡開始コマンドとして分類する
            ReceptionCommand(HoloMonCommand.ComeHere);

            // プレイヤーを追跡する
            p_Reference.AI.RequestTrackingPlayer();
        }

        /// <summary>
        /// じゃんけんの開始
        /// </summary>
        public void StartJanken()
        {
            // じゃんけん開始コマンドとして分類する
            ReceptionCommand(HoloMonCommand.StartJanken);

            // ホロモンAIにジャンケン開始の要求を行う
            p_Reference.AI.RequestJanken();
        }


        /// <summary>
        /// 眠りの開始
        /// </summary>
        public void StartSleep()
        {
            // 眠り開始コマンドとして分類する
            ReceptionCommand(HoloMonCommand.StartSleep);

            // ホロモンAIに眠り開始の要求を行う
            p_Reference.AI.RequestStartSleep();
        }

        /// <summary>
        /// ダンスの開始
        /// </summary>
        public void StartDance()
        {
            // ダンス開始コマンドとして分類する
            ReceptionCommand(HoloMonCommand.StartDance);

            // ホロモンAIにダンス開始の要求を行う
            p_Reference.AI.RequestDance();
        }

        /// <summary>
        /// 待ての開始
        /// </summary>
        public void StartWait()
        {
            // 待て開始コマンドとして分類する
            ReceptionCommand(HoloMonCommand.StartWait);

            // ホロモンAIに待て開始の要求を行う
            p_Reference.AI.RequestStayWait();
        }

        /// <summary>
        /// 食事を与える
        /// </summary>
        public void GiveFood(GameObject a_FoodObject)
        {
            // 種別情報を取得する
            ObjectFeatureWrap objectData = a_FoodObject.GetComponent<ObjectFeatures>().DataWrap;
            if (objectData == null) return;

            // 食事を与えるコマンドとして分類する
            ReceptionCommand(HoloMonCommand.GiveFood);

            // ホロモンAIに食事を与える行動の要求を行う
            p_Reference.AI.RequestGiveFood(objectData);
        }

        /// <summary>
        /// ボールを与える
        /// </summary>
        public void GiveBall(GameObject a_BallObject)
        {
            // 種別情報を取得する
            ObjectFeatureWrap objectData = a_BallObject.GetComponent<ObjectFeatures>().DataWrap;
            if (objectData == null) return;

            // ボールを与えるコマンドとして分類する
            ReceptionCommand(HoloMonCommand.GiveBall);

            // ホロモンAIにボールを与える行動の要求を行う
            p_Reference.AI.RequestGiveBall(objectData);
        }


        /// <summary>
        /// ホロモンのデータを読み込む
        /// </summary>
        public void LoadData()
        {
            // データ読み込みコマンドとして分類する
            ReceptionCommand(HoloMonCommand.LoadData);

            // 直接データを読み込む要求を行う
            p_Reference.SaveLoad.StoragesAPI.LoadData();
        }

        /// <summary>
        /// ホロモンのデータを書き込む
        /// </summary>
        public void SaveData()
        {
            // データ書き込みコマンドとして分類する
            ReceptionCommand(HoloMonCommand.SaveData);

            // 直接データを読み込む要求を行う
            p_Reference.SaveLoad.StoragesAPI.SaveData();
        }

        #region "private"
        /// <summary>
        /// コマンドの受付
        /// </summary>
        /// <param name="a_Command"></param>
        private void ReceptionCommand(HoloMonCommand a_Command)
        {
            // 変数の登録とリストへの追加を行う
            // 同じコマンドの設定を行う場合も SetValueAndForceNotify で強制的に通知を飛ばす
            p_HoloMonCommand.SetValueAndForceNotify(a_Command);
            p_UntilNowCommands.Add(a_Command);
            if (p_UntilNowCommands.Count > 10) p_UntilNowCommands.RemoveAt(0);
        }
        #endregion
    }
}