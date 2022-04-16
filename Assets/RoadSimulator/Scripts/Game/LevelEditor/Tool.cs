using RoadSimulator.Scripts.Game.Base;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public interface ITool
    {
        public string GetToolName();
    }

    public interface IRoadBuilderTool : ITool
    {
        public void ActionAt(IRoadBuilder builder, Vector2Int coord);
        public RoadObjectFactory.Type? GetRoadType();
    }

    public class SelectTool : IRoadBuilderTool
    {
        public string GetToolName() => "Sel";
        public void ActionAt(IRoadBuilder builder, Vector2Int coord) => builder.OnSelectAt(coord);
        public RoadObjectFactory.Type? GetRoadType() => null;
    }

    public class RemoveTool : IRoadBuilderTool
    {
        public string GetToolName() => "Del";
        public void ActionAt(IRoadBuilder builder, Vector2Int coord) => builder.OnRemoveRoadAt(coord);
        public RoadObjectFactory.Type? GetRoadType() => null;
    }

    public class Rotate90Tool : IRoadBuilderTool
    {
        public string GetToolName() => "+90°";
        public void ActionAt(IRoadBuilder builder, Vector2Int coord) => builder.OnRotate90At(true, coord);
        public RoadObjectFactory.Type? GetRoadType() => null;
    }

    public abstract class BaseRoadTool : IRoadBuilderTool
    {
        public abstract RoadObjectFactory.Type? GetRoadType();
        public abstract string GetToolName();
        
        public void ActionAt(IRoadBuilder builder, Vector2Int coord) => builder.OnAddRoadAt(coord);
    }

    public class RoadTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() =>RoadObjectFactory.Type.Road;
        public override string GetToolName() => "R1";
    }
    
    public class PlugTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() =>RoadObjectFactory.Type.Plug;
        public override string GetToolName() => "End";
    }

    public class CornerTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() =>  RoadObjectFactory.Type.Corner;
        public override string GetToolName() => "C";
    }

    public class CrossroadTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() => RoadObjectFactory.Type.Crossroad;
        public override string GetToolName() => "X";
    }

    public class TcrossroadTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() => RoadObjectFactory.Type.CrossroadT;
        public override string GetToolName() => "T";
    }

    public class HalfRoadTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() => RoadObjectFactory.Type.HalfRoad;
        public override string GetToolName() => "R0.5";
    }

    public class DualRoadTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() => RoadObjectFactory.Type.DualRoad;
        public override string GetToolName() => "R2";
    }
    
    public class GeneratorTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() => RoadObjectFactory.Type.Generator;
        public override string GetToolName() => "Generator";
    }
    
    public class DeletorTool : BaseRoadTool
    {
        public override RoadObjectFactory.Type? GetRoadType() => RoadObjectFactory.Type.Deletor;
        public override string GetToolName() => "Deletor";
    }
}