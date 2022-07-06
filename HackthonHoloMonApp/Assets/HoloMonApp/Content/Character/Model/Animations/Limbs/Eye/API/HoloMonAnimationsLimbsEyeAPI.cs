using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloMonApp.Content.Character.Model.Animations.Limbs.Eye
{
    public class HoloMonAnimationsLimbsEyeAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("瞳孔コントローラの参照")]
        private HoloMonPupilController p_HoloMonPupilController;

        /// <summary>
        /// 瞳孔の収縮アクションを実行する
        /// </summary>
        /// <returns></returns>
        public bool ActionContraction()
        {
            bool result = p_HoloMonPupilController.ActionContraction();
            return result;
        }

        /// <summary>
        /// 瞳孔の拡大アクションを実行する
        /// </summary>
        /// <returns></returns>
        public bool ActionExpansion()
        {
            bool result = p_HoloMonPupilController.ActionExpansion();
            return result;
        }
    }
}