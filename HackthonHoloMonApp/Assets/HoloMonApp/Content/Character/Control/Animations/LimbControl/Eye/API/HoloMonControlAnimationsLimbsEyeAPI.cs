using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Model.Animations.Limbs.Eye;

namespace HoloMonApp.Content.Character.Control.Animations.Limbs.Eye
{
    public class HoloMonControlAnimationsLimbsEyeAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("コントローラの参照")]
        private HoloMonAnimationsLimbsEyeAPI p_HoloMonAnimationsLimbsEyeAPI;

        /// <summary>
        /// 瞳孔の収縮アクションを実行する
        /// </summary>
        /// <returns></returns>
        public void ActionContraction()
        {
            p_HoloMonAnimationsLimbsEyeAPI.ActionContraction();
        }

        /// <summary>
        /// 瞳孔の拡大アクションを実行する
        /// </summary>
        /// <returns></returns>
        public void ActionExpansion()
        {
            p_HoloMonAnimationsLimbsEyeAPI.ActionExpansion();
        }
    }
}