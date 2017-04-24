using UnityEngine;
using UnityEngine.Networking;

public class AnimationsScript : NetworkBehaviour
{
    private Animator anim;
    private Animation anim2;
    private bool isNetworked;

    private string persoName;

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
        anim = GetComponent<Animator>();

        isNetworked = GameObject.Find("Network Manager") != null;
        persoName = GetComponent<PlayerScript>().persoName;
    }

    public void Do_animations()
    {
        switch (persoName) {
            case "Gianluigi Conti":
                if (isAttacking)
                    anim.Play("attack05", -1);
                else if (isHit)
                    anim.Play("gethit01", -1);
                else if (isRunning)
                    anim.Play("run00", -1);
                else
                    anim.Play("idle02", -1);
                break;

            case "Antiope":
                if (isAttacking)
                    anim.SetBool("Use", true);
                else {
                    anim.SetBool("Use", false);

                    if (isHit)
                        anim.SetBool("Pain", true);
                    //anim.Play("Pain", -1);
                    else if (isRunning)
                        anim.SetBool("Idling", false);
                    //anim.Play("SSCombatMove", -1);
                    else
                        anim.SetBool("Idling", true);
                    //anim.Play("SSCombatIdle", -1);
                }

                break;

            default:
                Debug.Log("[ERR] Animations : persoName not recognized");
                break;
        }
    }
}
