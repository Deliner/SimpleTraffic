using RoadSimulator.Scripts.Game.Simulation.RoadSystem;
using RoadSimulator.Scripts.Game.Simulation.World;
using UnityEngine;
using UnityEngine.AI;

namespace RoadSimulator.Scripts.Game.Simulation.Agent
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class Vehicle : Agent
    {
        public Transform front;

        private const float BlockedDistance = .25f;

        private NavSection _currentNavSection;

        private NavConnection _currentOutConnection;

        private bool _isBlocked;

        public void Initialize(NavSection navSection, NavConnection destination, int vehicleMaxSpeed)
        {
            _currentOutConnection = destination;
            _currentNavSection = navSection;
            RegisterVehicle(true);

            agent.enabled = true;
            agent.destination = destination.transform.position;
            agent.speed = TrafficSystem.GetAgentSpeedFromKph(Mathf.Min(navSection.speedLimit, vehicleMaxSpeed));
        }

        private void RegisterVehicle(bool isAdd)
        {
            _currentNavSection.RegisterVehicle(this, isAdd);
        }

        public override void Update()
        {
            if (agent.isOnNavMesh)
            {
                _isBlocked = CheckBlocked();
            }

            base.Update();
        }

        protected override bool CheckStop()
        {
            return _isBlocked || isWaiting;
        }

        public override void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("RoadConnection"))
            {
                var connection = col.GetComponent<NavConnection>();
                if (connection.navSection != null)
                {
                    if (connection.navSection != _currentNavSection)
                        SwitchRoad(connection);
                }
            }

            base.OnTriggerEnter(col);
        }

        private bool CheckBlocked()
        {
            var forward = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(front.position, forward, out var hit))
            {
                if (Vector3.Distance(front.position, hit.point) < BlockedDistance)
                {
                    if (hit.transform.CompareTag("Gib") || hit.transform.CompareTag("Unit"))
                        return true;
                }

                return false;
            }

            return false;
        }

        private void SwitchRoad(NavConnection newConnection)
        {
            RegisterVehicle(false);
            currentSpeed = TrafficSystem.GetAgentSpeedFromKph(Mathf.Min(newConnection.navSection.speedLimit, maxSpeed));
            agent.speed = currentSpeed;

            _currentNavSection = newConnection.navSection;
            RegisterVehicle(true);

            _currentOutConnection = newConnection.GetOutConnection();
            if (_currentOutConnection != null)
                agent.destination = _currentOutConnection.transform.position;
        }

        public override void OnDrawGizmos()
        {
            if (TrafficSystem.instance.drawGizmos)
            {
                Gizmos.color = CheckStop() ? Color.gray : Color.white;
                if (agent.hasPath)
                {
                    Gizmos.DrawWireSphere(agent.destination, 0.1f);
                    for (var i = 0; i < agent.path.corners.Length - 1; i++)
                    {
                        var path = agent.path;
                        Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
                    }
                }

                Gizmos.color = _isBlocked ? Color.red : Color.green;
                var blockedRayEnd = front.TransformPoint(new Vector3(0, 0, BlockedDistance));
                Gizmos.DrawLine(front.position, blockedRayEnd);
            }
        }
    }
}