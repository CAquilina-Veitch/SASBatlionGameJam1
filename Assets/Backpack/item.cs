using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
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
            ////////////////////////sR.sprite = itemSets[(int)value - 1].zero.sprite;
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

        linked.Add(this);

        int i = 0;
        while(i > -1)
        {
            List<item> adjSimilar = bp.GetAdjacentOfType(linked[i].coord, linked[i].type);
            bool isNew = false;
            foreach(item item in adjSimilar)
            {
                if (!linked.Contains(item))
                {
                    isNew = true;
                    linked.Add(item);
                    
                }
            }
            i++;
            if (!isNew && i == linked.Count)
            {
                i = -1;
            }

        }
        return linked;
    }

    public ItemSet currentItemSet;


    public void UpdateSprite()
    {
        List<Vector2Int> directions = bp.GetAdjacentDirOfType(coord,type);

        
    }
    public bool[] UpdateSprite(bool[] needsUniquePiece)
    {
        List<Vector2Int> directions = bp.GetAdjacentDirOfType(coord, type);
        int i = 0;
        bool[] _toOut = needsUniquePiece;
        transform.rotation = Quaternion.identity;
        sR.flipY = false;
        sR.flipX = false;

        ItemSet currentSet = bp.itemSets[(int)type - 1];///////////



        switch (directions.Count)
        {
            case 0:
                sR.sprite = currentSet.zero.sprite;
                break;
            case 1:

                //sprite

                if (needsUniquePiece[0])
                {
                    _toOut[0] = false;
                    sR.sprite = currentSet.oneUnique.sprite;
                }
                else
                {
                    sR.sprite = currentSet.one.sprite;
                }

                //rotation

                if (directions[0].y != 0)
                {
                    sR.flipY = directions[0].y > 0;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90 * directions[0].x);
                }

                if (directions[0].y != 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0,  90 - (90 * directions[0].y));
                }



                break;
            case 2:

                //sprite

                if (directions.Total().x + directions.Total().y == 0)
                {
                    //straight sprite
                    if (needsUniquePiece[1])
                    {
                        _toOut[1] = false;
                        sR.sprite = currentSet.twoUnique.sprite;
                    }
                    else
                    {
                        sR.sprite = currentSet.two.sprite;
                    }

                    //rotation
                    if (MathF.Abs(directions[0].x) == 1)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 90);
                    }


                }
                else
                {
                    //elbow
                    if (needsUniquePiece[2])
                    {
                        _toOut[2] = false;
                        sR.sprite = currentSet.twoElbowUnique.sprite;
                    }
                    else
                    {
                        sR.sprite = currentSet.twoElbow.sprite;
                    }

                    //rotation
                    sR.flipX = sR.flipY = directions.Total().x > 0;
                    sR.flipY = directions.Total().y > 0;
                }
                break;
            case 3:
                if (needsUniquePiece[3])
                {
                    _toOut[3] = false;
                    sR.sprite = currentSet.threeUnique.sprite;
                }
                else
                {
                    sR.sprite = currentSet.three.sprite;
                }

                transform.rotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg*Mathf.Atan2(directions.Total().y, directions.Total().x)));                
                break;
            case 4:
                if (needsUniquePiece[4])
                {
                    _toOut[4] = false;
                    sR.sprite = currentSet.fourUnique.sprite;
                }
                else
                {
                    sR.sprite = currentSet.four.sprite;
                }
                break;
            default:
                sR.sprite = currentSet.zero.sprite;
                break;
        }
        return _toOut;
    }

}
[Serializable]
public struct ItemSet
{
    public ItemType type;

    public img zero;
    public img one;
    public img oneUnique;
    public img two;
    public img twoUnique;
    public img twoElbow;
    public img twoElbowUnique;
    public img three;
    public img threeUnique;
    public img four;
    public img fourUnique;
}
[Serializable]
public struct img
{
    public Sprite sprite;
    public int id;
    public int numOfConnections;
}