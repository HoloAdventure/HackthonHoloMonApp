using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    /// <summary>
    /// ホロモン頭部アクションロジック
    /// </summary>
    public class HoloMonHeadLogicController : MonoBehaviour
    {
        /// <summary>
        /// オーバーライド無しロジック
        /// </summary>
        [SerializeField, Tooltip("オーバーライド無しロジック")]
        private HoloMonHeadLogicNoOverride NoOverride;

        /// <summary>
        /// 対象を見る頭部アクションロジック
        /// </summary>
        [SerializeField, Tooltip("対象を見る頭部アクションロジック")]
        private HoloMonHeadLogicLookAtTarget TargetTracking;


        /// <summary>
        /// 現在の頭部アクションモード
        /// </summary>
        [SerializeField, Tooltip("現在の頭部アクションモード")]
        private HoloMonActionHead p_HoloMonHead;


        /// <summary>
        /// 現在の視線モードの参照変数
        /// </summary>
        public HoloMonActionHead HoloMonHead => p_HoloMonHead;

        /// <summary>
        /// 指定視線状態のステータス
        /// </summary>
        /// <param name="a_HoloMonHead"></param>
        /// <returns></returns>
        public HoloMonActionHeadStatus SpecificHoloMonHeadLogicStatus(HoloMonActionHead a_HoloMonHead)
        {
            return SpecificHoloMonHeadLogicIntarfaces(a_HoloMonHead).GetHoloMonHeadStatus();
        }

        /// <summary>
        /// 指定視線状態の EveryValueChanged オブザーバ参照変数
        /// </summary>
        /// <param name="a_HoloMonHead"></param>
        /// <returns></returns>
        public IObservable<HoloMonActionHeadStatus> SpecificHoloMonHeadLogicStatusEveryValueChanged(HoloMonActionHead a_HoloMonHead)
        {
            return SpecificHoloMonHeadLogicIntarfaces(a_HoloMonHead).GetHoloMonHeadStatusEveryValueChanged();
        }


        /// <summary>
        /// 視線ロジックの切り替え
        /// </summary>
        public void ChangeHeadLogic(HeadLogicSetting a_Setting)
        {
            // 指定の頭部アクション種別を取得する
            HoloMonActionHead actionHead = a_Setting.HoloMonActionHead;

            // 一旦全てのロジックを無効化する
            CancelHeadLogic();

            // 指定のロジックの設定を反映する
            SpecificHoloMonHeadLogicIntarfaces(actionHead).ApplySetting(a_Setting);

            // 指定のロジックを有効化する
            SpecificHoloMonHeadLogicIntarfaces(actionHead).EnableHead();

            // 有効化したロジックを現在のモードとする
            p_HoloMonHead = actionHead;
        }

        /// <summary>
        /// 視線ロジックの無効化
        /// </summary>
        public void CancelHeadLogic()
        {
            // 全てのロジックを無効化する
            foreach (HoloMonHeadLogicInterface logicBase in ListHoloMonHeadLogicIntarfaces())
            {
                logicBase.DisableHead(HoloMonActionHeadStatus.Stopping);
            }
        }

        /// <summary>
        /// 指定モードのインタフェースを返す
        /// </summary>
        /// <param name="a_HoloMonHead"></param>
        /// <returns></returns>
        private HoloMonHeadLogicInterface SpecificHoloMonHeadLogicIntarfaces(HoloMonActionHead a_HoloMonHead)
        {
            // デフォルトは追従なしモードとする
            HoloMonHeadLogicInterface specificInterface = NoOverride;
            switch (a_HoloMonHead)
            {
                case HoloMonActionHead.NoOverride:
                    specificInterface = NoOverride;
                    break;
                case HoloMonActionHead.LookAtTarget:
                    specificInterface = TargetTracking;
                    break;
                default:
                    break;
            }
            return specificInterface;
        }

        /// <summary>
        /// インタフェースの参照リスト
        /// </summary>
        /// <returns></returns>
        private List<HoloMonHeadLogicInterface> ListHoloMonHeadLogicIntarfaces()
        {
            return new List<HoloMonHeadLogicInterface>()
            {
                NoOverride,
                TargetTracking,
            };
        }
    }
}
