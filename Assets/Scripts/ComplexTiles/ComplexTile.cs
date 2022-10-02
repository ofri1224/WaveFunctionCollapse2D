using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Tilemaps.Tile;
namespace ComplexTile
{
    public class ComplexTile : TileBase
    {
        public Sprite sprite;
        public Color color = Color.white;
        public Matrix4x4 transform = Matrix4x4.identity;
        public GameObject gameObject = null;
        public TileFlags flags = TileFlags.LockColor;
        public ColliderType colliderType = ColliderType.Sprite;
        public Vector3 rotation;
        public float c_rot;

        public override void RefreshTile(Vector3Int location, ITilemap tilemap)
        {
            tilemap.RefreshTile(location);
        }
        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite=sprite;
            tileData.color=color;
            tileData.transform=transform;
            tileData.gameObject=gameObject;
            tileData.flags=flags;
            tileData.colliderType=colliderType;
        }
        public void Rotate(bool Clockwise)
        {
            Debug.Log(Clockwise);
            
            if(Clockwise)
            {
                if(c_rot>=3)
                {
                    c_rot=0;
                }
                else
                {
                    c_rot++;
                }
            }
            else
            {
                if(c_rot<=0)
                {
                    c_rot=3;
                }
                else
                {
                    c_rot--;
                }
            }
            transform.SetTRS(Vector3.zero,Quaternion.Euler(0f,0f,c_rot*90f),Vector3.one);
            //bool updated = false;
            // tilemap = gameObject.GetComponent<Tilemap>();
            // foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            // {
            //     TileBase tile = tilemap.GetTile(position);
            //     if(tile&&tile is ComplexTile)
            //     {

            //     }
            // }
        }
        // public override bool GetTileAnimationData(Vector3Int location, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        // {

        // }
        // public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
        // {

        // }
        // public Vector3Int location;
        // public ITilemap tilemap;
        // public TileData TileData;
        



        // void Start()
        // {
        //     Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(rotation), Vector3.one);
        //     tilemap.SetTransformMatrix(tilePos, matrix);
        // }
    #if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a ComplexTile Asset
        [MenuItem("Assets/Create/ComplexTile")]
        public static void CreateComplexTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Complex Tile", "New Complex Tile", "Asset", "Save Complex Tile", "Assets");
            if (path == "")
                return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ComplexTile>(), path);
        }
    #endif
    }
}

