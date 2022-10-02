using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public class NeighborStrategySize2OrMore : IFIndNeighborStrategy
    {
        public Dictionary<int, PatternNeighbors> FindNeighbors(PatternDataResult patternFinderResult)
        {
            Dictionary<int,PatternNeighbors> result = new Dictionary<int, PatternNeighbors>();
            foreach (var patternData in patternFinderResult.PatternIndexDict)
            {
                foreach (var PossibleNeighbor in patternFinderResult.PatternIndexDict)
                {
                    FindNeighborsInAllDirections(result,patternData,PossibleNeighbor);
                }
            }
            return result;
        }

        private void FindNeighborsInAllDirections(Dictionary<int, PatternNeighbors> result, KeyValuePair<int, PatternData> patternData, KeyValuePair<int, PatternData> possibleNeighbor)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if(patternData.Value.CompareGrid(dir,possibleNeighbor.Value))
                {
                    if (!result.ContainsKey(patternData.Key))
                    {
                        result.Add(patternData.Key,new PatternNeighbors());
                    }
                    result[patternData.Key].AddPatternToDict(dir,possibleNeighbor.Key);
                }
            }
        }
    } 
}

