using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VertCamera : MonoBehaviour
{
    public Transform player;
    Vector3 startPos;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(startPos.x, Mathf.Max(transform.position.y,player.position.y), startPos.z),0.01f);
    }


    void Start()
    {
        transform.position = new Vector3(startPos.x,player.position.y,startPos.z);
    }

}
