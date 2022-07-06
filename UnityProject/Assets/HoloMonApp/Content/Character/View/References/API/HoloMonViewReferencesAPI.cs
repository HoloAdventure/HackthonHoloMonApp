using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.View.Animations;
using HoloMonApp.Content.Character.View.BodyComponents;
using HoloMonApp.Content.Character.View.References.Transforms;

namespace HoloMonApp.Content.Character.View.References
{
    public class HoloMonViewReferencesAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonViewTransformsAPI p_Transforms;
        public HoloMonViewTransformsAPI Transforms => p_Transforms;
    }
}