namespace UI;

public static class Helpers
{
    public static void EnableAllControls(Control container)
    {
        SetControlsEnabled(container, true);
    }

    public static void DisableAllControls(Control container)
    {
        SetControlsEnabled(container, false);
    }

    private static void SetControlsEnabled(Control container, bool enabled)
    {
        foreach (Control ctrl in container.Controls)
        {
            ctrl.Enabled = enabled;

            if (ctrl.HasChildren)
            {
                SetControlsEnabled(ctrl, enabled);
            }
        }
    }
}
