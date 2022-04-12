using System;
using RoadSimulator.Scripts.Game.Base;
using RoadSimulator.Scripts.Game.LevelEditor;
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

        public RoadObjectFactory.Resources GetResources()
        {
            return new RoadObjectFactory.Resources(road, corner, crossroad, halfRoad, crossroadT, dualRoad);
        }
    }
}