using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{

    public List<item> spawned = new List<item>();

    public Dictionary<Vector2Int, item> items;

    public GameObject itemPrefab;


    public Vector2Int proportions = new Vector2Int(7,5);



    public void SpawnNewTopRow()
    {
        List<item> nextRow = new List<item>();

        int numItems = Random.Range(2, 4);

        for(int i = 0; i < numItems; i++)
        {
            GameObject temp = Instantiate(itemPrefab, SpawnPosition,,transform);
        }


    }

    public static Vector2 SpawnPosition(int column)
    {
        return new Vector2(column - 3, 3);
    }

    public  List<ItemType> itemRow()
    {
        List<ItemType> temp = new List<ItemType>();
        for(int i =0;i< proportions.x;i++)
        return
    } 
}
