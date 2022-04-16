using System.Globalization;
using RoadSimulator.Scripts.Game.LevelEditor;
using RoadSimulator.Scripts.Game.Simulation.RoadSystem;
using RoadSimulator.Scripts.UI.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoadSimulator.Scripts.UI.Popup
{
    public class PlugParamsPopup : BasePopup
    {
        [SerializeField] private TMP_InputField constantPeriodInput;
        [SerializeField] private TMP_InputField maxPeriodInput;
        [SerializeField] private TMP_InputField minPeriodInput;
        [SerializeField] private Toggle periodToggle;

        [SerializeField] private TMP_InputField constantSpeedInput;
        [SerializeField] private TMP_InputField maxSpeedInput;
        [SerializeField] private TMP_InputField minSpeedInput;
        [SerializeField] private Toggle speedToggle;

        private VehicleSpawn.SpawnParams _spawnParams;
        private PlugParams _plugParams;

        public override void OnCreated()
        {
            _plugParams = GetArguments() as PlugParams;
            _spawnParams = _plugParams!.SpawnParams;


            constantPeriodInput.text = _spawnParams.constantPeriod.ToString(CultureInfo.InvariantCulture);
            maxPeriodInput.text = _spawnParams.maxPeriod.ToString(CultureInfo.InvariantCulture);
            minPeriodInput.text = _spawnParams.minPeriod.ToString(CultureInfo.InvariantCulture);
            periodToggle.isOn = _spawnParams.setConstantPeriod;

            constantSpeedInput.text = _spawnParams.constantSpeed.ToString(CultureInfo.InvariantCulture);
            maxSpeedInput.text = _spawnParams.maxSpeed.ToString(CultureInfo.InvariantCulture);
            minSpeedInput.text = _spawnParams.minSpeed.ToString(CultureInfo.InvariantCulture);
            speedToggle.isOn = _spawnParams.setConstantSpeed;
        }

        public void OnApplyClicked()
        {
            if (
                float.TryParse(constantPeriodInput.text, out var constantPeriod) &&
                float.TryParse(maxPeriodInput.text, out var maxPeriod) &&
                float.TryParse(minPeriodInput.text, out var minPeriod) &&
                int.TryParse(constantSpeedInput.text, out var constantSpeed) &&
                int.TryParse(maxSpeedInput.text, out var maxSpeed) &&
                int.TryParse(minSpeedInput.text, out var minSpeed)
            )
            {
                _spawnParams.constantPeriod = constantPeriod;
                _spawnParams.maxPeriod = maxPeriod;
                _spawnParams.minPeriod = minPeriod;

                _spawnParams.constantSpeed = constantSpeed;
                _spawnParams.maxSpeed = maxSpeed;
                _spawnParams.minSpeed = minSpeed;

                _spawnParams.setConstantPeriod = periodToggle.isOn;
                _spawnParams.setConstantSpeed = speedToggle.isOn;

                _spawnParams.Validate();

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