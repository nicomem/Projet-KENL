using UnityEngine;

public class IATrainingScript : IAAbstract
{
    // See IAAbstract to see variables

    protected override void GetInputsIA()
    {
        if (ennemyScript.transform.position.x < transform.position.x - 2)
            xInput = -1.0f;
        else if (ennemyScript.transform.position.x > transform.position.x + 2)
            xInput = 1.0f;
        else
            xInput = 0f;


        // Lorsque IA touchée
        if (playerScript.IsHit()) {
            if (ennemyScript.transform.position.x < transform.position.x)
                xInput = 1.0f;
            else
                xInput = -1.0f;
        }

        // Le time évite le bug au début ou le perso saute sans raison
        if ((!ennemyCharaControl.isGrounded && Time.deltaTime > 1.0f)
            || (ennemyCharaControl.transform.position.y - transform.position.y > 0.2f))
            jumpButtonPressed = true;
        else
            jumpButtonPressed = false;
    }
}
