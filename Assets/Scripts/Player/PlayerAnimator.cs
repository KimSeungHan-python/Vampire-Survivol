using UnityEngine;
using System.Collections.Generic;
using System.Collections;   
public class PlayerAnimator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator am;
    public PlayerMovement pm;
    SpriteRenderer sr;

    void Start()
    {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
            if(pm.MoveDir.x != 0 || pm.MoveDir.y != 0)
            {
                am.SetBool("Move", true);
                
                SpriteDirectionChecker();
            }
            else
            {
                am.SetBool("Move", false);
            }
    }

    void SpriteDirectionChecker()
    {
        if(pm.lastHorizontalVector > 0)
        {
            sr.flipX = false;
        }
        else if(pm.lastHorizontalVector < 0)
        {
            sr.flipX = true;
        }
    }
}
