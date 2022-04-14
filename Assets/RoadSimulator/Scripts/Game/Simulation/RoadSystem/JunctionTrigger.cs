using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class JunctionTrigger : MonoBehaviour
    {
        public enum TriggerType
        {
            Enter,
            Exit
        }

        public TriggerType type;

        public Junction junction;

        public void TriggerJunction()
        {
            junction.TryChangePhase();
        }
    }
}