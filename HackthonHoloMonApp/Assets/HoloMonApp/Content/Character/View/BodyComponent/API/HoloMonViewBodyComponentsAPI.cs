using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

using HoloMonApp.Content.Character.Model.BodyComponents;

using HoloMonApp.Content.Character.View.BodyComponents.ToBillBoard;
using HoloMonApp.Content.Character.View.BodyComponents.ToCollider;
using HoloMonApp.Content.Character.View.BodyComponents.ToNavMeshAgent;
using HoloMonApp.Content.Character.View.BodyComponents.ToObjectManipulator;
using HoloMonApp.Content.Character.View.BodyComponents.ToProblemEvent;
using HoloMonApp.Content.Character.View.BodyComponents.ToRigidbody;
using HoloMonApp.Content.Character.View.BodyComponents.ToTransformUtility;

namespace HoloMonApp.Content.Character.View.BodyComponents
{
    /// <summary>
    /// ボディコンポーネントAPI
    /// </summary>
    public class HoloMonViewBodyComponentsAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonViewBodyComponentsToBillBoardAPI p_ToBillBoard;
        public HoloMonViewBodyComponentsToBillBoardAPI ToBillBoard => p_ToBillBoard;

        [SerializeField]
        private HoloMonViewBodyComponentsToColliderAPI p_ToCollider;
        public HoloMonViewBodyComponentsToColliderAPI ToCollider => p_ToCollider;

        [SerializeField]
        private HoloMonViewBodyComponentsToNavMeshAgentAPI p_ToNavMeshAgent;
        public HoloMonViewBodyComponentsToNavMeshAgentAPI ToNavMeshAgent => p_ToNavMeshAgent;

        [SerializeField]
        private HoloMonViewBodyComponentsToObjectManipulatorAPI p_ToObjectManipulator;
        public HoloMonViewBodyComponentsToObjectManipulatorAPI ToObjectManipulator => p_ToObjectManipulator;

        [SerializeField]
        private HoloMonViewBodyComponentsToProblemEventAPI p_ToProblemEvent;
        public HoloMonViewBodyComponentsToProblemEventAPI ToProblemEvent => p_ToProblemEvent;

        [SerializeField]
        private HoloMonViewBodyComponentsToRigidbodyAPI p_ToRigidbody;
        public HoloMonViewBodyComponentsToRigidbodyAPI ToRigidbody => p_ToRigidbody;

        [SerializeField]
        private HoloMonViewBodyComponentsToTransformUtilityAPI p_ToTransformUtility;
        public HoloMonViewBodyComponentsToTransformUtilityAPI ToTransformUtility => p_ToTransformUtility;
    }
}