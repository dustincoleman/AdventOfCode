using System;
using System.Diagnostics;
using System.Windows;
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
                Grid2VisualizerWindow window = new Grid2VisualizerWindow((IVisualizerObjectProvider2)objectProvider);

                window.SetOwner(parentWindow.Handle);
                window.RemoveIcon();
                window.RemoveMinButton();
                window.ShowDialog();
            }
        }

        public static void TestShow(StringGrid2 grid)
        {
            try
            {
                VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(grid, typeof(Grid2DialogVisualizer));
                visualizerHost.ShowVisualizer();
            }
            catch (CannotUnloadAppDomainException)
            {
            }
        }
    }
}
