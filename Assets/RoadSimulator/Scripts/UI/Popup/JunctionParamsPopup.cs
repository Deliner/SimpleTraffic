﻿using System;
using System.Globalization;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.World;
using RoadSimulator.Scripts.UI.Common;
using TMPro;
using UnityEngine;

namespace RoadSimulator.Scripts.UI.Popup
{
    public class JunctionParamsPopup : BasePopup
    {
        [SerializeField] private TMP_InputField phaseIntervalInput;
        [SerializeField] private TMP_InputField maxSpeedInput;

        private JunctionParams _junctionParams;

        public override void OnCreated()
        {
            _junctionParams = GetArguments() as JunctionParams;

            phaseIntervalInput.text = _junctionParams!.PhaseInterval.ToString(CultureInfo.InvariantCulture);
            maxSpeedInput.text = _junctionParams!.MaxSpeed.ToString();
        }

        public void OnApplyClicked()
        {
            if (int.TryParse(maxSpeedInput.text, out var maxSpeed) && float.TryParse(phaseIntervalInput.text, out var phaseInterval))
            {
                _junctionParams.PhaseInterval = Math.Clamp(phaseInterval, SimulationInfo.MinPhaseInterval, SimulationInfo.MaxPhaseInterval);
                _junctionParams.MaxSpeed = Math.Clamp(maxSpeed, SimulationInfo.MinCarSpeed, SimulationInfo.MaxCarSpeed);
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