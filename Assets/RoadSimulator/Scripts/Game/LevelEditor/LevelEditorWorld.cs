using UnityEngine;

namespace Kawaiiju.Traffic.LevelEditor
{
    public class LevelEditorWorld
    {
        private const float WorldGridStep = 0.25f;
        
        public static Vector3 TransformPositionToWorldGrid(Vector3 position)
        {
            var x = (int)(position.x / WorldGridStep);
            var z = (int)(position.z / WorldGridStep);
            return new Vector3(x * WorldGridStep,0 , z * WorldGridStep);
        }
    }
}