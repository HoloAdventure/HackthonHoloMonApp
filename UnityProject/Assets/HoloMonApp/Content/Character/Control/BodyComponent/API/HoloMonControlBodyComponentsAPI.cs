using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

using HoloMonApp.Content.Character.Model.BodyComponents;

using HoloMonApp.Content.Character.Control.BodyComponents.ToBillBoard;
using HoloMonApp.Content.Character.Control.BodyComponents.ToCollider;
using HoloMonApp.Content.Character.Control.BodyComponents.ToNavMeshAgent;
using HoloMonApp.Content.Character.Control.BodyComponents.ToObjectManipulator;
using HoloMonApp.Content.Character.Control.BodyComponents.ToProblemEvent;
using HoloMonApp.Content.Character.Control.BodyComponents.ToRigidbody;
using HoloMonApp.Content.Character.Control.BodyComponents.ToTransformUtility;

namespace HoloMonApp.Content.Character.Control.BodyComponents
{
    /// <summary>
    /// ボディコンポーネントAPI
    /// </summary>
    public class HoloMonControlBodyComponentsAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonControlBodyComponentsToBillBoardAPI p_ToBillBoard;
        public HoloMonControlBodyComponentsToBillBoardAPI ToBillBoard => p_ToBillBoard;

        [SerializeField]
        private HoloMonControlBodyComponentsToColliderAPI p_ToCollider;
        public HoloMonControlBodyComponentsToColliderAPI ToCollider => p_ToCollider;

        [SerializeField]
        private HoloMonControlBodyComponentsToNavMeshAgentAPI p_ToNavMeshAgent;
        public HoloMonControlBodyComponentsToNavMeshAgentAPI ToNavMeshAgent => p_ToNavMeshAgent;

        [SerializeField]
        private HoloMonControlBodyComponentsToObjectManipulatorAPI p_ToObjectManipulator;
        public HoloMonControlBodyComponentsToObjectManipulatorAPI ToObjectManipulator => p_ToObjectManipulator;

        [SerializeField]
        private HoloMonControlBodyComponentsToProblemEventAPI p_ToProblemEvent;
        public HoloMonControlBodyComponentsToProblemEventAPI ToProblemEvent => p_ToProblemEvent;

        [SerializeField]
        private HoloMonControlBodyComponentsToRigidbodyAPI p_ToRigidbody;
        public HoloMonControlBodyComponentsToRigidbodyAPI ToRigidbody => p_ToRigidbody;

        [SerializeField]
        private HoloMonControlBodyComponentsToTransformUtilityAPI p_ToTransformUtility;
        public HoloMonControlBodyComponentsToTransformUtilityAPI ToTransformUtility => p_ToTransformUtility;
    }
}