using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic
{
    public class HoloMonHeadModeLogicAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("頭部状態ロジックコントローラの参照")]
        private HoloMonHeadLogicController p_HoloMonHeadLogicController;

        /// <summary>
        /// 個別アクション無しにする
        /// </summary>
        /// <returns></returns>
        public bool ActionNoOverride()
        {
            // 頭部追従ロジックを無効化する
            p_HoloMonHeadLogicController.ChangeHeadLogic(
                new HeadLogicSetting(new HeadLogicNoOverrideData())
                );
            return true;
        }

        /// <summary>
        /// 指定のオブジェクトを見るアクションを実行する
        /// （指定オブジェクトがnullの場合は個別アクション無しにする）
        /// </summary>
        /// <returns></returns>
        public bool ActionLookAtTarget(GameObject a_LookObject)
        {
            if (a_LookObject == null)
            {
                // 指定オブジェクトが null の場合は頭部追従ロジックを無効化して終了する
                p_HoloMonHeadLogicController.ChangeHeadLogic(
                    new HeadLogicSetting(new HeadLogicNoOverrideData())
                    );
                return false;
            }

            // 指定のオブジェクトを見る
            p_HoloMonHeadLogicController.ChangeHeadLogic(
                    new HeadLogicSetting(new HeadLogicLookAtTargetData(a_LookObject))
                );
            return true;
        }
    }
}