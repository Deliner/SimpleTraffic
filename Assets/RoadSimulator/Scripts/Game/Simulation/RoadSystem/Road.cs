using System;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.Agent;
using UnityEngine;

namespace RoadSimulator.Scripts.Game.Simulation.RoadSystem
{
    public class Road : NavSection
    {
        public IBandwidthInformer informer => new BandwidthInformer(this);
        private float carBandwidth => _carPassed / _timePassed;

        private const float MaxTimePeriod = 120f;

        private float _timePassed;
        private int _carPassed;

        private void Update()
        {
            _timePassed += Time.deltaTime;
        }

        public override void RegisterVehicle(Vehicle input, bool isAdd)
        {
            base.RegisterVehicle(input, isAdd);

            if (!isAdd)
                UpdateCarBandwidth();
        }

        public override void SetParams(IRoadParams roadParams)
        {
            if (roadParams is not RoadParams @params)
                throw new Exception($"Expected {typeof(RoadParams)} but received {roadParams.GetParamsType()}");

            speedLimit = @params.MaxSpeed;
        }

        private void UpdateCarBandwidth()
        {
            _carPassed++;
            if (_carPassed == int.MaxValue || _timePassed > MaxTimePeriod)
            {
                _timePassed /= 2;
                _carPassed /= 2;
            }
        }

        private sealed class BandwidthInformer : IBandwidthInformer
        {
            private Road _road;

            public float value => _road.carBandwidth;

            internal BandwidthInformer(Road road)
            {
                _road = road;
            }
        }

        public interface IBandwidthInformer
        {
            public float value { get; }
        }
    }
}