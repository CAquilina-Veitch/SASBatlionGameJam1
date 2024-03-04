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
            currentItemSet = bp.itemSets[(int)value-1];
            sR.sprite = currentItemSet.zero.sprite;
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

        bool[] _toOut = needsUniquePiece;

        transform.rotation = Quaternion.identity;
        sR.flipY = false;
        sR.flipX = false;

        switch (directions.Count)
        {
            case 0:
                sR.sprite = currentItemSet.zero.sprite;
                break;
            case 1:

                //sprite
                if (currentItemSet.one.sprite == null)
                {
                    sR.sprite = currentItemSet.zero.sprite;
                    break;
                }
                if (needsUniquePiece[0])
                {
                    _toOut[0] = false;
                    sR.sprite = currentItemSet.oneUnique.sprite;
                }
                else
                {
                    sR.sprite = currentItemSet.one.sprite;
                }

                //rotation

                if (directions[0].y != 0)
                {
                    sR.flipY = directions[0].y > 0;
                    sR.flipX = directions[0].y > 0;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90 * directions[0].x);
                }

                break;
            case 2:
                if (currentItemSet.two.sprite == null)
                {
                    sR.sprite = currentItemSet.zero.sprite;
                    break;
                }
                //sprite
                if ( MathF.Abs( directions.Total().x) + MathF.Abs(directions.Total().y) == 0)
                {
                    //straight sprite
                    if (needsUniquePiece[1])
                    {
                        _toOut[1] = false;
                        sR.sprite = currentItemSet.twoUnique.sprite;
                    }
                    else
                    {
                        sR.sprite = currentItemSet.two.sprite;
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
                        sR.sprite = currentItemSet.twoElbowUnique.sprite;
                    }
                    else
                    {
                        sR.sprite = currentItemSet.twoElbow.sprite;
                    }

                    //rotation
                    sR.flipX = sR.flipY = directions.Total().x > 0;
                    sR.flipY = directions.Total().y > 0;
                }
                break;
            case 3:
                if (currentItemSet.three.sprite == null)
                {
                    sR.sprite = currentItemSet.zero.sprite;
                    break;
                }
                if (needsUniquePiece[3])
                {
                    _toOut[3] = false;
                    sR.sprite = currentItemSet.threeUnique.sprite;
                }
                else
                {
                    sR.sprite = currentItemSet.three.sprite;
                }

                transform.rotation = Quaternion.Euler(0, 0, 90 + (Mathf.Rad2Deg * Mathf.Atan2(directions.Total().y, directions.Total().x)));
                break;
            case 4:
                if (currentItemSet.four.sprite == null)
                {
                    sR.sprite = currentItemSet.zero.sprite;
                    break;
                }
                if (needsUniquePiece[4])
                {
                    _toOut[4] = false;
                    sR.sprite = currentItemSet.fourUnique.sprite;
                }
                else
                {
                    sR.sprite = currentItemSet.four.sprite;
                }
                break;
            default:
                sR.sprite = currentItemSet.zero.sprite;
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
}