using UnityEngine;
using UnityEngine.Networking;

public class AnimationsScript : NetworkBehaviour
{
    private Animator anim;
    private bool isNetworked;

    private SoundManager soundManager;
    private PlayerScript playerScript;

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

    #region SyncVar: isBlocking
    [SyncVar] public bool isBlocking = false;
    public void SyncIsBlocking(bool b)
    {
        if (isServer || !isNetworked)
            isBlocking = b;
        else
            CmdSyncIsBlocking(b);
    }
    [Command] private void CmdSyncIsBlocking(bool b) { isBlocking = b; }
    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();

        isNetworked = GameObject.Find("Network Manager") != null;
        playerScript = GetComponent<PlayerScript>();

        GameObject soundManagerGO = GameObject.Find("Sound Manager");
        if (soundManagerGO != null)
            soundManager = soundManagerGO.GetComponent<SoundManager>();
    }

    public void DoSounds()
    {
        if (soundManager == null)
            return;

        if (isAttacking)
            soundManager.DoBruitages("Attack");
    }

    public void DoAnimations()
    {
        switch (playerScript.persoName) {
            case "Gianluigi Conti":
                AnimationsGuianluigi();
                break;

            case "Antiope":
                AnimationsAntiope();
                break;

            case "Vladimir X":
                AnimationsVladimir();
                break;

            default:
                Debug.Log("[ERR] Animations : persoName not recognized");
                break;
        }
    }

    private void AnimationsGuianluigi()
    {
        if (isBlocking) { anim.Play("idle02", -1); return; }
        if (isHit) { anim.Play("gethit01", -1); return; }
        if (isAttacking) { anim.Play("attack05", -1); return; }
        if (isRunning) { anim.Play("run00", -1); return; }

        anim.Play("idle02", -1);
    }

    private void AnimationsAntiope()
    {
        anim.SetBool("Idling", true);
        anim.SetBool("Pain", false);
        anim.SetBool("Use", false);

        if (isBlocking) return; // Idling => true
        if (isHit) { anim.SetBool("Pain", true); return; }
        if (isAttacking) { anim.SetBool("Use", true); return; }
        if (isRunning) { anim.SetBool("Idling", false); return; }
    }

    private void AnimationsVladimir()
    {
        if (isBlocking) { anim.Play("idel", 0); return; }
        if (isHit) { anim.Play("gethit", 0); return; }
        if (isAttacking) { anim.Play("attack_01", 0); return; }
        if (isRunning) { anim.Play("run", 0); return; }

        anim.Play("idel", -1);
    }
}
