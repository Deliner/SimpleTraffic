using RoadSimulator.Scripts.Game.Base;
using UnityEngine;

namespace RoadSimulator.Scripts.UI.Utils
{
    public class RoadResourcesHolder : MonoBehaviour
    {
        [SerializeField] private GameObject crossroadT;
        [SerializeField] private GameObject crossroad;
        [SerializeField] private GameObject generator;
        [SerializeField] private GameObject halfRoad;
        [SerializeField] private GameObject dualRoad;
        [SerializeField] private GameObject deletor;
        [SerializeField] private GameObject corner;
        [SerializeField] private GameObject road;
        [SerializeField] private GameObject plug;

        public RoadObjectFactory.Resources GetResources()
        {
            return new RoadObjectFactory.Resources(road, corner, crossroad, halfRoad, crossroadT, dualRoad, generator, deletor, plug);
        }
    }
}