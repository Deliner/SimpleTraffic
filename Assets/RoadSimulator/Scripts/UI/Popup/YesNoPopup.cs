using System;
using RoadSimulator.Scripts.UI.Common;

namespace RoadSimulator.Scripts.UI.Popup
{
    public class YesNoPopup : BasePopup
    {
        public void OnYesClicked()
        {
            (GetArguments() as Action)?.Invoke();
            Close();
        }

        public void OnNoClicked()
        {
            Close();
        }
    }
}