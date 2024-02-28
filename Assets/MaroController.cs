using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaroController : MonoBehaviour
{
    public Rigidbody2D rb;

    float xVel;

    public float moveSpeed;
    public float jumpForce;

    private void Update()
    {
        xVel = Mathf.Lerp(rb.velocity.x, Input.GetAxisRaw("Horizontal")*moveSpeed, 0.01f);

        float rJump = ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && rb.velocity.y == 0)?jumpForce:rb.velocity.y;
       
        rb.velocity = new Vector3(xVel, rJump);

    }



}
