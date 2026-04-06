using System;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    /// <summary>
    /// ToolStrip subclass that processes clicks even when the parent form is inactive.
    /// Fixes the standard WinForms issue where the first click on a ToolStrip button
    /// only activates the form/toolstrip without firing the button's Click event.
    /// </summary>
    internal class ClickThroughToolStrip : ToolStrip
    {
        private const int WM_MOUSEACTIVATE = 0x0021;
        private const int MA_ACTIVATE = 1;
        private const int MA_ACTIVATEANDEAT = 2;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE && m.Result == (IntPtr)MA_ACTIVATEANDEAT)
                m.Result = (IntPtr)MA_ACTIVATE;
            base.WndProc(ref m);
        }
    }
}
