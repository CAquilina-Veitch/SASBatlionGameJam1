using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            transform.position = coord.ToPos();
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
        List<item> toCheck = new List<item>();
        List<item> checkedTiles = new List<item>();


        linked.Add(this);
        toCheck.Add(this);
        checkedTiles.Add(this);

        while (toCheck.Count > 0)
        {
            List<item> adj = bp.GetAdjacent(toCheck[0].coord);
            foreach (item item in adj)
            {
                if (checkedTiles.Contains(item))
                {
                    adj.Remove(item);
                }
                else
                {
                    if (toCheck[0].type == type)
                    {
                        linked.Add(item);
                        toCheck.Add(item);
                        checkedTiles.Add(item);

                    }
                }
            }
        }


        return linked;
    }


}
