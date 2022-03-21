using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoadSimulator.Scripts.UI.View
{
    public class ToolView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI toolText;
        [SerializeField] private Image selectImage;
        [SerializeField] private Image toolImage;

        private ICallback _callback;

        private bool _isOn;

        public void Init(ICallback callback, string toolName)
        {
            toolText.text = toolName;
            _callback = callback;
        }

        public void OnClick()
        {
            _isOn = !_isOn;
            UpdateRender();

            _callback.OnToolSelected(_isOn);
        }

        public void Unselect()
        {
            _isOn = false;
            UpdateRender();
        }

        private void UpdateRender()
        {
            selectImage.enabled = _isOn;
        }

        public interface ICallback
        {
            public void OnToolSelected(bool newValue);
        }
    }
}