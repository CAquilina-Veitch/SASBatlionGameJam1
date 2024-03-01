using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Empty, Sword, Shield, Potion, Gold}
public class item : MonoBehaviour
{
    public ItemType type;
    public Vector2Int coord;

    public List<item> merged; 


}
