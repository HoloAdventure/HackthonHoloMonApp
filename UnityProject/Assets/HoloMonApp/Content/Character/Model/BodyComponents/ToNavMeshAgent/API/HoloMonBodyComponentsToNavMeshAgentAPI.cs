using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace HoloMonApp.Content.Character.Model.BodyComponents.ToNavMeshAgent
{
    /// <summary>
    /// ナビメッシュ操作API
    /// </summary>
    public class HoloMonBodyComponentsToNavMeshAgentAPI : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent p_Agent;
        public NavMeshAgent NavMeshAgent => p_Agent;
    }
}