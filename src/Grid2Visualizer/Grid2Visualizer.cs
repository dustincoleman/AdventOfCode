using System;
using System.Diagnostics;
using AdventOfCode.Common;
using Grid2Visualizer.Remote;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Microsoft.VisualStudio.Utilities;
using WinForms = System.Windows.Forms;

[assembly: DebuggerVisualizer(
    typeof(Grid2Visualizer.Grid2DialogVisualizer),
    typeof(Grid2ObjectSource),
    Target = typeof(Grid2<>),
    Description = "Grid2 Visualizer")]

namespace Grid2Visualizer
{
    public class Grid2DialogVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            IDialogVisualizerService modalService = windowService ?? throw new ApplicationException("This debugger does not support modal visualizers");
            WinForms.IWin32Window parentWindow = windowService as WinForms.IWin32Window ?? throw new ApplicationException("This debugger does not support modal visualizers");

            using (DpiAwareness.EnterDpiScope(DpiAwarenessContext.PerMonitorAwareV2))
            {
                IGrid2 grid = (IGrid2)((IVisualizerObjectProvider2)objectProvider).GetObject();
                Grid2VisualizerWindow window = new Grid2VisualizerWindow(grid);

                window.SetOwner(parentWindow.Handle);
                window.RemoveIcon();
                window.RemoveMinButton();
                window.ShowDialog();
            }
        }
    }
}
