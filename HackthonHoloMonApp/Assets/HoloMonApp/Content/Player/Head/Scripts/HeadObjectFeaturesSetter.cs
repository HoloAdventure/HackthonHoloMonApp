using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Player
{
    [RequireComponent(typeof(ObjectFeatures))]
    public class HeadObjectFeaturesSetter : MonoBehaviour
    {
        [SerializeField, Tooltip("顔の状態判定ロジックの参照")]
        private HeadGestureRecognizer p_HeadGestureRecognizer;

        [SerializeField, Tooltip("高さ計測ロジックの参照")]
        private HeadHeightChecker p_HeadHeightChecker;

        // オブジェクトの特徴の設定参照
        private ObjectFeatures p_ObjectFeatures;


        /// <summary>
        /// 起動処理
        /// </summary>
        void Start()
        {
            // オブジェクトの特徴の参照を取得する
            p_ObjectFeatures = this.gameObject.GetComponent<ObjectFeatures>();

            // 初期状態は通常状態
            SettingNormal();

            // 判定イベントを登録する
            // MEMO : Updateで参照する形式に変更
            //p_HeadGestureRecognizer.EventNothing += SettingNormal;
            //p_HeadGestureRecognizer.EventNod += SettingNod;
            //p_HeadGestureRecognizer.EventShake += SettingShake;
            //p_HeadGestureRecognizer.EventTiltLeft += SettingTiltLeft;
            //p_HeadGestureRecognizer.EventTiltRight += SettingTiltRight;
        }

        private void LateUpdate()
        {
            // 現在ジェスチャーを行っているか
            switch (p_HeadGestureRecognizer.CurrentStatus)
            {
                case HeadGestureRecognizer.HeadGestureStatus.Nod:
                    SettingNod();
                    return;
                case HeadGestureRecognizer.HeadGestureStatus.Shake:
                    SettingShake();
                    return;
                case HeadGestureRecognizer.HeadGestureStatus.TiltLeft:
                    SettingTiltLeft();
                    return;
                case HeadGestureRecognizer.HeadGestureStatus.TiltRight:
                    SettingTiltRight();
                    return;
            }
            // ジェスチャーを行っていなければ高さで寝転び判定を行う
            if (p_HeadHeightChecker.CurrentHeight < 0.5f)
            {
                SettingLieDown();
                return;
            }

            // どの状態にも一致しない場合
            SettingNormal();
            return;
        }


        /// <summary>
        /// 状態を通常状態に設定する
        /// </summary>
        private void SettingNormal()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHead(ObjectStatusHead.Nothing);
        }

        /// <summary>
        /// 状態を頷き状態に設定する
        /// </summary>
        private void SettingNod()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHead(ObjectStatusHead.Head_Nod);
        }

        /// <summary>
        /// 状態を首振り状態に設定する
        /// </summary>
        private void SettingShake()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHead(ObjectStatusHead.Head_Shake);
        }

        /// <summary>
        /// 状態を首傾げ(左方向)状態に設定する
        /// </summary>
        private void SettingTiltLeft()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHead(ObjectStatusHead.Head_TiltLeft);
        }

        /// <summary>
        /// 状態を首傾げ(右方向)状態に設定する
        /// </summary>
        private void SettingTiltRight()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHead(ObjectStatusHead.Head_TiltRight);
        }

        /// <summary>
        /// 状態を寝転び状態に設定する
        /// </summary>
        private void SettingLieDown()
        {
            p_ObjectFeatures.DataWrap.ChangeStatusHead(ObjectStatusHead.Head_LieDown);
        }
    }
}
