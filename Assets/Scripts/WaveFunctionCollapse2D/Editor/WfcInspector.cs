using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace different
{
    [CustomEditor(typeof(Test))]
    [CanEditMultipleObjects]
    public class WfcInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Test script = (Test)target;
            if(GUILayout.Button("Create tilemap"))
            {
                script.CreateWFC();
                script.CreateTilemap();
            }
            if(GUILayout.Button("Save tilemap"))
            {
                script.SaveTilemap();
            }
        }
    }
}
