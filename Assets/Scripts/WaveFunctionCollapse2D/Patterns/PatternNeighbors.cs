using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public class PatternNeighbors
    {
        public Dictionary<Direction,HashSet<int>> DirectionNeighborPatternDict = new Dictionary<Direction, HashSet<int>>();

        public void AddPatternToDict(Direction dir,int patternIndex)
        {
            if(DirectionNeighborPatternDict.ContainsKey(dir))
            {
                DirectionNeighborPatternDict[dir].Add(patternIndex);
            }
            else
            {
                DirectionNeighborPatternDict.Add(dir,new HashSet<int>{patternIndex});
            }
        }
        internal HashSet<int> GetNeighborsInDir(Direction dir)
        {
            if(DirectionNeighborPatternDict.ContainsKey(dir))
            {
                return DirectionNeighborPatternDict[dir];
            }
            return new HashSet<int>();
        }

        public void AddNeighbor(PatternNeighbors neighbors)
        {
            foreach (var item in neighbors.DirectionNeighborPatternDict)
            {
                if(!DirectionNeighborPatternDict.ContainsKey(item.Key))
                {
                    DirectionNeighborPatternDict.Add(item.Key,new HashSet<int>());
                }
                DirectionNeighborPatternDict[item.Key].UnionWith(item.Value);
            }
        }
    }
}

