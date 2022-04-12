using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
	[RequireComponent(typeof(Collider))]
	public class NavConnection : MonoBehaviour
	{
		public NavSection navSection;

		public NavConnection[] outConnections;

		public NavConnection GetOutConnection()
		{
			if(outConnections.Length > 0)
			{
				int index = Random.Range(0, outConnections.Length);
				return outConnections[index];
			}
			return null;
		}

		private void OnDrawGizmos()
		{
			if(TrafficSystem.instance.drawGizmos)
			{
				Gizmos.color = Color.white;
				var objectTransform = transform;
				Gizmos.DrawSphere(objectTransform.position - new Vector3(0, objectTransform.localScale.y * 0.5f, 0), 0.05f);
			}
		}
	}
}

