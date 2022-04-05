using System.Collections.Generic;
using RoadSimulator.Scripts.UI.Adapter;

namespace Kawaiiju.Traffic.LevelEditor
{
    public static class ToolManager
    {
        private static readonly List<Tool> ToolList = new() { new Road(), new Corner(), new Crossroad(), new HalfRoad(), new TCrossroad(), new DualRoad() };

        public static List<Tool> GetToolList()
        {
            return ToolList;
        }
    }
}