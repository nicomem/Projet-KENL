using UnityEngine;

public class IAEdwinScript : IAAbstract
{
    // See IAAbstract to see variables

    protected override void GetInputsIA()
    {
        float dxSigned = transform.position.x - ennemyScript.transform.position.x,
            dySigned = transform.position.y - ennemyScript.transform.position.y,
            dx = Mathf.Abs(dxSigned),
            dy = Mathf.Abs(dxSigned);

        if (dx < 3f && dy < 3f)
            attackSelected = 0;
        else
            attackSelected = -1;

        if (dxSigned > 2f)
            xInput = -1f;
        else if (dxSigned < -2f)
            xInput = 1f;
        else
            xInput = 0f;

        // Lorsque IA touchée
        if (playerScript.IsHit()) {
            if (dxSigned > 0)
                xInput = 1.0f;
            else
                xInput = -1.0f;
        }

        // Le time évite le bug au début ou le perso saute sans raison
        if ((!ennemyCharaControl.isGrounded && Time.deltaTime > 1.0f)
            || (dySigned < -0.2f))
            jumpButtonPressed = true;
        else
            jumpButtonPressed = false;

        if (playerScript.horizontalSpeed < 0)
            xInput *= -1f;
    }
}
