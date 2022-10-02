using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
        
    }
}

