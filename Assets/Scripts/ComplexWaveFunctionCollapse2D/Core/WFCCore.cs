using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public class WFCCore
    {
        OutputGrid outputGrid;
        PatternManager patternManager;
        private int maxIterations=0;
        public WFCCore(int outputWidth,int outputHeight,int maxIterations, PatternManager patternManager)
        {
            this.outputGrid = new OutputGrid(outputWidth,outputHeight,patternManager.GetNumberOfPatterns());
            this.patternManager = patternManager;
            this.maxIterations = maxIterations;
        }
        public int[][] CreateOutputGrid()
        {
            int iteration=0;
            while (iteration<this.maxIterations)
            {
                CoreSolver solver = new CoreSolver(this.outputGrid,this.patternManager);
                int innerIteration = 1000;
                while (!solver.CheckForConflicts() && !solver.IsSolved())
                {
                    Vector2Int position = solver.GetLowestEntropyCell();
                    solver.CollapseCell(position);
                    solver.Propagate();
                    innerIteration--;
                    if(innerIteration<=0)
                    {
                        Debug.Log("Propagation Is Stuck");
                        return new int[0][];
                    }
                }
                if(solver.CheckForConflicts())
                {
                    Debug.Log("\n conflict occurred. Iteration:" + iteration);
                    iteration++;
                    outputGrid.ResetAllPossibilities();
                    solver = new CoreSolver(this.outputGrid,this.patternManager);
                }
                else
                {
                    Debug.Log("Solved On: " + iteration);
                    this.outputGrid.PrintResultsToConsole();
                    break;
                }
            }
            if(iteration>=this.maxIterations)
                {
                    Debug.Log("Could not solve the tilemap");
                }
            return outputGrid.GetSolvedOutputGrid();
        }
    }
}

