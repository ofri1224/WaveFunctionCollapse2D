using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public interface IFIndNeighborStrategy
    {
        public Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResult patternFinderResult);
    }
}

