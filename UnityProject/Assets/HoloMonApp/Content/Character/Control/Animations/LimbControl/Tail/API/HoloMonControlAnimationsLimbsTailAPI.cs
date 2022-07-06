using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.Character.Model.Animations.Limbs.Tail;

namespace HoloMonApp.Content.Character.Control.Animations.Limbs.Tail
{
    public class HoloMonControlAnimationsLimbsTailAPI : MonoBehaviour
    {
        [SerializeField, Tooltip("コントローラの参照")]
        private HoloMonAnimationsLimbsTailAPI p_HoloMonAnimationsLimbsTailAPI;
    }
}