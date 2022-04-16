using System;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.World;
using RoadSimulator.Scripts.UI.Common;
using TMPro;
using UnityEngine;

namespace RoadSimulator.Scripts.UI.Popup
{
    public class DefaultRoadParamsPopup : BasePopup
    {
        [SerializeField] private TMP_InputField maxSpeedInput;

        private RoadParams _roadParams;

        public override void OnCreated()
        {
            _roadParams = GetArguments() as RoadParams;
            maxSpeedInput.text = _roadParams!.MaxSpeed.ToString();
        }

        public void OnApplyClicked()
        {
            if (int.TryParse(maxSpeedInput.text, out var maxSpeed))
            {
                _roadParams.MaxSpeed = Math.Clamp(maxSpeed, SimulationInfo.MinCarSpeed, SimulationInfo.MaxCarSpeed);
                Context.OnDialogClosed();
                Close();
            }
        }

        public void OnCloseClicked()
        {
            Context.OnDialogClosed();
            Close();
        }
    }
}