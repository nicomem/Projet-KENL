using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTriggerScript : MonoBehaviour
{
    private PlayerScript player;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerScript>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Plateform")) {
            player.SetVerticalVelocity(0);
        }
    }
}
