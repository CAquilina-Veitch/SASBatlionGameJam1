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
        List<Vector2Int> checkedTiles = new List<Vector2Int>();


        checkedTiles.Add(coord);
        linked.Add(this);

        List<item> adjSimilar = bp.GetAdjacentOfType(coord,type);
        foreach(item sim in adjSimilar)
        {
            checkedTiles.Add(sim.coord);
            linked.Add(sim);
            List<item> adjSim2 = bp.GetAdjacentOfType(sim.coord, type);
            foreach(item sim2 in adjSim2)
            {
                if (!checkedTiles.Contains(sim.coord))
                {
                    checkedTiles.Add(sim2.coord);
                    linked.Add(sim2);
                }
            }
        }

/*



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
*/

        return linked;
    }


}
