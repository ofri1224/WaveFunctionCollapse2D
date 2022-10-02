using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ComplexTile
{
    [CustomGridBrush(true, false, false, "Complex Brush")]
    public class ComplexTileBrush :  GridBrush
    {
        public int rotation = 0;
        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            //brushTarget.transform.Rotate(0,0,rotation);
            
            base.Paint(grid,brushTarget,position);
        }
        public void RotateTile(bool clockwise)
        {
            if(clockwise)
            {
                if(rotation>=270)
                {
                    rotation=0;
                }
                else
                {
                    rotation+=90;
                }
            }
            else
            {
                if(rotation<=0)
                {
                    rotation=270;
                }
                else
                {
                    rotation-=90;
                }
            }
        }
        [MenuItem("Assets/Create/Complex Brush")]
        public static void CreateBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Complex Brush", "New Complex Brush", "Asset", "Save Complex Brush", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ComplexTileBrush>(), path);
        }
        // http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
    }
    [CustomEditor(typeof(ComplexTileBrush))]
    public class LineBrushEditor : GridBrushEditor
    {
        public int rotation = 0;
        private ComplexTileBrush complexTileBrush { get { return target as ComplexTileBrush; } }
        public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
        {
            if(Event.current.isScrollWheel)
            {
                if(Event.current.delta.y>0)
                {
                    //complexTileBrush.RotateTile(true);
                    //RotateTile(false);
                    this.complexTileBrush.Rotate(GridBrushBase.RotationDirection.Clockwise,grid.cellLayout);
                    Debug.Log("down");
                }
                if(Event.current.delta.y<0)
                {
                    //complexTileBrush.RotateTile(true);
                    //RotateTile(true);
                    this.complexTileBrush.Rotate(GridBrushBase.RotationDirection.CounterClockwise,grid.cellLayout);
                    Debug.Log("up");
                }
                Event.current.Use();
            }
            brushTarget.transform.rotation = Quaternion.Euler(0,0,rotation);
            base.OnPaintSceneGUI(grid,brushTarget,position,tool,executing);

            // base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);
            // if (complexTileBrush.lineStartActive)
            // {
            //     Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
            //     if (tilemap != null)
            //         tilemap.ClearAllEditorPreviewTiles();
            //     // Draw preview tiles for tilemap
            //     Vector2Int startPos = new Vector2Int(complexTileBrush.lineStart.x, complexTileBrush.lineStart.y);
            //     Vector2Int endPos = new Vector2Int(position.x, position.y);
            //     if (startPos == endPos)
            //         PaintPreview(grid, brushTarget, position.min);
            //     else
            //     {
            //         foreach (var point in ComplexTileBrush.GetPointsOnLine(startPos, endPos))
            //         {
            //             Vector3Int paintPos = new Vector3Int(point.x, point.y, position.z);
            //             PaintPreview(grid, brushTarget, paintPos);
            //         }
            //     }
            //     if (Event.current.type == EventType.Repaint)
            //     {
            //         var min = complexTileBrush.lineStart;
            //         var max = complexTileBrush.lineStart + position.size;
            //         // Draws a box on the picked starting position
            //         GL.PushMatrix();
            //         GL.MultMatrix(GUI.matrix);
            //         GL.Begin(GL.LINES);
            //         Handles.color = Color.blue;
            //         Handles.DrawLine(new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z));
            //         Handles.DrawLine(new Vector3(max.x, min.y, min.z), new Vector3(max.x, max.y, min.z));
            //         Handles.DrawLine(new Vector3(max.x, max.y, min.z), new Vector3(min.x, max.y, min.z));
            //         Handles.DrawLine(new Vector3(min.x, max.y, min.z), new Vector3(min.x, min.y, min.z));
            //         GL.End();
            //         GL.PopMatrix();
            //     }
            // }
        }
        public void RotateTile(bool clockwise)
        {
            if(clockwise)
            {
                if(rotation>=270)
                {
                    rotation=0;
                }
                else
                {
                    rotation+=90;
                }
            }
            else
            {
                if(rotation<=0)
                {
                    rotation=270;
                }
                else
                {
                    rotation-=90;
                }
            }
        }
    }
}

