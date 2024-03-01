using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilDog : MonoBehaviour
{
    public Vector2Int coord;
    public Backpack bp;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            TryToMoveToPosFrom(coord + Vector2Int.up, coord);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryToMoveToPosFrom(coord + Vector2Int.left, coord);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryToMoveToPosFrom(coord + Vector2Int.down, coord);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TryToMoveToPosFrom(coord + Vector2Int.right, coord);
        }

    }
    public void TryToMoveToPosFrom(Vector2Int to, Vector2Int from)
    {
        if (to.x >= 0 && to.x < bp.proportions.x && to.y >= 0 && to.y < bp.proportions.y)
        {
            //not out of backpack
            if(bp.activeItemDictionary.TryGetValue(to, out item pushTarget))
            {
                //push item pushing
                bp.TryPushItem(pushTarget,to-from);
            }
            else
            {
                coord = to;
                transform.position = to.ToPos();
            }
        }
    }

}
