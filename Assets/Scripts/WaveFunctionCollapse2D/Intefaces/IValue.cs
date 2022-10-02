using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse2D
{
    public interface IValue<T> : IEqualityComparer<IValue<T>> , IEquatable<IValue<T>>
    {
        T Value {get;}
    }
}

