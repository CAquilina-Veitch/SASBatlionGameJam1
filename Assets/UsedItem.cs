using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedItem : MonoBehaviour
{
    public Backpack bP;
    public void CreateItemSet(List<item> pieces)
    {
        Vector2Int start= pieces[0].coord;
        foreach (item i in pieces)
        {
            bP.activeItemDictionary.Remove(i.coord);
            i.transform.parent = transform;
            i.transform.localPosition = new Vector3(i.coord.x - start.x, i.coord.y - start.y);
        }
    }



}