using RoadSimulator.Scripts.Game.Simulation.RoadSystem;
using UnityEngine;
using UnityEngine.AI;

namespace RoadSimulator.Scripts.Game.Simulation.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Agent : MonoBehaviour
    {
        public int maxSpeed = 120;

        public float speed;
        protected bool isWaiting { get; private set; }

        private Transform _destination;

        private WaitZone _currentWaitZone;

        private NavMeshAgent _agent;

        protected NavMeshAgent agent
        {
            get
            {
                if (!_agent)
                    _agent = GetComponent<NavMeshAgent>();
                return _agent;
            }
        }

        public virtual void Update()
        {
            if (agent.isOnNavMesh)
            {
                if (CheckStop())
                    agent.velocity = Vector3.zero;
                
                CheckWaitZone();
            }
        }

        public virtual void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("WaitZone"))
            {
                var waitZone = col.GetComponent<WaitZone>();

                _currentWaitZone = waitZone;
                if (!waitZone.canPass)
                    isWaiting = true;
            }
        }

        private void CheckWaitZone()
        {
            if (isWaiting)
            {
                if (_currentWaitZone)
                    isWaiting = !_currentWaitZone.canPass;
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