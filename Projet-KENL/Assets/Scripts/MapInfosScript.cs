using UnityEngine;

public class MapInfosScript : MonoBehaviour
{
    public GameObject[] listPlayers = new GameObject[4];

    public float xMinLimit, xMaxLimit, yMinLimit, yMaxLimit;
    private float currentY, currentX;

    // Update is called once per frame
    void Update()
    {
        CheckEjected();
    }

    private void CheckEjected()
    {
        for (short i = 0; i < listPlayers.Length; i++) {
            currentY = listPlayers[i].transform.position.y;
            currentX = listPlayers[i].transform.position.x;

            if (currentY < yMinLimit || currentY > yMaxLimit
             || currentX < xMinLimit || currentX > xMaxLimit) {
                /* Animation ejected + remove player */
                /* If --player.lives > 0 => respawn player */
            }
        }
    }
}
