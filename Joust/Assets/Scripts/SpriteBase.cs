using UnityEngine;
using System.Collections;
using System;

public abstract class SpriteBase : MonoBehaviour
{
    public bool isSecondary = false;
    public float horizontalInput = 0;
    public bool upInput = false;

    public delegate void PlayerDeath();
    public delegate void StandingEnter(bool standing);

    public event PlayerDeath OnBeeDeath;
    public event StandingEnter OnStandingEnter;

    protected void FireOnBeeDeath()
    {
        PlayerDeath handler = OnBeeDeath;
        if (handler != null)
        {
            handler();
        }
    }

    protected void FireOnStandingEnter(bool standing)
    {
        StandingEnter handler = OnStandingEnter;
        if (handler != null)
        {
            handler(standing);
        }
    }

    public abstract void MoveHorizontal(float input);
    public abstract void StandingAnimation(bool standing);
    public abstract void Jump(bool up);
}
