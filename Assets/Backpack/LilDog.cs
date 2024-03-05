using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LilDog : MonoBehaviour
{
    public Vector2Int coord;
    public Backpack bp;
    public KeyCode pullKey;

    private void Start()
    {
        transform.position = coord.ToPos();
    }
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
        Vector2Int dir = to - from;
        if (to.x >= 0 && to.x < bp.proportions.x && to.y >= 0 && to.y < bp.proportions.y)
        {
            //not out of backpack
            if (Input.GetKey(pullKey))
            {
                if (bp.activeItemDictionary.TryGetValue(to, out item pushTarget)) 
                {
                    //item in the way

                    //push item pushing
                    bp.TryPushItem(pushTarget, dir);
                }
                else
                {
                    if (bp.activeItemDictionary.TryGetValue(from-dir, out item pullTarget))
                    {
                        bp.TryPushItem(pullTarget, dir);
                    }

                }

            }
            else
            {
                //pushin
                if (bp.activeItemDictionary.TryGetValue(to, out item pushTarget))
                {
                    //push item pushing
                    bp.TryPushItem(pushTarget, dir);
                }
                else
                {
                    coord = to;
                    transform.position = to.ToPos();
                }
            }





            
        }
    }
    public void PushedMove(Vector2Int dir)
    {
        Vector2Int to = coord + dir;
        coord = to;
        transform.position = to.ToPos();
    }


}
