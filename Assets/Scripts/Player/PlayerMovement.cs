using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    //Movement speed of the player
    public float moveSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorizontalVector;
    [HideInInspector]
    public float lastVerticalVector;
    [HideInInspector]
    public Vector2 MoveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1f, 0f);
    }

    void Update()
    {
        InputManagement(); 
    }

    void FixedUpdate()
    {
        Move();
    }
    void InputManagement()
    {
        Vector2 input = Keyboard.current != null 
            ? new Vector2(
                (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1 : 0) - 
                (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1 : 0),
                (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1 : 0) - 
                (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1 : 0))
            : Vector2.zero;

        MoveDir = input.normalized;

        if (MoveDir.x != 0)
        {
            lastHorizontalVector = MoveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);
        }
        if (MoveDir.y != 0)
        {
            lastVerticalVector = MoveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector);
        }
        
        if (MoveDir.x != 0 && MoveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(MoveDir.x * moveSpeed, MoveDir.y * moveSpeed);
    }
}
