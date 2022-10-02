using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
        
    }
}

