using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Common
{
    public class VirtualGrid3<T> : IEnumerable<VirtualGrid3Region<T>>
    {
        private List<VirtualGrid3Region<T>> regions = new List<VirtualGrid3Region<T>>();

        public VirtualGrid3()
        {
        }

        public void Set(VirtualGrid3Region<T> regionToSet)
        {
            List<VirtualGrid3Region<T>> newRegions = new List<VirtualGrid3Region<T>>();

            foreach (VirtualGrid3Region<T> existingRegion in regions)
            {
                if (regionToSet.Contains(existingRegion))
                {
                    continue;
                }

                if (existingRegion.Intersects(regionToSet, out VirtualGrid3Region<T> intersection))
                {
                    newRegions.AddRange(existingRegion.Without(intersection));
                }
                else
                {
                    newRegions.Add(existingRegion);
                }
            }

            newRegions.Add(regionToSet);

            this.regions = newRegions;
        }

        public IEnumerator<VirtualGrid3Region<T>> GetEnumerator() => this.regions.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
