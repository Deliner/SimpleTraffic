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
            throw new System.NotImplementedException();
        }

        public override string GetToolName()
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class ObjectTool : Tool
    {
        
    }

    public class Road : ObjectTool
    {
        public override void Action()
        {
        }

        public override string GetToolName()
        {
            return "Road";
        }
    }

    public class Corner : ObjectTool
    {
        public override void Action()
        {
        }

        public override string GetToolName()
        {
            return "Corner";
        }
    }

    public class Crossroad : ObjectTool
    {
        public override void Action()
        {
        }

        public override string GetToolName()
        {
            return "Crossroad";
        }
    }

    public class TCrossroad : ObjectTool
    {
        public override void Action()
        {
        }

        public override string GetToolName()
        {
            return "TCrossroad";
        }
    }

    public class ShortRoad : ObjectTool
    {
        public override void Action()
        {
        }

        public override string GetToolName()
        {
            return "ShortRoad";
        }
    }
}