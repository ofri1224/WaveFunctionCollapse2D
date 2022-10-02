using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public interface IOutputCreator<T>
    {
        T OutputImage {get;}
        void CreateOutput(PatternManager manager,int[][] output,int width,int height);
    }

}
