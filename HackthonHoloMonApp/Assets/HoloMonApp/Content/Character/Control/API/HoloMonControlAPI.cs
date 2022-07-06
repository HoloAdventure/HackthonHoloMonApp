using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Control.Animations;
using HoloMonApp.Content.Character.Control.BodyComponents;
using HoloMonApp.Content.Character.Control.ObjectInteraction;
using HoloMonApp.Content.Character.Control.Parameters;
using HoloMonApp.Content.Character.Control.References;
using HoloMonApp.Content.Character.Control.WorldItems;
using HoloMonApp.Content.Character.Control.VisualizeUIs;

using HoloMonApp.Content.Character.Control.ObjectInteraction.HoldItem;
using HoloMonApp.Content.Character.Control.ObjectInteraction.RideItem;
using HoloMonApp.Content.Character.Control.Parameters.Conditions.Body;
using HoloMonApp.Content.Character.Control.Parameters.Conditions.Life;
using HoloMonApp.Content.Character.Control.Parameters.Settings.Personal;
using HoloMonApp.Content.Character.Control.Animations.Body;
using HoloMonApp.Content.Character.Control.Animations.Limbs.Eye;
using HoloMonApp.Content.Character.Control.Animations.Limbs.Head;
using HoloMonApp.Content.Character.Control.Animations.Limbs.Tail;
using HoloMonApp.Content.Character.Control.BodyComponents.ToBillBoard;
using HoloMonApp.Content.Character.Control.BodyComponents.ToCollider;
using HoloMonApp.Content.Character.Control.BodyComponents.ToNavMeshAgent;
using HoloMonApp.Content.Character.Control.BodyComponents.ToObjectManipulator;
using HoloMonApp.Content.Character.Control.BodyComponents.ToProblemEvent;
using HoloMonApp.Content.Character.Control.BodyComponents.ToRigidbody;
using HoloMonApp.Content.Character.Control.BodyComponents.ToTransformUtility;
using HoloMonApp.Content.Character.Control.WorldItems.Stand;
using HoloMonApp.Content.Character.Control.VisualizeUIs.EmotionPanel;
using HoloMonApp.Content.Character.Control.VisualizeUIs.WordBubble;

namespace HoloMonApp.Content.Character.Control
{
    public class HoloMonControlAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonControlAnimationsAPI p_Animations;

        [SerializeField]
        private HoloMonControlBodyComponentsAPI p_BodyComponents;

        [SerializeField]
        private HoloMonControlObjectInteractionAPI p_ObjectInteractions;

        [SerializeField]
        private HoloMonControlParametersAPI p_Parameters;

        [SerializeField]
        private HoloMonControlReferencesAPI p_References;

        [SerializeField]
        private HoloMonControlSupportItemAPI p_SupportItem;

        [SerializeField]
        private HoloMonControlVisualizeUIsAPI p_VisualizeUI;


        // 参照用短縮変数
        public HoloMonControlHoldItemAPI ObjectInteractionsHoldItemAPI => p_ObjectInteractions.HoldItem;

        public HoloMonControlRideItemAPI ObjectInteractionsRideItemAPI => p_ObjectInteractions.RideItem;

        public HoloMonControlConditionsBodyAPI ConditionsBodyAPI => p_Parameters.Conditions.Body;

        public HoloMonControlConditionsLifeAPI ConditionsLifeAPI => p_Parameters.Conditions.Life;

        public HoloMonControlSettingsPersonalAPI SettingsPersonalAPI => p_Parameters.Settings.Personal;

        public HoloMonControlAnimationsBodyAPI AnimationsBodyAPI => p_Animations.Body;

        public HoloMonControlAnimationsLimbsHeadAPI LimbAnimationsHeadAPI => p_Animations.Limbs.Head;

        public HoloMonControlAnimationsLimbsEyeAPI LimbAnimationsEyeAPI => p_Animations.Limbs.Eye;

        public HoloMonControlAnimationsLimbsTailAPI LimbComponentsTailAPI => p_Animations.Limbs.Tail;

        public HoloMonControlBodyComponentsToBillBoardAPI BodyComponentsToBillBoardAPI => p_BodyComponents.ToBillBoard;

        public HoloMonControlBodyComponentsToColliderAPI BodyComponentsToColliderAPI => p_BodyComponents.ToCollider;

        public HoloMonControlBodyComponentsToNavMeshAgentAPI BodyComponentsToNavMeshAgentAPI => p_BodyComponents.ToNavMeshAgent;

        public HoloMonControlBodyComponentsToObjectManipulatorAPI BodyComponentsToObjectManipulatorAPI => p_BodyComponents.ToObjectManipulator;

        public HoloMonControlBodyComponentsToProblemEventAPI BodyComponentsToProblemEventAPI => p_BodyComponents.ToProblemEvent;

        public HoloMonControlBodyComponentsToRigidbodyAPI BodyComponentsToRigidbodyAPI => p_BodyComponents.ToRigidbody;

        public HoloMonControlBodyComponentsToTransformUtilityAPI BodyComponentsToTransformUtilityAPI => p_BodyComponents.ToTransformUtility;

        public HoloMonControlItemStandAPI SupportItemsStandAPI => p_SupportItem.Stand;

        public HoloMonControlVisualizeUIsEmotionPanelAPI VisualizeUIsEmotionPanelAPI => p_VisualizeUI.EmotionPanel;

        public HoloMonControlVisualizeUIsWordBubbleAPI VisualizeUIsWordBubbleAPI => p_VisualizeUI.WordBubble;
    }
}