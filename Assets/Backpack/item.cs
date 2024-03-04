using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public enum ItemType { Empty, Sword, Shield, Potion, Gold}
public class item : MonoBehaviour
{
    public ItemType type 
    {
        get 
        {  
            return _type; 
        }
        set 
        {
            sR.sprite = tileSprites[(int)value-1].sprites[0];
            _type = value; 
        }
    }
    private ItemType _type;

    public Vector2Int _coord;
    public Vector2Int coord
    {
        get 
        { 
            return _coord; 
        }
        set 
        {
            //transform.position = coord.ToPos();
            _coord = value; 
        } 
    }

    private bool _isPreview;
    public SpriteRenderer sR;
    public bool isPreview
    {
        get
        {
            return _isPreview;
        }
        set 
        { 
            sR.color = value? sR.color.toHalf():sR.color.toFull();
            _isPreview = value; 
        }
    }

    public Backpack bp;

    public List<item> GetLinked()
    {
        List<item> linked = new List<item>();

        linked.Add(this);

        int i = 0;
        while(i > -1)
        {
            List<item> adjSimilar = bp.GetAdjacentOfType(linked[i].coord, linked[i].type);
            bool isNew = false;
            foreach(item item in adjSimilar)
            {
                if (!linked.Contains(item))
                {
                    isNew = true;
                    linked.Add(item);
                    
                }
            }
            i++;
            if (!isNew && i == linked.Count)
            {
                i = -1;
            }

        }
        return linked;
    }


    public List<TileSet> tileSprites = new List<TileSet>();


    public void UpdateSprite()
    {
        List<Vector2Int> directions = bp.GetAdjacentDirOfType(coord,type);

        
    }
    public bool UpdateSprite(bool needsUniquePiece)
    {
        List<Vector2Int> directions = bp.GetAdjacentDirOfType(coord, type);
        int i = 0;
        bool _toOut = needsUniquePiece;
        transform.rotation = Quaternion.identity;
        sR.flipY = false;
        sR.flipX = false;
        switch (directions.Count)
        {
            case 0:
                i = 0;
                break;
            case 1:
                if (needsUniquePiece)
                {
                    i = 3;
                    _toOut = false;
                }
                else
                {
                    i = 1;
                }
                transform.rotation = Quaternion.Euler(0, 0, -90 * directions[0].x);
                if (directions[0].y != 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0,  90 - (90 * directions[0].y));
                }
                break;
            case 2:
                sR.flipX = directions.Total().x > 0;
                if (directions[0] + directions[1] == Vector2Int.zero)
                {
                    i = 2;
                    transform.rotation = directions[0].x != 0 ? Quaternion.Euler(0, 0, 90) : Quaternion.identity;
                }
                else
                {
                    i = 5;
                    sR.flipY = directions.Total().y > 0;
                }
                break;
            case 3:
                i = 4;
                if (directions.Total().y == 0)
                {
                    sR.flipX = directions.Total().x > 0;
                }
                else
                {
                    transform.rotation = directions.Total().y>0? Quaternion.Euler(0, 0, -90): Quaternion.Euler(0, 0, 90); 
                }
                
                break;
            default:
                i = 6;
                break;
        }
        if (i == 1)
        {
            sR.flipY = true;
        }
        sR.sprite = tileSprites[(int)type - 1].sprites[i];
        return _toOut;
    }

}
[Serializable]
public struct TileSet
{
    public ItemType itemType;
    public Sprite[] sprites;
}