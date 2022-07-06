using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Model.Animations.Limbs.Head;

namespace HoloMonApp.Content.Character.Control.Animations.Limbs.Head
{
    public class HoloMonControlAnimationsLimbsHeadAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("コントローラの参照")]
        private HoloMonAnimationsLimbsHeadAPI p_HoloMonAnimationsLimbsHeadAPI;

        /// <summary>
        /// 頭部アクション無しにする
        /// </summary>
        /// <returns></returns>
        public void ActionNoOverride()
        {
            p_HoloMonAnimationsLimbsHeadAPI.ActionNoOverride();
        }

        /// <summary>
        /// 指定のオブジェクトを見るアクションを実行する
        /// （指定オブジェクトがnullの場合は頭部アクション無しにする）
        /// </summary>
        /// <returns></returns>
        public void ActionLookAtTarget(GameObject a_LookObject)
        {
            p_HoloMonAnimationsLimbsHeadAPI.ActionLookAtTarget(a_LookObject);
        }
    }
}