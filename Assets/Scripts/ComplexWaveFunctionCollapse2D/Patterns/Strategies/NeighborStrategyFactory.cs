using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ComplexWaveFunctionCollapse2D
{
    public class NeighborStrategyFactory
    {
        Dictionary<string, Type> strategies;
        public NeighborStrategyFactory()
        {
            LoadTypesIFindNeighborsStrategy();
        }

        private void LoadTypesIFindNeighborsStrategy()
        {
            strategies = new Dictionary<string, Type>();
            Type[] typesInThisAssembly = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in typesInThisAssembly)
            {
                if(type.GetInterface(typeof(IFIndNeighborStrategy).ToString())!=null)
                {
                    strategies.Add(type.Name.ToLower(),type);
                }
            }
        }

        internal IFIndNeighborStrategy CreateInstance(string NameOfStrategy)
        {
            Type t = GetTypeToCreate(NameOfStrategy);
            if(t==null)
            {
                t = GetTypeToCreate("more");
            }
            return Activator.CreateInstance(t) as IFIndNeighborStrategy;
        }

        private Type GetTypeToCreate(string nameOfStrategy)
        {
            foreach (var possibleStrategy in strategies)
            {
                if(possibleStrategy.Key.Contains(nameOfStrategy))
                {
                    return possibleStrategy.Value;
                }
            }
            return null;
        }
    }
}


