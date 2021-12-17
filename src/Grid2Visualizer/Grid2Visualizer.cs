using System;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Microsoft.VisualStudio.Utilities;
using WinForms = System.Windows.Forms;

namespace Grid2Visualizer
{
    public class Grid2Visualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            IDialogVisualizerService modalService = windowService ?? throw new ApplicationException("This debugger does not support modal visualizers");
            WinForms.IWin32Window parentWindow = windowService as WinForms.IWin32Window ?? throw new ApplicationException("This debugger does not support modal visualizers");

            using (DpiAwareness.EnterDpiScope(DpiAwarenessContext.PerMonitorAwareV2))
            {
                Grid2VisualizerWindow window = new Grid2VisualizerWindow();

                window.SetOwner(parentWindow.Handle);
                window.RemoveIcon();
                window.RemoveMinButton();
                window.ShowDialog();
            }
        }
    }
}
