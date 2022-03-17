using System;
using Common;

public class YesNoPopup : BasePopup
{
    public void OnYesClicked()
    {
        (GetArguments() as Action)?.Invoke();
    }

    public void OnNoClicked()
    {
        Close();
    }
}
