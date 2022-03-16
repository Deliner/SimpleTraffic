using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kawaiiju.Traffic;

namespace Kawaiiju
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class Vehicle : Agent 
	{
		[Header("Vehicle")]
		public Transform front;

		protected float BlockedDistance  = .25f;
		
		private NavSection m_CurrentNavSection;

		private NavConnection m_CurrentOutConnection;

		private bool m_Blocked;


		public virtual void Initialize(NavSection navSection, NavConnection destination)
		{
			m_CurrentNavSection = navSection;
            RegisterVehicle(m_CurrentNavSection, true);
			m_CurrentOutConnection = destination;
			agent.enabled = true;
			speed = TrafficSystem.instance.GetAgentSpeedFromKPH(Mathf.Min(navSection.speedLimit, maxSpeed));
			agent.speed = speed;
			agent.destination = destination.transform.position;
		}

        public virtual void RegisterVehicle(NavSection section, bool isAdd)
        {
            section.RegisterVehicle(this, isAdd);
        }

		public override void Update()
		{
			if(agent.isOnNavMesh)
			{
				m_Blocked = CheckBlocked();
			}
			base.Update();
		}

		protected override bool CheckStop()
		{
			if(m_Blocked || isWaiting)
				return true;
			return false;
		}

		public override void OnTriggerEnter(Collider col)
		{
			if(col.tag == "RoadConnection")
			{
				NavConnection connection = col.GetComponent<NavConnection>();
				if(connection.navSection != m_CurrentNavSection)
					SwitchRoad(connection);
			}
			base.OnTriggerEnter(col);
		}
		
		private bool CheckBlocked()
		{
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			RaycastHit hit;
			if (Physics.Raycast(front.position, forward, out hit))
			{
				if(Vector3.Distance(front.position, hit.point) < BlockedDistance)
				{
					if(hit.transform.tag == "Gib" || hit.transform.tag == "Unit")
						return true;
				}
				return false;
			}
			return false;
		}
		
		private void SwitchRoad(NavConnection newConnection)
		{
			RegisterVehicle(m_CurrentNavSection, false);
			speed = TrafficSystem.instance.GetAgentSpeedFromKPH(Mathf.Min(newConnection.navSection.speedLimit, maxSpeed));
			agent.speed =
				speed;
			m_CurrentNavSection = newConnection.navSection;
			RegisterVehicle(m_CurrentNavSection, true);
			m_CurrentOutConnection = newConnection.GetOutConnection();
			if(m_CurrentOutConnection != null)
				agent.destination = m_CurrentOutConnection.transform.position;
		}
		public override void OnDrawGizmos()
		{
			if(TrafficSystem.instance.drawGizmos)
			{
				Gizmos.color = CheckStop() ? Color.gray : Color.white;
				if(agent.hasPath)
				{	
					Gizmos.DrawWireSphere(agent.destination, 0.1f);
					for (int i = 0; i < agent.path.corners.Length - 1; i++)
					{
						var path = agent.path;
						Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
					}
				}

				Gizmos.color = m_Blocked ? Color.red : Color.green;
				Vector3 blockedRayEnd = front.TransformPoint(new Vector3(0, 0, BlockedDistance));
				Gizmos.DrawLine(front.position, blockedRayEnd);
			}
		}
	}
}

