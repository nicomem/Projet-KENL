using UnityEngine;
using UnityEngine.Networking;

public class AnimationsScript : NetworkBehaviour {

    public Animator anim;
    [SyncVar] public bool isRunning = false;
    [SyncVar] public bool isAttacking = false;

    [Command] public void CmdSyncIsRunning(bool b) { isRunning = b; }
    [Command] public void CmdSyncIsAttacking(bool b) { isAttacking = b; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Do_animations(float inputX, float InvulnerableTimer)
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
