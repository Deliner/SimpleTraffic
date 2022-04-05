using System;
using UnityEngine;

namespace Common
{
    public class RoadResourcesHolder : MonoBehaviour
    {
        [SerializeField] private GameObject Road;
        [SerializeField] private GameObject Corner;
        [SerializeField] private GameObject Crossroad;
        [SerializeField] private GameObject HalfRoad;
        [SerializeField] private GameObject TCrossroad;
        [SerializeField] private GameObject DualRoad;

        private static Resources? _resources = null;

        public Resources GetResources()
        {
            _resources ??= new Resources(Road, Corner, Crossroad, HalfRoad, TCrossroad, DualRoad);
            return _resources.Value;
        }

        public struct Resources
        {
            public readonly GameObject Road;
            public readonly GameObject Corner;
            public readonly GameObject Crossroad;
            public readonly GameObject HalfRoad;
            public readonly GameObject TCrossroad;
            public readonly GameObject DualRoad;

            public Resources(GameObject road, GameObject corner, GameObject crossroad, GameObject halfRoad, GameObject tCrossroad, GameObject dualRoad)
            {
                Road = road;
                Corner = corner;
                Crossroad = crossroad;
                HalfRoad = halfRoad;
                TCrossroad = tCrossroad;
                DualRoad = dualRoad;
            }
        }
    }
}