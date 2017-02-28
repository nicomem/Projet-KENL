using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsScript : MonoBehaviour {

    public Animator anim;
    public bool isRunning = false;
    public bool isAttacking = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void do_animations(float inputX, float InvulnerableTimer)
    {
        if (isAttacking)
        {
            anim.Play("attack05", -1);
        } else if (isRunning)
        {
            anim.Play("run00", -1);
        } else if (InvulnerableTimer > 0)
        {
            anim.Play("gethit01", -1);
        } else
        {
            anim.Play("idle02", -1);
        }
    }
}
