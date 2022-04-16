using System;
using RoadSimulator.Scripts.Game.Base;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public static class RoadParamsFactory
    {
        public static IRoadParams GetParams(RoadObjectFactory.Type type)
        {
            return type switch
            {
                RoadObjectFactory.Type.Road => new RoadParams(),
                RoadObjectFactory.Type.Corner => new RoadParams(),
                RoadObjectFactory.Type.Crossroad => new JunctionParams(),
                RoadObjectFactory.Type.HalfRoad => new RoadParams(),
                RoadObjectFactory.Type.CrossroadT => new JunctionParams(),
                RoadObjectFactory.Type.DualRoad => new RoadParams(),
                RoadObjectFactory.Type.Generator => new RoadParams(),
                RoadObjectFactory.Type.Deletor => new RoadParams(),
                RoadObjectFactory.Type.Plug => new PlugParams(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }


        public enum Type
        {
            Default,
            Junction,
            Plug
        }
    }
}