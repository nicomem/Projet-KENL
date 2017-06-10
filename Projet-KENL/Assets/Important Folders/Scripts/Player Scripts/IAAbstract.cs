using System.Collections.Generic;
using UnityEngine;

public abstract class IAAbstract : MonoBehaviour
{
    protected float xInput;
    protected bool jumpButtonPressed;
    protected int attackSelected;
    protected bool blockPressed;

    protected PlayerScript playerScript;
    protected CharacterController charaControl;

    protected PlayerScript ennemyScript;
    protected CharacterController ennemyCharaControl;

    protected MapInfosScript mapInfos;
    protected List<PlayerScript> ennemyScriptList;

    // Use this for initialization
    protected void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        charaControl = GetComponent<CharacterController>();
        mapInfos = GameObject.Find("Map Infos").GetComponent<MapInfosScript>();

        ennemyScriptList = new List<PlayerScript>(mapInfos.ListPlayerScripts);
        ennemyScriptList.Remove(playerScript); // We are not our own ennemy
    }

    protected void UpdateEnnemy()
    {
        // Get a new ennemy if current is ko

        if (ennemyScript != null && !ennemyScript.isKO)
            return;

        // Remove all ko
        ennemyScriptList.RemoveAll(script => script.isKO);
        
        // Get the ennemy (randomly)
        System.Random rand = new System.Random();
        ennemyScript = ennemyScriptList[rand.Next(ennemyScriptList.Count)];

        ennemyCharaControl = ennemyScript.GetComponent<CharacterController>();
    }

    protected void Update()
    {
        // If end
        if (ennemyScriptList.Count == 0)
            return;

        // Update the ennemy
        UpdateEnnemy();

        // We move the IA
        GetInputsIA();

        // Function for moving the player with input (!= IA)
        MovementPlayer();
    }

    protected abstract void GetInputsIA();

    protected void MovementPlayer()
    {
        /* To move the player with input */

        playerScript.Movements(xInput, jumpButtonPressed, attackSelected,
            blockPressed);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
