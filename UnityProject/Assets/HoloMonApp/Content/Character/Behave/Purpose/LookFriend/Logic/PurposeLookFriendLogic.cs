using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UniRx;

using HoloMonApp.Content.Character.Behave.ModeLogic;
using HoloMonApp.Content.Character.Behave.ModeLogic.LookAround;
using HoloMonApp.Content.Character.Data.Knowledge.Objects;

namespace HoloMonApp.Content.Character.Behave.Purpose.LookFriend
{
    /// <summary>
    /// 友人に注目する行動ロジック
    /// </summary>
    public class PurposeLookFriendLogic : MonoBehaviour, HoloMonPurposeBehaveIF
    {
        /// <summary>
        /// 目的行動共通参照
        /// </summary>
        private PurposeReference p_PurposeReference;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="behaveReference"></param>
        /// <param name="actionModeLogicAPI"></param>
        public void AwakeInit(
            HoloMonBehaveReference behaveReference,
            HoloMonActionModeLogicAPI actionModeLogicAPI)
        {
            p_PurposeReference = new PurposeReference(behaveReference, actionModeLogicAPI);
        }

        /// <summary>
        /// 反映中の設定データ
        /// </summary>
        [SerializeField]
        private PurposeInformation p_Info;
        private PurposeLookFriendData p_Data => (PurposeLookFriendData)p_Info.PurposeData;

        private void ApplyData(PurposeInformation a_Porpuse)
        {
            // 設定データの取得
            p_Info = a_Porpuse;
        }

        /// <summary>
        /// ホロモン目的行動種別
        /// </summary>
        /// <returns></returns>
        public HoloMonPurposeType GetHoloMonPurposeType()
        {
            return HoloMonPurposeType.LookFriend;
        }

        /// <summary>
        /// 友人に注目する行動の開始(async/await制御)
        /// </summary>
        public async UniTask<bool> StartLogicAsync(PurposeInformation a_Porpuse)
        {
            // 行動設定を反映する
            ApplyData(a_Porpuse);

            // アクション変数
            ModeLogicResult modeLogicResult;

            // 友人に注目する
            ObjectUnderstandType targetType = ObjectUnderstandType.FriendFace;

            // 指定種別のオブジェクトが見えているか否か
            GameObject targetVisionObject = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CheckCollectionByNearDistance(a_ObjectUnderstandType: targetType)?.Object;

            if (targetVisionObject == null)
            {
                // ターゲットが見えていない場合

                // 見回しモードのアクションを実行待機する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionLookAroundAcync(
                    a_SearchObjectUnderstandType: targetType
                    );

                // 結果に応じて次のイベントを発生させる
                if (modeLogicResult.FinishModeStatus != HoloMonActionModeStatus.Achievement)
                {
                    // ロジックを達成できなかった場合

                    // 処理を終了する
                    return true;
                }

                // 発見オブジェクトを取得する
                ModeLogicLookAroundReturn lookAroundReturn = (ModeLogicLookAroundReturn)modeLogicResult.ModeLogicReturn;
                targetVisionObject = lookAroundReturn.FindedObject;
            }

            // ターゲットまでの距離を取得する
            float targetDistance = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CurrentDistanceForGameObject(a_Object: targetVisionObject, a_IgnoreYAxis: true);

            // ターゲットとの相対角度を取得する
            float targetAngle = p_PurposeReference
                .View.SensationsFieldOfVisionAPI
                .CurrentAngleForGameObject(a_Object: targetVisionObject, a_IgnoreYAxis: true);

            // 5m以内の距離(ホロモン1m時)かチェックする
            float checkDistance = 5.0f * p_PurposeReference.View.ConditionsBodyAPI.BodyHeight;

            if (targetDistance < checkDistance)
            {
                // 5m以内の距離(ホロモン1m時)なら振り向き判定を行う

                if (targetAngle > 90.0f)
                {
                    // 90度以上角度があれば体ごと振り向く

                    // 振り向きのアクションを実行待機する
                    modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTurnTargetAcync(
                        a_TargetObject: targetVisionObject
                        );

                    // 成否判定無し
                }
            }
            else
            {
                // 5m以上の距離があれば近づいてくる

                // ターゲット追跡モードのアクションを実行待機する
                // 友達の 2m 手前まで移動する
                modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionTrackingTargetAcync(
                    a_TargetObject: targetVisionObject,
                    a_StoppingDistance: 2.0f
                    );

                // 成否判定無し
            }

            // スタンバイモードのアクションを実行待機する
            modeLogicResult = await p_PurposeReference.ActionModeLogic.ActionStandbyAcync(
                a_StartLookObject: targetVisionObject
                );

            return true;
        }
    }
}