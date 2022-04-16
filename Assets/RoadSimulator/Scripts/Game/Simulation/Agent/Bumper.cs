using System;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.Agent
{
    public class Bumper : MonoBehaviour
    {
        private int _enterCounter;


        public bool IsBlocked() => _enterCounter != 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Gib"))
            {
                _enterCounter++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Gib"))
            {
                _enterCounter--;
            }
        }
    }
}