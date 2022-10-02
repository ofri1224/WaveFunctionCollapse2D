using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace ComplexTile
{
    [CustomEditor(typeof(ComplexTile))]
    [CanEditMultipleObjects]
    public class ComplexTileEditor : Editor
    {
        //SerializedProperty transform;
        ComplexTile complexTile;

         void OnEnable()
        {
            //transform = serializedObject.FindProperty("transform");
            complexTile = (ComplexTile)target;
        }
        // public override void OnInspectorGUI()
        // {
        //     base.OnInspectorGUI();
        //     HandleKeyboard();
        // }
        public void OnSceneGUI()
        {
            HandleKeyboard();
        }
        private void HandleKeyboard()
        {
            Event current = Event.current;
            if (current.type != EventType.MouseDown)
                return;
            Debug.Log("click");
            if(current.button==1)
            {
                if(current.control)
                {
                    complexTile.Rotate(false);
                }
                else
                {
                    complexTile.Rotate(true);
 
                }
                EditorUtility.SetDirty(complexTile);
            }
            // switch(current.keyCode)
            // {
            //     case :
            //         if(current.control)
            //         {
            //             complexTile.Rotate(false);
            //         }
            //         else
            //         {
            //             complexTile.Rotate(true);
            //         }
            //         EditorUtility.SetDirty(complexTile);
            //         break;
            // }
        }
    }
}
