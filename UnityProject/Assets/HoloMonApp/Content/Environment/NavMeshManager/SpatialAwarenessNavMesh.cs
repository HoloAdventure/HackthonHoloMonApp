using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HoloMonApp.Content.XRPlatform;

namespace HoloMonApp.Content.Environment.NavMeshManager
{
    public class SpatialAwarenessNavMesh : MonoBehaviour
    {
        /// <summary>
        /// 有効時処理
        /// </summary>
        private void OnEnable()
        {
            HoloMonXRPlatform.Instance.EnvironmentMap.Handler.OnAdded += OnObservationAdded;
            HoloMonXRPlatform.Instance.EnvironmentMap.Handler.OnUpdated += OnObservationUpdated;
            HoloMonXRPlatform.Instance.EnvironmentMap.Handler.OnRemoved += OnObservationRemoved;
        }

        /// <summary>
        /// 無効時処理
        /// </summary>
        private void OnDisable()
        {
            HoloMonXRPlatform.Instance.EnvironmentMap.Handler.OnAdded -= OnObservationAdded;
            HoloMonXRPlatform.Instance.EnvironmentMap.Handler.OnUpdated -= OnObservationUpdated;
            HoloMonXRPlatform.Instance.EnvironmentMap.Handler.OnRemoved -= OnObservationRemoved;
        }

        /// <inheritdoc />
        public void OnObservationAdded(EnvironmentMapEventData eventData)
        {
            // Mesh追加時のイベント
            Debug.Log($"Tracking mesh {eventData.Id}");

            // NavMeshSourceTag をオブジェクトに追加
            eventData.MapObject.AddComponent<NavMeshSourceTag>();
        }

        /// <inheritdoc />
        public void OnObservationUpdated(EnvironmentMapEventData eventData)
        {
            // Mesh更新時のイベント
            Debug.Log($"Update mesh {eventData.Id}");

            // NavMeshSourceTag をオブジェクトに追加
            NavMeshSourceTag tag = eventData.MapObject.GetComponent<NavMeshSourceTag>();
            if (tag == null)
            {
                eventData.MapObject.AddComponent<NavMeshSourceTag>();
            }
        }

        /// <inheritdoc />
        public void OnObservationRemoved(EnvironmentMapEventData eventData)
        {
            // Mesh削除時のイベント
            Debug.Log($"Remove mesh {eventData.Id}");
        }
    }
}

/*
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;

namespace SpatialAwarenessNavMesh
{
    using SpatialAwarenessHandler = IMixedRealitySpatialAwarenessObservationHandler<SpatialAwarenessMeshObject>;

    public class SpatialAwarenessNavMesh : MonoBehaviour, SpatialAwarenessHandler
    {
        /// <summary>
        /// Value indicating whether or not this script has registered for spatial awareness events.
        /// このスクリプトが SpatialAwareness イベントに登録されているかどうかを示す値。
        /// </summary>
        private bool isRegistered = false;

        /// <summary>
        /// 有効時処理
        /// </summary>
        private void OnEnable()
        {
            RegisterEventHandlers();
        }

        /// <summary>
        /// 無効時処理
        /// </summary>
        private void OnDisable()
        {
            UnregisterEventHandlers();
        }

        /// <summary>
        /// 削除時処理
        /// </summary>
        private void OnDestroy()
        {
            UnregisterEventHandlers();
        }

        /// <summary>
        /// Registers for the spatial awareness system events.
        /// SpatialAwarenessのイベント登録を行う
        /// </summary>
        private void RegisterEventHandlers()
        {
            if (!isRegistered && (CoreServices.SpatialAwarenessSystem != null))
            {
                CoreServices.SpatialAwarenessSystem.RegisterHandler<SpatialAwarenessHandler>(this);
                isRegistered = true;
            }
        }

        /// <summary>
        /// Unregisters from the spatial awareness system events.
        /// SpatialAwarenessのイベントを削除する
        /// </summary>
        private void UnregisterEventHandlers()
        {
            if (isRegistered && (CoreServices.SpatialAwarenessSystem != null))
            {
                CoreServices.SpatialAwarenessSystem.UnregisterHandler<SpatialAwarenessHandler>(this);
                isRegistered = false;
            }
        }

        /// <inheritdoc />
        public virtual void OnObservationAdded(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            // Mesh追加時のイベント
            Debug.Log($"Tracking mesh {eventData.Id}");

            // NavMeshSourceTag をオブジェクトに追加
            eventData.SpatialObject.GameObject.AddComponent<NavMeshSourceTag>();
        }

        /// <inheritdoc />
        public virtual void OnObservationUpdated(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            // Mesh更新時のイベント
            Debug.Log($"Update mesh {eventData.Id}");

            // NavMeshSourceTag をオブジェクトに追加
            NavMeshSourceTag tag = eventData.SpatialObject.GameObject.GetComponent<NavMeshSourceTag>();
            if (tag == null)
            {
                eventData.SpatialObject.GameObject.AddComponent<NavMeshSourceTag>();
            }
        }

        /// <inheritdoc />
        public virtual void OnObservationRemoved(MixedRealitySpatialAwarenessEventData<SpatialAwarenessMeshObject> eventData)
        {
            // Mesh削除時のイベント
            Debug.Log($"Remove mesh {eventData.Id}");
        }
    }
}
*/