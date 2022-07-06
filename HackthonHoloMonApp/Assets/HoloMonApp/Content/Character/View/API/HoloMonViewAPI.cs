using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.View.Animations;
using HoloMonApp.Content.Character.View.BodyComponents;
using HoloMonApp.Content.Character.View.Parameters;
using HoloMonApp.Content.Character.View.References;
using HoloMonApp.Content.Character.View.Sensations;

using HoloMonApp.Content.Character.View.Animations.Body;
using HoloMonApp.Content.Character.View.BodyComponents.ToBillBoard;
using HoloMonApp.Content.Character.View.BodyComponents.ToCollider;
using HoloMonApp.Content.Character.View.BodyComponents.ToNavMeshAgent;
using HoloMonApp.Content.Character.View.BodyComponents.ToObjectManipulator;
using HoloMonApp.Content.Character.View.BodyComponents.ToProblemEvent;
using HoloMonApp.Content.Character.View.BodyComponents.ToRigidbody;
using HoloMonApp.Content.Character.View.BodyComponents.ToTransformUtility;
using HoloMonApp.Content.Character.View.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.View.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.View.Parameters.Settings.Length;
using HoloMonApp.Content.Character.View.Parameters.Settings.Personal;
using HoloMonApp.Content.Character.View.References.Transforms.Around;
using HoloMonApp.Content.Character.View.References.Transforms.Body;
using HoloMonApp.Content.Character.View.References.Transforms.Bone;
using HoloMonApp.Content.Character.View.Sensations.FeelEyeGaze;
using HoloMonApp.Content.Character.View.Sensations.FeelHandGrab;
using HoloMonApp.Content.Character.View.Sensations.FieldOfVision;
using HoloMonApp.Content.Character.View.Sensations.ListenVoice;
using HoloMonApp.Content.Character.View.Sensations.TactileBody;

namespace HoloMonApp.Content.Character.View
{
    public class HoloMonViewAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonViewAnimationsAPI p_Animations;

        [SerializeField]
        private HoloMonViewBodyComponentsAPI p_BodyComponents;

        [SerializeField]
        private HoloMonViewParametersAPI p_Parameters;

        [SerializeField]
        private HoloMonViewReferencesAPI p_References;

        [SerializeField]
        private HoloMonViewSensationsAPI p_Sensations;


        // 参照用短縮変数
        public HoloMonViewAnimationsBodyAPI AnimationsBodyAPI => p_Animations.Body;

        public HoloMonViewBodyComponentsToBillBoardAPI BodyComponentsToBillBoardAPI => p_BodyComponents.ToBillBoard;

        public HoloMonViewBodyComponentsToColliderAPI BodyComponentsToColliderAPI => p_BodyComponents.ToCollider;

        public HoloMonViewBodyComponentsToNavMeshAgentAPI BodyComponentsToNavMeshAgentAPI => p_BodyComponents.ToNavMeshAgent;

        public HoloMonViewBodyComponentsToObjectManipulatorAPI BodyComponentsToObjectManipulatorAPI => p_BodyComponents.ToObjectManipulator;

        public HoloMonViewBodyComponentsToProblemEventAPI BodyComponentsToProblemEventAPI => p_BodyComponents.ToProblemEvent;

        public HoloMonViewBodyComponentsToRigidbodyAPI BodyComponentsToRigidbodyAPI => p_BodyComponents.ToRigidbody;

        public HoloMonViewBodyComponentsToTransformUtilityAPI BodyComponentsToTransformUtilityAPI => p_BodyComponents.ToTransformUtility;

        public HoloMonViewConditionsBodyAPI ConditionsBodyAPI => p_Parameters.Conditions.Body;

        public HoloMonViewConditionsLifeAPI ConditionsLifeAPI => p_Parameters.Conditions.Life;

        public HoloMonViewSettingsLengthAPI SettingsLengthAPI => p_Parameters.Settings.Length;

        public HoloMonViewSettingsPersonalAPI SettingsPersonalAPI => p_Parameters.Settings.Personal;

        public HoloMonViewTransformsAroundAPI TransformsAroundAPI => p_References.Transforms.Around;

        public HoloMonViewTransformsBodyAPI TransformsBodyAPI => p_References.Transforms.Body;

        public HoloMonViewTransformsBoneAPI TransformsBoneAPI => p_References.Transforms.Bone;

        public HoloMonViewFeelEyeGazeAPI SensationsFeelEyeGazeAPI => p_Sensations.FeelEyeGaze;

        public HoloMonViewFeelHandGrabAPI SensationsFeelHandGrab => p_Sensations.FeelHandGrab;

        public HoloMonViewFieldOfVisionAPI SensationsFieldOfVisionAPI => p_Sensations.FieldOfVision;

        public HoloMonViewListenVoiceAPI SensationsListenVoiceAPI => p_Sensations.ListenVoice;

        public HoloMonViewTactileBodyAPI SensationsTactileBodyAPI => p_Sensations.TactileBody;
    }
}