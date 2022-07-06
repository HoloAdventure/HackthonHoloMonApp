using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace HoloMonApp.Content.ItemSpace
{
	public class ParticleCollisionCheck : MonoBehaviour
	{
		[Serializable]
		private class VectorUnityEvent : UnityEvent<Vector3> { }

		private ParticleSystem p_RefParticle;
		private List<ParticleCollisionEvent> p_CollisionEventList;

		[SerializeField, Tooltip("ヒット位置の通知")]
		private VectorUnityEvent p_HitPositionNotice;

		private void Start()
		{
			p_RefParticle = this.GetComponent<ParticleSystem>();
			p_CollisionEventList = new List<ParticleCollisionEvent>();
		}

		private void OnParticleCollision(GameObject hitObject)
		{
			// パーティクルのヒット位置を取得するため、イベントリストを取得する
			p_RefParticle.GetCollisionEvents(hitObject, p_CollisionEventList);

			foreach (ParticleCollisionEvent collisionEvent in p_CollisionEventList)
			{
				GameObject collisionEventObject = collisionEvent.colliderComponent?.gameObject;
				// ヒット通知のオブジェクトと同一かどうか
				if (hitObject == collisionEventObject)
				{
					Vector3 pos = collisionEvent.intersection;
					Debug.Log("Particle Hit : object name = " + collisionEventObject.name + ", position = " + pos.ToString());

					// イベントが登録されていればヒット位置を通知する
					p_HitPositionNotice?.Invoke(pos);
				}
			}
		}
	}
}