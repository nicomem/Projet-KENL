using UnityEngine;

public class IAScript : IAAbstract
{
    // See IAAbstract to see variables

    protected override void GetInputsIA()
    {
        float dxSigned = transform.position.x - ennemyScript.transform.position.x,
            dySigned = transform.position.y - ennemyScript.transform.position.y,
            dx = Mathf.Abs(dxSigned),
            dy = Mathf.Abs(dxSigned);

        if (dx < 2 && dy < 1)
            attackSelected = 0;
        else
            attackSelected = -1;

        if (dxSigned > 2f)
            xInput = -1f;
        else if (dxSigned < -2f)
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
    }

    private bool CheckMin(GameObject[] liste)
    {
        for (int i = 0; i < liste.GetLength(0); i++) {
            if (Mathf.Abs(liste[i].transform.position.x - transform.position.x) < 3
            && Mathf.Abs(liste[i].transform.position.y - transform.position.y) < 3
            && liste[i].GetComponent<BulletScript>().player != gameObject)
                return true;
        }

        return false;
    }
}
