using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace RoadSimulator.Scripts.Game.Base
{
    public class RoadObjectFactory
    {
        private readonly Resources _resources;

        public RoadObjectFactory(Resources resources)
        {
            _resources = resources;
        }

        public GameObject GetRoadObject(Type roadType)
        {
            return Object.Instantiate(roadType switch
            {
                Type.Road => _resources.Road,
                Type.Corner => _resources.Corner,
                Type.Crossroad => _resources.Crossroad,
                Type.HalfRoad => _resources.HalfRoad,
                Type.CrossroadT => _resources.CrossroadT,
                Type.DualRoad => _resources.DualRoad,
                Type.Generator => _resources.Generator,
                Type.Deletor => _resources.Deletor,
                Type.Plug => _resources.Plug,
                _ => throw new ArgumentOutOfRangeException(nameof(roadType), roadType, null)
            });
        }

        public enum Type
        {
            Road,
            Corner,
            Crossroad,
            HalfRoad,
            CrossroadT,
            DualRoad,
            Generator,
            Deletor,
            Plug
        }

        public struct Resources
        {
            public readonly GameObject Road;
            public readonly GameObject Corner;
            public readonly GameObject Crossroad;
            public readonly GameObject HalfRoad;
            public readonly GameObject CrossroadT;
            public readonly GameObject DualRoad;
            public readonly GameObject Generator;
            public readonly GameObject Deletor;
            public readonly GameObject Plug;

            public Resources(
                GameObject road, GameObject corner, GameObject crossroad,
                GameObject halfRoad, GameObject tCrossroad, GameObject dualRoad,
                GameObject generator, GameObject deletor, GameObject plug
            )
            {
                Road = road;
                Corner = corner;
                Crossroad = crossroad;
                HalfRoad = halfRoad;
                CrossroadT = tCrossroad;
                DualRoad = dualRoad;
                Generator = generator;
                Deletor = deletor;
                Plug = plug;
            }
        }
    }
}