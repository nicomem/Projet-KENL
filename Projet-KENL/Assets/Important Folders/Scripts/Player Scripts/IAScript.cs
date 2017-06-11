using UnityEngine;

public class IAScript : IAAbstract
{
    // See IAAbstract to see variables

    protected override void GetInputsIA()
    {
        float dxSigned = transform.position.x - ennemyScript.transform.position.x,
            dySigned = transform.position.y - ennemyScript.transform.position.y,
            dx = Mathf.Abs(dxSigned),
            dy = Mathf.Abs(dySigned);

        if (dy < 4f) {
            if (dx < 3f)
                attackSelected = 0;
            else
                attackSelected = 1;
        }
        else attackSelected = -1;

        if (dxSigned > 1.5f)
            xInput = -1f;
        else if (dxSigned < -1.5f)
            xInput = 1f;
        else
            xInput = 0f;

        if (dx < 2 && dySigned > 5 && ennemyCharaControl.isGrounded)
            xInput = -1.0f;

        // Lorsque IA touchée
        if (playerScript.IsHit()) {
            if (dxSigned > 0)
                xInput = 1.0f;
            else
                xInput = -1.0f;
        }

        GameObject[] projectile = GameObject.FindGameObjectsWithTag("Bullet");
        blockPressed = CheckMin(projectile);

        // Le time évite le bug au début ou le perso saute sans raison
        if ((!ennemyCharaControl.isGrounded && Time.deltaTime > 1.0f)
            || (dySigned < -0.2f))
            jumpButtonPressed = true;
        else
            jumpButtonPressed = false;

        if (playerScript.horizontalSpeed < 0 && dx > 2.5f)
            xInput *= -1f;
    }

    private bool CheckMin(GameObject[] liste)
    {
        for (int i = 0; i < liste.GetLength(0); i++) {
            if (Mathf.Abs(liste[i].transform.position.x - transform.position.x) < 6f
            && Mathf.Abs(liste[i].transform.position.y - transform.position.y) < 4f
            && liste[i].GetComponent<BulletScript>().player.GetInstanceID()
                    != gameObject.GetInstanceID())
                return true;
        }

        return false;
    }
}