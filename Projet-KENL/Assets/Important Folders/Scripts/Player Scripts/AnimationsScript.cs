using UnityEngine;
using UnityEngine.Networking;

public class AnimationsScript : NetworkBehaviour
{
    private Animator anim;
    private Animation anim2;
    private bool isNetworked;

    private SoundManager soundManager;
    private PlayerScript playerScript;

    [HideInInspector] public bool isRunning, isAttacking, isHit, isBlocking;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim2 = GetComponent<Animation>();

        isNetworked = GameObject.Find("Network Manager") != null;
        playerScript = GetComponent<PlayerScript>();

        GameObject soundManagerGO = GameObject.Find("Sound Manager");
        if (soundManagerGO != null)
            soundManager = soundManagerGO.GetComponent<SoundManager>();
    }

    private void Update()
    {
        DoAnimations();
        DoSounds();
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
        if (playerScript == null) {
            AnimationsSatela();
            return;
        }

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

            case "Satela":
                AnimationsSatela();
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

    private void AnimationsSatela()
    {
        if (isBlocking) { anim2.Play("Idle"); return; }
        if (isHit) { anim2.Play("Receibe Damage"); return; }
        if (isAttacking) { anim2.Play("Dagger Strike 2"); return; }
        if (isRunning) { anim2.Play("Run"); return; }

        anim2.Play("Idle");
    }
}
