using JetBrains.Annotations;
using RoadSimulator.Scripts.Game.Simulation.World;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    [RequireComponent(typeof(Collider))]
    public class NavConnection : MonoBehaviour
    {
        public NavConnection[] outConnections;
        
        public NavSection navSection;

        [CanBeNull]
        public NavConnection GetOutConnection()
        {
            if (outConnections.Length > 0)
            {
                var index = Random.Range(0, outConnections.Length);
                return outConnections[index];
            }

            return null;
        }

        private void OnDrawGizmos()
        {
            if (TrafficSystem.instance.drawGizmos)
            {
                Gizmos.color = Color.white;
                var objectTransform = transform;
                Gizmos.DrawSphere(objectTransform.position - new Vector3(0, objectTransform.localScale.y * 0.5f, 0), 0.05f);
            }
        }
    }
}