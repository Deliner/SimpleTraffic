using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Kawaiiju.Traffic;

namespace Kawaiiju
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Agent : MonoBehaviour
    {
        [Header("Agent")] public TrafficType type = TrafficType.Pedestrian;
        public int maxSpeed = 120;

        public float speed;
        protected bool isWaiting { get; private set; }

        private Transform m_Destination;

        private WaitZone m_CurrentWaitZone;

        private NavMeshAgent m_Agent;

        protected NavMeshAgent agent
        {
            get
            {
                if (!m_Agent)
                    m_Agent = GetComponent<NavMeshAgent>();
                return m_Agent;
            }
        }

        public void Initialize()
        {
            agent.enabled = true;
            speed = TrafficSystem.instance.GetAgentSpeedFromKPH(maxSpeed);
            agent.speed = speed;
            m_Destination = TrafficSystem.instance.GetPedestrianDestination();
            if (m_Destination)
                agent.destination = m_Destination.position;
        }

        public virtual void Update()
        {
            if (agent.isOnNavMesh)
            {
                if (CheckStop())
                    agent.velocity = Vector3.zero;
                CheckWaitZone();
                if (type == TrafficType.Pedestrian)
                    TestDestination();
            }
        }

        private void TestDestination()
        {
            if (m_Destination)
            {
                var distanceToDestination = Vector3.Distance(transform.position, m_Destination.position);
                if (distanceToDestination < 1f)
                {
                    m_Destination = TrafficSystem.instance.GetPedestrianDestination();
                    agent.destination = m_Destination.position;
                }
            }
        }

        public virtual void OnTriggerEnter(Collider col)
        {
            if (col.tag == "WaitZone")
            {
                var waitZone = col.GetComponent<WaitZone>();
                if (waitZone.type == type)
                {
                    if (type == TrafficType.Pedestrian)
                    {
                        if (CheckOppositeWAitZone(waitZone))
                            return;
                    }

                    m_CurrentWaitZone = waitZone;
                    if (!waitZone.canPass)
                        isWaiting = true;
                }
            }
        }

        private bool CheckOppositeWAitZone(WaitZone waitZone)
        {
            if (waitZone.opposite)
            {
                if (waitZone.opposite == m_CurrentWaitZone)
                    return true;
            }

            return false;
        }

        private void CheckWaitZone()
        {
            if (isWaiting)
            {
                if (m_CurrentWaitZone)
                    isWaiting = !m_CurrentWaitZone.canPass;
            }
        }

        protected virtual bool CheckStop()
        {
            return isWaiting;
        }

        public virtual void OnDrawGizmos()
        {
        }
    }
}