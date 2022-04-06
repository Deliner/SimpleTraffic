using System;
using UnityEngine;

namespace Common
{
    public class RoadResourcesHolder : MonoBehaviour
    {
        [SerializeField] private GameObject crossroadT;
        [SerializeField] private GameObject crossroad;
        [SerializeField] private GameObject halfRoad;
        [SerializeField] private GameObject dualRoad;
        [SerializeField] private GameObject corner;
        [SerializeField] private GameObject road;

        private static RoadObjectFactory.Resources? _resources;

        public RoadObjectFactory.Resources GetResources()
        {
            _resources ??= new RoadObjectFactory.Resources(road, corner, crossroad, halfRoad, crossroadT, dualRoad);
            return _resources.Value;
        }
    }
}