using UnityEngine;

namespace Kawaiiju.Traffic.LevelEditor
{
    public abstract class Tool
    {
        public abstract void Action();
       
        public abstract string GetToolName();
    }
    
    public class SelectTool : Tool
    {
        public override void Action()
        {
        }

        public override string GetToolName()
        {
            return "SelectTool";
        }
    }

    public abstract class ModifyTool : Tool
    {
        
    }

    public abstract class RoadTool : Tool
    {
        public abstract RoadFactory.Type GetRoadType();
    }

    public class Road : RoadTool
    {
        public override void Action()
        {
        }

        public override RoadFactory.Type GetRoadType()
        {
            return RoadFactory.Type.Road;
        }

        public override string GetToolName()
        {
            return "Road";
        }
    }

    public class Corner : RoadTool
    {
        public override void Action()
        {
        }

        public override RoadFactory.Type GetRoadType()
        {
            return RoadFactory.Type.Corner;
        }

        public override string GetToolName()
        {
            return "Corner";
        }
    }

    public class Crossroad : RoadTool
    {
        public override void Action()
        {
        }

        public override RoadFactory.Type GetRoadType()
        {
            return RoadFactory.Type.Crossroad;
        }

        public override string GetToolName()
        {
            return "Crossroad";
        }
    }

    public class TCrossroad : RoadTool
    {
        public override void Action()
        {
        }

        public override RoadFactory.Type GetRoadType()
        {
            return RoadFactory.Type.TCrossroad;
        }

        public override string GetToolName()
        {
            return "TCrossroad";
        }
    }

    public class HalfRoad : RoadTool
    {
        public override void Action()
        {
        }

        public override RoadFactory.Type GetRoadType()
        {
            return RoadFactory.Type.HalfRoad;
        }

        public override string GetToolName()
        {
            return "HalfRoad";
        }
    }
    
    public class DualRoad : RoadTool
    {
        public override void Action()
        {
        }

        public override RoadFactory.Type GetRoadType()
        {
            return RoadFactory.Type.DualRoad;
        }

        public override string GetToolName()
        {
            return "DualRoad";
        }
    }
}