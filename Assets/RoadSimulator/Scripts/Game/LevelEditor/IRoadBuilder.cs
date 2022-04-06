using UnityEngine;

namespace RoadSimulator.Scripts.Game.LevelEditor
{
    public interface IRoadBuilder
    {
        public void OnAddRoadAt(Vector2Int coord);
        public void OnRemoveRoadAt(Vector2Int coord);
        public void OnRotate90At(bool clockwise, Vector2Int coord);
        public void OnSelectAt(Vector2Int coord);

    }
}