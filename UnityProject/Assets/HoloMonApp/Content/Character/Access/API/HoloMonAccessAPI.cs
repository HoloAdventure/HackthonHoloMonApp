using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Interrupt;
using HoloMonApp.Content.Character.Interrupt.Commands;
using HoloMonApp.Content.Character.View;

namespace HoloMonApp.Content.Character.Access
{
    public class HoloMonAccessAPI : MonoBehaviour
    {
        /// <summary>
        /// ホロモンの割込みAPIの参照
        /// (外部から参照できるのはコマンド割込みのみ)
        /// </summary>
        [SerializeField]
        private HoloMonInterruptAPI p_Interrupt;
        public HoloMonInterruptCommandsAPI InterruptCommands => p_Interrupt.InterruptCommandsAPI;

        /// <summary>
        /// ホロモンのビューAPIの参照
        /// </summary>
        [SerializeField]
        private HoloMonViewAPI p_View;
        public HoloMonViewAPI View => p_View;


        // 呼出し用関数(ボタン実行)

        /// <summary>
        /// NavMeshパスライン描画の切り替え
        /// </summary>
        public void SwitchHoloMonNavMeshPathLine()
        {
            // TODO: 未実装
            InterruptCommands.SwitchHoloMonNavMeshPathLine();
        }

        /// <summary>
        /// 遠距離(Far)操作の切り替え
        /// </summary>
        public void SwitchHoloMonFarManipulation()
        {
            InterruptCommands.SwitchHoloMonFarManipulation();
        }

        /// <summary>
        /// ホロモンの位置を指定のトランスフォーム座標にリセットする
        /// </summary>
        public void ResetPosition(Transform a_ResetPosition)
        {
            InterruptCommands.ResetPosition(a_ResetPosition);
        }

        /// <summary>
        /// ホロモンスケールの変更
        /// </summary>
        public void ChangeHoloMonModelScale(float a_Scale)
        {
            InterruptCommands.ChangeHoloMonModelScale(a_Scale);
        }
        

        /// <summary>
        /// スタンバイさせる
        /// </summary>
        public void Standby()
        {
            InterruptCommands.Standby();
        }

        /// <summary>
        /// 追跡の開始
        /// </summary>
        public void StartComeHere()
        {
            InterruptCommands.StartComeHere();
        }

        /// <summary>
        /// じゃんけんの開始
        /// </summary>
        public void StartJanken()
        {
            InterruptCommands.StartJanken();
        }


        /// <summary>
        /// 眠りの開始
        /// </summary>
        public void StartSleep()
        {
            InterruptCommands.StartSleep();
        }

        /// <summary>
        /// ダンスの開始
        /// </summary>
        public void StartDance()
        {
            InterruptCommands.StartDance();
        }

        /// <summary>
        /// 待ての開始
        /// </summary>
        public void StartWait()
        {
            InterruptCommands.StartWait();
        }

        /// <summary>
        /// 食事を与える
        /// </summary>
        public void GiveFood(GameObject a_FoodObject)
        {
            InterruptCommands.GiveFood(a_FoodObject);
        }

        /// <summary>
        /// ボールを与える
        /// </summary>
        public void GiveBall(GameObject a_BallObject)
        {
            InterruptCommands.GiveBall(a_BallObject);
        }


        /// <summary>
        /// ホロモンのデータを読み込む
        /// </summary>
        public void LoadData()
        {
            InterruptCommands.LoadData();
        }

        /// <summary>
        /// ホロモンのデータを書き込む
        /// </summary>
        public void SaveData()
        {
            InterruptCommands.SaveData();
        }
    }
}