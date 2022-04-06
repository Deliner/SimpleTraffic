using System;
using System.Collections.Generic;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public static class  ToolFactory
    {
        private static List<ITool> _toolList;
        
        public static List<ITool> GetAllTools()
        {
            _toolList ??= GetToolList();
            return _toolList;
        }

        private static List<ITool> GetToolList()
        {
            var toolList = new List<ITool>();
            foreach (var type in (Type[]) Enum.GetValues(typeof(Type)))
            {
                toolList.Add(GetTool(type));
            }
            return toolList;
        }

        private static ITool GetTool(Type type)
        {
            return type switch
            {
                Type.Corner => new Corner(),
                Type.Road => new Road(),
                Type.Crossroad => new Crossroad(),
                Type.CrossroadT => new CrossroadT(),
                Type.HalfRoad => new HalfRoad(),
                Type.DualRoad => new DualRoad(),
                Type.Select => new SelectTool(),
                Type.Rotate90 => new Rotate90Tool(),
                Type.Remove => new RemoveTool(),
                _ => throw new Exception()
            };
        }

        private enum Type
        {
            Road,
            Corner,
            Crossroad,
            CrossroadT,
            HalfRoad,
            DualRoad,
            Select,
            Rotate90,
            Remove
        }
    }
}