using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Backpack : MonoBehaviour
{

    public List<item> spawned = new List<item>();

    public Dictionary<Vector2Int, item> activeItemDictionary;



    public GameObject itemPrefab;


    public Vector2Int proportions = new Vector2Int(7,5);


    private void Start()
    {
        MoveRows();
    }

    public item[] previewRow = new item[7];


    public void MoveRows()
    {
        DropPreviewRow();
        GeneratePreviewRow();
        //physics all the rows;
    }
    public void DropPreviewRow()
    {
        for (int i = 0; i < previewRow.Length; i++)
        {
            if (previewRow[i] != null)
            {
                //check for vacant cells
                if (activeItemDictionary.TryGetValue(new Vector2Int(previewRow[i].coord.x, proportions.y), out item filled))
                {
                    //debug if found a tile
                }
                else
                {
                    //empty; can drop
                    TryMoveItem();
                }
            }
        }
    }


    public List<Vector2Int> GetEmptyCells()
    {
        List<Vector2Int> temp = new List<Vector2Int>();
        for (int x = 0; x < proportions.x; x++)
        {
            for (int y = 0; y < proportions.y; y++)
            {
                Vector2Int coord = new Vector2Int(x, y);
                if (!activeItemDictionary.ContainsKey(coord))
                {
                    temp.Add(coord);
                }
            }
        }
        return temp;
    }

    public void GeneratePreviewRow()
    {

        List<ItemType> itemSpawnAttempts = GenerateItemPreviewTypes();

        List<Vector2Int> emptys = GetEmptyCells();

        foreach (ItemType i in itemSpawnAttempts)
        {
            Vector2Int Coord = emptys[Random.Range(0, emptys.Count)];
            PreviewItemAtCoord(i,Coord);


        }



        for(int i = 0; i < proportions.x; i++)
        {
            if (itemSpawnAttempt[i] != ItemType.Empty)
            {
                GameObject temp = Instantiate(itemPrefab, SpawnPosition(i),Quaternion.identity, transform);
                previewRow[i] = temp.GetComponent<item>();
                previewRow[i].coord = new Vector2Int(i, -1);
            }

        }
    }

    item PreviewItemAtCoord(ItemType i, Vector2Int coord)
    {
        item temp = Instantiate(itemPrefab,coord.ToPos(),Quaternion.identity, transform);
        return temp;
    }


    public void TryMoveItem(Vector2Int to, Vector2Int from)
    {
        if(activeItemDictionary.TryGetValue(from, out item fromSpot))
        {
            TryMoveItem(to, fromSpot);
        }
    }
    public void TryMoveItem(Vector2Int to, item from)
    {
        if (activeItemDictionary.TryGetValue(to, out item INTHEWAY))
        {
            Debug.LogError($" THIS MF {INTHEWAY} IS IN THE WAY??!??!");
        }
        else
        {
            MoveItem(to, from);
        }
    }

    public void MoveItem(Vector2Int to, item from)
    {
        Vector2 a;

    }





    public List<ItemType> GenerateItemPreviewTypes()
    {
        int numPieces = Random.Range(2, 10);
        int type = 1;
        List<ItemType> temp = new List<ItemType>();
        for (int i = 0; i < numPieces; i++)
        {
            type = Random.Range(1, 5);
            temp.Add((ItemType)type);
        }
        return temp;
    } 
}
public static class Functions
{
    public static Vector2 ToPos(this Vector2Int coord)
    {
        return new Vector2(coord.x - 3, coord.y - 2);
    }
    public static Vector2Int ToCoord(this Vector2 pos)
    {
        return new Vector2Int((int)pos.x + 3, (int)pos.y + 2);
    }
}