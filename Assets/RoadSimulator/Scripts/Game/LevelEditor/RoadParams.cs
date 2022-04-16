using RoadSimulator.Scripts.Game.Simulation.RoadSystem;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public interface IRoadParams
    {
        public RoadParamsFactory.Type GetParamsType();
    }

    public class RoadParams : IRoadParams
    {
        public int MaxSpeed = 40;
        public virtual RoadParamsFactory.Type GetParamsType() => RoadParamsFactory.Type.Default;
    }

    public class JunctionParams : RoadParams
    {
        public float PhaseInterval = 10f;

        public override RoadParamsFactory.Type GetParamsType() => RoadParamsFactory.Type.Junction;
    }

    public class PlugParams : RoadParams
    {
        public VehicleSpawn.SpawnParams SpawnParams = new();

        public override RoadParamsFactory.Type GetParamsType() => RoadParamsFactory.Type.Plug;
    }
}