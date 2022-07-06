using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

using HoloMonApp.Content.Character.Model.BodyComponents.ToNavMeshAgent;

namespace HoloMonApp.Content.Character.View.BodyComponents.ToNavMeshAgent
{
    /// <summary>
    /// ナビメッシュ操作API
    /// </summary>
    public class HoloMonViewBodyComponentsToNavMeshAgentAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToNavMeshAgentAPI p_BodyComponentsToNavMeshAgentAPI;

        public NavMeshAgent Agent => p_BodyComponentsToNavMeshAgentAPI.NavMeshAgent;

        public NavMeshPath Path => p_BodyComponentsToNavMeshAgentAPI.NavMeshAgent.path;

        public Vector3 CurrentPosition => p_BodyComponentsToNavMeshAgentAPI.NavMeshAgent.transform.position; 

        public Vector3 Velocity => p_BodyComponentsToNavMeshAgentAPI.NavMeshAgent.velocity;

        public float StopDistance => p_BodyComponentsToNavMeshAgentAPI.NavMeshAgent.stoppingDistance;
    }
}