using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
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