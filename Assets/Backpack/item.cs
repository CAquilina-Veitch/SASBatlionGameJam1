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

        int i = 0;
        switch (directions.Count)
        {
            case 0:
                i = 0;
                break;
            case 1:
                i = 1;
                // 1 or 3
                break;
            case 2:
                i = 2;
                //2 or 5
                break;
            case 3:
                i = 4;
                break;
            default:
                i = 6;
                break;
        }
        sR.sprite = tileSprites[(int)type - 1].sprites[i];
    }


}
[Serializable]
public struct TileSet
{
    public ItemType itemType;
    public Sprite[] sprites;
}