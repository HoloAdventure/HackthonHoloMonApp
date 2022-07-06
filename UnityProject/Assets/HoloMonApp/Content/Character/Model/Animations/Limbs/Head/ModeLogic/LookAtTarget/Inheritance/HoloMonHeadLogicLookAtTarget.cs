using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    public class HoloMonHeadLogicLookAtTarget : MonoBehaviour, HoloMonHeadLogicInterface
    {
        /// <summary>
        /// 視線ロジック共通情報
        /// </summary>
        [SerializeField, Tooltip("視線ロジック共通情報")]
        private HoloMonHeadLogicCommon p_HeadLogicCommon = new HoloMonHeadLogicCommon();

        /// <summary>
        /// 現在のモードを取得する
        /// </summary>
        /// <returns></returns>
        public HoloMonActionHeadStatus GetHoloMonHeadStatus() => p_HeadLogicCommon.HeadLogicStatus;

        /// <summary>
        /// EveryValueChanged オブザーバ参照変数を取得する
        /// </summary>
        /// <returns></returns>
        public IObservable<HoloMonActionHeadStatus> GetHoloMonHeadStatusEveryValueChanged()
            => p_HeadLogicCommon.IObservableHeadLogicStatusEveryValueChanged;

        /// <summary>
        /// モード設定を有効化する
        /// </summary>
        public bool EnableHead()
        {
            // 開始処理を行う
            EnableSetting();

            // 開始状態を設定する
            p_HeadLogicCommon.SetHeadStatus(HoloMonActionHeadStatus.Runtime);

            return true;
        }

        /// <summary>
        /// モード設定を無効化する
        /// </summary>
        public bool DisableHead(HoloMonActionHeadStatus a_DisableStatus)
        {
            // 停止処理を行う
            DisableSetting();

            // 停止状態を設定する
            p_HeadLogicCommon.SetHeadStatus(a_DisableStatus);

            return true;
        }

        /// <summary>
        /// アクション設定を反映する
        /// </summary>
        /// <returns></returns>
        public bool ApplySetting(HeadLogicSetting a_HeadLogicSetting)
        {
            // 設定の反映を行う
            ApplyData(a_HeadLogicSetting);

            return true;
        }


        /// <summary>
        /// 設定アクションデータ
        /// </summary>
        [SerializeField, Tooltip("設定アクションデータ")]
        private HeadLogicLookAtTargetData p_Data;

        /// <summary>
        /// ホロモン頭部視点の制御コンポーネント
        /// </summary>
        [SerializeField, Tooltip("ホロモン頭部視点の制御コンポーネント")]
        private HeadLookControllerSimple p_HeadLookController;
        //private HeadLookControllerSmooth p_HeadLookController;

        /// <summary>
        /// HeadLookControllerの各ボーンの最大角デフォルト設定
        /// </summary>
        [SerializeField, Tooltip("HeadLookControllerの各ボーンの最大角デフォルト設定")]
        private float p_DefaultMaxBendingAngles;

        /// <summary>
        /// HeadLookControllerのアニメーションオーバーライドのデフォルト値
        /// </summary>
        [SerializeField, Tooltip("HeadLookControllerのアニメーションオーバーライドのデフォルト値")]
        private bool p_DefaultOverrideAnimation;



        /// <summary>
        /// 定期処理(Late)
        /// </summary>
        void LateUpdate()
        {
            if (p_HeadLogicCommon.HeadLogicStatus == HoloMonActionHeadStatus.Runtime)
            {
                // プレイヤーの頭部位置を毎フレーム更新する
                p_HeadLookController.target = p_Data.TargetObject.transform.position;
            }
        }


        /// <summary>
        /// 視線モードの設定を有効化する
        /// </summary>
        private bool EnableSetting()
        {
            // 視線の追従を有効化する
            ChangeMaxBendingAngle(true);

            return true;
        }

        /// <summary>
        /// 視線モードの設定を無効化する
        /// </summary>
        private bool DisableSetting()
        {
            // 視線の追従を無効化する
            ChangeMaxBendingAngle(false);

            return true;
        }

        /// <summary>
        /// 設定アクションデータを反映する
        /// </summary>
        private bool ApplyData(HeadLogicSetting a_HeadLogicSetting)
        {
            // 設定アクションデータをキャストして取得する
            p_Data = a_HeadLogicSetting.HeadLogicLookAtTargetData;

            return true;
        }

        /// <summary>
        /// 有効無効に合わせてHeadLookControllerの最大角を変更する
        /// </summary>
        /// <param name="onoff"></param>
        private void ChangeMaxBendingAngle(bool onoff)
        {
            // 有効無効に合わせてHeadLookControllerの最大角を変更する
            float setValue = 0;
            if (onoff)
            {
                setValue = p_DefaultMaxBendingAngles;
            }
            p_HeadLookController.segment.maxBendingAngle = setValue;

            /*
            int segmentnum = p_HeadLookController.segments.Length;
            if (segmentnum > 0)
            {
                for (int loop = 0; loop < segmentnum; loop++)
                {
                    float setValue = 0;
                    if (onoff)
                    {
                        setValue = p_DefaultMaxBendingAngles;
                    }
                    p_HeadLookController.segments[loop].maxBendingAngle = setValue;
                }
            }
            */

            // 有効無効に合わせてOverrideAnimationの設定を変更する
            p_HeadLookController.overrideAnimation = onoff ? p_DefaultOverrideAnimation : false;
        }
    }
}