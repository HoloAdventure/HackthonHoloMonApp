using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using HoloMonApp.Content.Character.Model.BodyComponents.ToNavMeshAgent;

namespace HoloMonApp.Content.Character.Control.BodyComponents.ToNavMeshAgent
{
    /// <summary>
    /// ナビメッシュ操作API
    /// </summary>
    public class HoloMonControlBodyComponentsToNavMeshAgentAPI : MonoBehaviour
    {
        [SerializeField]
        private HoloMonBodyComponentsToNavMeshAgentAPI p_BodyComponentsToNavMeshAgentAPI;

        public void SetEnabled(bool a_onoff)
        {
            p_BodyComponentsToNavMeshAgentAPI.NavMeshAgent.enabled = a_onoff;
        }

        public void SetDestination(Vector3 a_Position)
        {
            p_BodyComponentsToNavMeshAgentAPI.NavMeshAgent.destination = a_Position;
        }
    }
}