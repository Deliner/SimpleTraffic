﻿using System;
using System.Collections.Generic;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public static class ToolFactory
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
            foreach (var type in (Type[])Enum.GetValues(typeof(Type)))
            {
                if (type is not (Type.Generator or Type.Deletor))
                {
                    toolList.Add(GetTool(type));
                }
            }

            toolList.Remove(new GeneratorTool());
            toolList.Remove(new DeletorTool());
            return toolList;
        }

        private static ITool GetTool(Type type)
        {
            return type switch
            {
                Type.Corner => new CornerTool(),
                Type.Road => new RoadTool(),
                Type.Plug => new PlugTool(),
                Type.Crossroad => new CrossroadTool(),
                Type.CrossroadT => new TcrossroadTool(),
                Type.HalfRoad => new HalfRoadTool(),
                Type.DualRoad => new DualRoadTool(),
                Type.Deletor => new DeletorTool(),
                Type.Generator => new GeneratorTool(),
                Type.Select => new SelectTool(),
                Type.Rotate90 => new Rotate90Tool(),
                Type.Remove => new RemoveTool(),
                _ => throw new Exception()
            };
        }

        private enum Type
        {
            Plug,
            Road,
            DualRoad,
            HalfRoad,
            Corner,
            CrossroadT,
            Crossroad,
            Generator,
            Deletor,
            Select,
            Rotate90,
            Remove
        }
    }
}