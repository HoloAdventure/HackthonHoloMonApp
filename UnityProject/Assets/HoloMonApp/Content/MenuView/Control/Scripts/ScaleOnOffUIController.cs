using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

using HoloMonApp.Content.Character.Access;

namespace HoloMonApp.Content.MenuViewSpace
{
    public class ScaleOnOffUIController : MonoBehaviour
    {
        [SerializeField]
        private HoloMonAccessAPI p_HoloMonAccessAPI;

        [SerializeField]
        private bool p_CheckFlg;

        [SerializeField]
        private List<SwitchOnOffObjectController> p_SwichList;

        [SerializeField]
        private List<float> p_BorderList;

        private void Start()
        {
#if UNITY_EDITOR
            // Editor確認用
            // エディター実行時は全てのボタンを有効化する
            p_CheckFlg = false;
#endif

            // 初期状態をチェックする
            CheckPower(p_HoloMonAccessAPI.View.ConditionsBodyAPI.BodyPower);

            // ちから状態の変化時の処理を設定する
            p_HoloMonAccessAPI.View.ConditionsBodyAPI.ReactivePropertyStatus
                .ObserveOnMainThread()
                .Subscribe(status =>
                {
                    CheckPower(status.BodyPower.Value);
                })
                .AddTo(this);
        }

        /// <summary>
        /// ちからに合わせて状態を変更
        /// </summary>
        public void CheckPower(float a_Power)
        {
            if (!p_CheckFlg)
            {
                // チェックしない場合は全て有効状態にしておく
                for (int switchIndex = 0; switchIndex < p_SwichList.Count; switchIndex++)
                {
                    p_SwichList[switchIndex].ObjectSwitch(true);
                }
                return;
            }

            int nowOnIndex = 0;

            for(int borderIndex = 0; borderIndex < p_BorderList.Count; borderIndex++)
            {
                if (a_Power > p_BorderList[borderIndex])
                {
                    nowOnIndex = borderIndex + 1;
                }
                else
                {
                    break;
                }
            }

            for(int switchIndex = 0; switchIndex < p_SwichList.Count; switchIndex++)
            {
                if (switchIndex < nowOnIndex)
                {
                    p_SwichList[switchIndex].ObjectSwitch(true);
                }
                else
                {
                    p_SwichList[switchIndex].ObjectSwitch(false);
                }
            }
        }
    }
}