using System.Collections.Generic;
using RoadSimulator.Scripts.UI.Adapter;

namespace Kawaiiju.Traffic.LevelEditor
{
    public static class ToolManager
    {
        private static readonly List<Tool> _toolList = new List<Tool>() { new Road(), new Corner(), new Crossroad(), new ShortRoad(), new TCrossroad() };

        public static List<Tool> GetToolList()
        {
            return _toolList;
        }
    }
}