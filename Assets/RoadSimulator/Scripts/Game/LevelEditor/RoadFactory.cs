using System;
using Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kawaiiju.Traffic.LevelEditor
{
    public class RoadFactory
    {
        private readonly RoadResourcesHolder.Resources _resources;

        public RoadFactory(RoadResourcesHolder.Resources resources)
        {
            _resources = resources;
        }

        public GameObject GetRoadObject(Type roadType)
        {
            return Object.Instantiate( roadType switch
            {
                Type.Road => _resources.Road,
                Type.Corner => _resources.Corner,
                Type.Crossroad => _resources.Crossroad,
                Type.HalfRoad => _resources.HalfRoad,
                Type.TCrossroad => _resources.TCrossroad,
                Type.DualRoad => _resources.DualRoad,
                _ => throw new ArgumentOutOfRangeException(nameof(roadType), roadType, null)
            });
        }
        
        public enum Type
        {
            Road,
            Corner,
            Crossroad,
            HalfRoad,
            TCrossroad,
            DualRoad
        }
    }
}