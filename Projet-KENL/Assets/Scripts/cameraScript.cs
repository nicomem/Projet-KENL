using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cameraScript : MonoBehaviour
{
    public float cameraXMin,
        cameraXMax,
        cameraYMin,
        cameraYMax; // Limit the camera to go beyond

    public GameObject[] listPlayers; // The list of players

    private Camera mainCam;
    private float cameraRecul;
    private float CameraFOV;
    private float videoFormat;

    // Use this for initialization
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraRecul = 1.5f;

        CameraFOV = Mathf.Deg2Rad * mainCam.fieldOfView; // radians
        videoFormat = mainCam.aspect; // Video format quotient (ex: 16/9)

    }

    // FixedUpdate is called once per frame (at a constant rate = for physics/movements calculations)
    void FixedUpdate()
    {
        float xMax = cameraXMin, xMin = cameraXMax;
        float yMax = cameraYMin, yMin = cameraYMax;
        float posX, posY;

        foreach (GameObject player in listPlayers)
        {
            posX = player.transform.position.x;
            posY = player.transform.position.y;

            xMax = Mathf.Max(xMax, posX);
            xMin = Mathf.Min(xMin, posX);
            yMax = Mathf.Max(yMax, posY);
            yMin = Mathf.Min(yMin, posY);
        }

        // We do not want to show beyond the area
        xMax = Mathf.Min(xMax, cameraXMax);
        xMin = Mathf.Max(xMin, cameraXMin);
        yMax = Mathf.Min(yMax, cameraYMax);
        yMin = Mathf.Max(yMin, cameraYMin);

        float xCamera = (xMax + xMin) / 2;
        float yCamera = (yMax + yMin) / 2;

        // On cherche la distance (z) entre la caméra et le plan
        // Trigo:
        // zX = (xMax - xMin) * 2 * tan(fov / 2)
        // zY = (yMax - yMin) * 2 * tan(fov / 2)
        // z = max(zX, zY)
        //
        // z = max(xMax - xMin, yMax - yMin) / tan(fov / 2)

        float zCamera = -Mathf.Max(15, Mathf.Max((xMax - xMin) / videoFormat, yMax - yMin) / (2 * Mathf.Tan(CameraFOV / 2)));

        // Les joueurs aux positions extremums sont aux bords de la caméra
        // Pour ajouter de la visibilité, on recule un peu la caméra

        zCamera *= cameraRecul;

        transform.position = new Vector3(xCamera, yCamera, zCamera);
    }
}
