using UnityEngine;
using System.Collections;
using System;

public class DeadSpriteScript : SpriteBase
{
    
    // Use this for initialization
    void Start () {
	}

    public override void Jump(bool up)
    {
    }

    public override void  MoveHorizontal(float input)
    {
    }
    public override void StandingAnimation(bool standing)
    {
    }

    // Update is called once per frame
    void Update () {
        /*
        if(transform.position.x > 0)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.left;
            GetComponent<SpriteRenderer>().flipX = true;
        }*/
	}

}
