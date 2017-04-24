using UnityEngine;
using UnityEngine.Networking;

public class AnimationsScript : NetworkBehaviour
{
    private Animator anim;
    private Animation anim2;
    public bool useAnimator = true;
    private bool isNetworked;

    #region SyncVar: isRunning
    [SyncVar] public bool isRunning = false;
    public void SyncIsRunning(bool b)
    {
        if (isServer || !isNetworked)
            isRunning = b;
        else
            CmdSyncIsRunning(b);
    }
    [Command] private void CmdSyncIsRunning(bool b) { isRunning = b; }
    #endregion

    #region SyncVar: isAttacking
    [SyncVar] public bool isAttacking = false;
    public void SyncIsAttacking(bool b)
    {
        if (isServer || !isNetworked)
            isAttacking = b;
        else
            CmdSyncIsAttacking(b);
    }
    [Command] private void CmdSyncIsAttacking(bool b) { isAttacking = b; }
    #endregion

    #region SyncVar: isHit
    [SyncVar] public bool isHit = false;
    public void SyncIsHit(bool b)
    {
        if (isServer || !isNetworked)
            isHit = b;
        else
            CmdSyncIsHit(b);
    }
    [Command] private void CmdSyncIsHit(bool b) { isHit = b; }
    #endregion

    private void Start()
    {
        if (useAnimator)
            anim = GetComponent<Animator>();
        else
            anim2 = GetComponent<Animation>();

        isNetworked = GameObject.Find("Network Manager") != null;
    }

    public void Do_animations()
    {
        if (useAnimator) {
            if (isAttacking)
                anim.Play("attack05", -1);
            else if (isHit)
                anim.Play("gethit01", -1);
            else if (isRunning)
                anim.Play("run00", -1);
            else
                anim.Play("idle02", -1);
        } else {
            if (isAttacking)
                anim2.Play("attack05");
            else if (isHit)
                anim2.Play("gethit01");
            else if (isRunning)
                anim2.Play("run00");
            else
                anim2.Play("idle02");
        }
    }
}
