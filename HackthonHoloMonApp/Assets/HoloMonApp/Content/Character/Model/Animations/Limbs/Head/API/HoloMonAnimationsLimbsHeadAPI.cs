using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Model.Animations.Limbs.Head.ModeLogic;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Head
{
    public class HoloMonAnimationsLimbsHeadAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("頭部コントローラの参照")]
        private HoloMonHeadModeLogicAPI p_HoloMonHeadController;

        /// <summary>
        /// 頭部アクション無しにする
        /// </summary>
        /// <returns></returns>
        public bool ActionNoOverride()
        {
            bool result = p_HoloMonHeadController.ActionNoOverride();
            return result;
        }

        /// <summary>
        /// 指定のオブジェクトを見るアクションを実行する
        /// （指定オブジェクトがnullの場合は頭部アクション無しにする）
        /// </summary>
        /// <returns></returns>
        public bool ActionLookAtTarget(GameObject a_LookObject)
        {
            bool result = p_HoloMonHeadController.ActionLookAtTarget(a_LookObject);
            return result;
        }
    }
}