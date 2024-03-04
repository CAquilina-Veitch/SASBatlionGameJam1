using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [Header("Dependencies")]

    public UsedItem usedItem;


    public LilDog player;

    [HideInInspector] public List<item> spawned = new List<item>();

    [HideInInspector] public Dictionary<Vector2Int, item> activeItemDictionary = new Dictionary<Vector2Int, item>();

    [HideInInspector] public List<item> previewSet = new List<item>();



    public GameObject itemPrefab;

    [Space(40)]
    [Header("Variables to change")]
    public Vector2Int proportions = new Vector2Int(9,9);
    public List<ItemSet> itemSets = new List<ItemSet>();



    private void Start()
    {
        player.coord = new Vector2Int(0, 0);
        GeneratePreviewSet();
        TileTurn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TileTurn();
        }
    }

    public void TileTurn()
    {
        DropPreviewSet();
        GeneratePreviewSet();
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
        foreach(item prev in previewSet)
        {
            UpdateTouchingItemSprites(prev);
        }
    }


    public void UpdateTouchingItemSprites(item target)
    {
        List<item> pushTargets = target.GetLinked();
        ItemSet temp = itemSets[(int)target.type - 1];
        bool[] needsUniquePiece =
        {
            temp.oneUnique.sprite!=null,
            temp.twoUnique.sprite!=null,
            temp.twoElbowUnique.sprite!=null,
            temp.threeUnique.sprite!=null,
            temp.fourUnique.sprite!=null
        };
        foreach (item pt in pushTargets)
        {
            needsUniquePiece = pt.UpdateSprite(needsUniquePiece);
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
        temp.type = i;
        temp.coord = coord;
        temp.isPreview = true;
        return temp;
    }


    public void TryPushItem(item item, Vector2Int dir)
    {
        Debug.Log("trypush");
        List<item> pushTargets = item.GetLinked();

        bool canPush = true;
        foreach (item piece in pushTargets)
        {
            Vector2Int to = piece.coord + dir;

            if(!(to.x >= 0 && to.x < proportions.x && to.y >= 0 && to.y < proportions.y))
            {
                if (to.x >= proportions.x)
                {
                    Debug.LogWarning("WE HAVE A PIECE MADE");
                    PlayItemSet(pushTargets);
                }
                canPush = false;
                break;
            }
            if (activeItemDictionary.TryGetValue(to, out item obstacle))
            {
                if (obstacle.type != item.type)
                {
                    canPush = false;
                    break;
                }
            }
        }
        if (canPush)
        {
            PushWhole(dir,pushTargets);
        }
    }
    public void PlayItemSet(List<item> itemSet)
    {
        usedItem.CreateItemSet(itemSet);
    }



    public void PushWhole(Vector2Int dir, List<item> pushTargets)
    {
        foreach(item tar in pushTargets)
        {
            activeItemDictionary.Remove(tar.coord);
        }
        foreach(item tar in pushTargets)
        {
            tar.coord += dir;
            tar.transform.position = tar.coord.ToPos();
            activeItemDictionary.Add(tar.coord, tar);
        }
        UpdateTouchingItemSprites(pushTargets[0]);
    }

    public void MoveItem(Vector2Int dir, item item)
    {
        activeItemDictionary.Remove(item.coord);
        Debug.LogWarning($"from {item.coord} to {item.coord + dir}");

        item.coord += dir;
        item.transform.position = item.coord.ToPos();
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
            type = Random.Range(1, 2);
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
    public List<item> GetAdjacentOfType(Vector2Int coord,ItemType type)
    {
        List<item> temp = new List<item>();

        foreach (Vector2Int dir in Functions.Dirs)
        {
            if(activeItemDictionary.TryGetValue(coord+dir,out item i))
            {
                if (i.type == type)
                {
                    temp.Add(i);
                }
            }
        }
        return temp;
    }
    public List<Vector2Int> GetAdjacentDirOfType(Vector2Int coord, ItemType type)
    {
        List<Vector2Int> temp = new List<Vector2Int>();

        foreach (Vector2Int dir in Functions.Dirs)
        {
            if (activeItemDictionary.TryGetValue(coord + dir, out item i))
            {
                if (i.type == type)
                {
                    temp.Add(dir);
                }
            }
        }
        return temp;
    }


}
