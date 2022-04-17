using System.Collections;
using System.Globalization;
using RoadSimulator.Scripts.Game.Simulation.RoadSystem;
using RoadSimulator.Scripts.UI.Common;
using TMPro;
using UnityEngine;

namespace RoadSimulator.Scripts.UI.Popup
{
    public class BandwidthPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI bandwidthText;

        private Road.IBandwidthInformer _informer;
        private IEnumerator _closeCoroutine;

        public override void OnCreated()
        {
            _informer = GetArguments() as Road.IBandwidthInformer;
        }

        public void OnCloseButtonClicked()
        {
            if (_closeCoroutine != null)
            {
                StopCoroutine(_closeCoroutine);
                _closeCoroutine = null;
            }

            Close();
            Context.OnDialogClosed();
        }

        protected override void Start()
        {
            base.Start();
            _closeCoroutine = CloseScript();
            StartCoroutine(_closeCoroutine);
        }

        private void Update()
        {
            bandwidthText.text = _informer.value.ToString(CultureInfo.InvariantCulture);
        }

        private IEnumerator CloseScript()
        {
            yield return new WaitForSeconds(3);
            Close();
            Context.OnDialogClosed();
        }
    }
}