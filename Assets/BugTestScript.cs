using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BugTestScript : MonoBehaviour
{
    public Tilemap tilemap;
    // Start is called before the first frame update
    public holder[] tiles;
    void Start()
    {
        BoundsInt.PositionEnumerator Enumerator = tilemap.cellBounds.allPositionsWithin;
        int amount = tilemap.cellBounds.size.x*tilemap.cellBounds.size.y;
        tiles = new holder[amount];
        for (int i = 0; i < amount; i++)
        {
            Enumerator.MoveNext();
            if(tilemap.GetTile(Enumerator.Current)!=null)
            {
                Debug.Log(tilemap.GetTransformMatrix(Enumerator.Current));
                Tile _tile = tilemap.GetTile<Tile>(Enumerator.Current);
                //tiles[i] = tilemap.GetTile<Tile>(Enumerator.Current);

                tiles[i] = new holder(_tile,tilemap.GetTransformMatrix(Enumerator.Current));
            }
        }
        foreach (var tile in tiles)
        {
            if(tile!=null)
            {
                Debug.Log(tile.tile.transform);
            }
        }
    }
    public class holder
    {
        private Tile _tile;
        private Matrix4x4 _transform;
        // public Tile tile { get => new Tile(){
        //     colliderType=_tile.colliderType,
        //     color=_tile.color,
        //     flags=_tile.flags,
        //     gameObject=_tile.gameObject,
        //     hideFlags=_tile.hideFlags,
        //     name=_tile.name,
        //     sprite=_tile.sprite,
        //     transform=_transform
        //     };}
        public Tile tile {get{_tile.transform=_transform;return _tile;}}
        public holder(Tile tile,Matrix4x4 transform)
        {
            this._tile=tile;
            this._transform = transform;
        }
        
    }
}
