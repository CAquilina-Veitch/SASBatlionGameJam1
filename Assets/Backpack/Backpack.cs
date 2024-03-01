using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public LilDog player;

    public List<item> spawned = new List<item>();

    public Dictionary<Vector2Int, item> activeItemDictionary = new Dictionary<Vector2Int, item>();

    public List<item> previewSet = new List<item>();



    public GameObject itemPrefab;


    public Vector2Int proportions = new Vector2Int(7,5);


    private void Start()
    {
        player.coord = new Vector2Int(4, 3);
        GeneratePreviewSet();
        MoveRows();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            MoveRows();
        }
    }

    public void MoveRows()
    {
        DropPreviewSet();
        GeneratePreviewSet();
        //physics all the rows;
    }

    public void DropPreviewSet()
    {
        if (previewSet.Count == 0) 
        {
            return;
        }
        foreach(item prev in previewSet)
        {
            if (prev != null)
            {
                //check for vacant cells
                if (activeItemDictionary.TryGetValue(prev.coord, out item filled))
                {
                    //debug if found a tile
                    Debug.LogError($" Cant drop because {filled} is already there");
                }
                else
                {
                    //empty; can drop
                    DropPreview(prev);
                }
            }
        }
    }

    public void GeneratePreviewSet()
    {
        List<ItemType> itemSpawnAttempts = GenerateItemPreviewTypes();

        List<Vector2Int> emptys = GetEmptyCells();

        previewSet.Clear();

        foreach (ItemType i in itemSpawnAttempts)
        {
            if (emptys.Count == 0)
            {
                return;
            }
            Vector2Int Coord = emptys[Random.Range(0, emptys.Count)];
            item tempItem = PreviewItemAtCoord(i, Coord);
            previewSet.Add(tempItem);
            emptys.Remove(Coord);

        }
    }

    item PreviewItemAtCoord(ItemType i, Vector2Int coord)
    {
        GameObject obj = Instantiate(itemPrefab,coord.ToPos(),Quaternion.identity, transform);

        item temp = obj.GetComponent<item>();
        temp.bp = this;
        temp.coord = coord;
        temp.isPreview = true;
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

    public void TryPushItem(item item, Vector2Int dir)
    {
        List<item> pushTargets = item.GetLinked();
        bool canPush = true;
        foreach(item piece in pushTargets)
        {
            if (activeItemDictionary.TryGetValue(piece.coord + dir,out item obstacle))
            {
                if(obstacle.type != item.type)
                {
                    canPush = false; 
                    break;
                }
            }
        }
        if( canPush)
        {
            foreach(item piece in pushTargets)
            {
                MoveItem(dir, piece);
            }
        }
    }


    public void MoveItem(Vector2Int dir, item item)
    {
        activeItemDictionary.Remove(item.coord);
        item.coord += dir;
        activeItemDictionary.Add(item.coord, item);
    }

    public void DropPreview(item i)
    {
        activeItemDictionary[i.coord] = i;
        i.isPreview = false;
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
        temp.Remove(player.coord);
        return temp;
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

    public List<item> GetAdjacent(Vector2Int coord)
    {
        List<item> temp = new List<item>();

        foreach (Vector2Int dir in Functions.Dirs)
        {
            if(activeItemDictionary.TryGetValue(coord+dir,out item i))
            {
                temp.Add(i);
            }
        }
        return temp;
    }


}
