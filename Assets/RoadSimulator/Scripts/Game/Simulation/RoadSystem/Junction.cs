using System;
using RoadSimulator.Scripts.Game.Simulation.World;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class Junction : Road
    {
        public Phase[] phases;

        public float phaseInterval = 5;

        private int _mCurrentPhase;
        private float _phaseTimer;

        public void Start()
        {
            if (phases.Length > 0)
                phases[0].Apply();
        }

        private void Update()
        {
            _phaseTimer += Time.deltaTime;
            if (_phaseTimer > phaseInterval)
                ChangePhase();
        }

        private void ChangePhase()
        {
            _phaseTimer = 0;
            phases[_mCurrentPhase].Undo();

            SwitchPhase();
            phases[_mCurrentPhase].Apply();
        }

        private void SwitchPhase()
        {
            if (_mCurrentPhase < phases.Length - 1)
                _mCurrentPhase++;
            else
                _mCurrentPhase = 0;
        }

        private void OnDrawGizmos()
        {
            if (TrafficSystem.instance.drawGizmos)
            {
                var phase = phases[_mCurrentPhase];
                foreach (var zone in phase.positiveZones)
                {
                    Gizmos.color = zone.canPass ? Color.green : Color.red;
                    DrawAreaGizmo(zone.transform);
                }

                Gizmos.color = Color.red;
                foreach (var zone in phase.negativeZones)
                    DrawAreaGizmo(zone.transform);
            }
        }

        private static void DrawAreaGizmo(Transform t)
        {
            var rotationMatrix = Matrix4x4.TRS(t.position - new Vector3(0, t.localScale.y * 0.5f, 0), t.rotation, Vector3.Scale(t.lossyScale, new Vector3(1f, 0.1f, 1f)));
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireMesh(GizmoHolder.cube, Vector3.zero, Quaternion.identity);
        }

        [Serializable]
        public class Phase
        {
            public TrafficLight[] positiveLights;
            public TrafficLight[] negativeLights;
            public WaitZone[] positiveZones;
            public WaitZone[] negativeZones;

            public void Apply()
            {
                foreach (var zone in positiveZones)
                    zone.canPass = true;
                foreach (var light in positiveLights)
                    light.SetLight(true);

                foreach (var zone in negativeZones)
                    zone.canPass = false;
                foreach (var light in negativeLights)
                    light.SetLight(false);
            }

            public void Undo()
            {
                foreach (var zone in positiveZones)
                    zone.canPass = false;
                foreach (var light in positiveLights)
                    light.SetLight(false);
            }
        }

        private static class GizmoHolder
        {
            public static Mesh cube
            {
                get
                {
                    if (_cube == null)
                    {
                        var primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        _cube = primitive.GetComponent<MeshFilter>().sharedMesh;
                        DestroyImmediate(primitive);
                    }

                    return _cube;
                }
            }

            private static Mesh _cube;
        }
    }
}