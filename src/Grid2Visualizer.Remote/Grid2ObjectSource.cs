using AdventOfCode.Common;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.IO;

namespace Grid2Visualizer.Remote
{
    public class Grid2ObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            IGrid2 grid = new ImmutableGrid2((IGrid2)target);
            base.GetData(grid, outgoingData);
        }
    }
}
