﻿using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera mainCam;
    private GameObject[] listPlayers;
    private float cameraRecul;
    private float CameraFOV;
    private float videoFormat;

    private float cameraXMin, cameraXMax, cameraYMin, cameraYMax;
    private float xMax, xMin, yMax, yMin;
    private float posX, posY;
    private float xCamera, yCamera, zCamera;

    // Use this for initialization
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraRecul = 1.5f;

        CameraFOV = Mathf.Deg2Rad * mainCam.fieldOfView; // radians
        videoFormat = mainCam.aspect; // Video format quotient (ex: 16/9)

        MapInfosScript mapInfos = GameObject.Find("Map Infos")
            .GetComponent<MapInfosScript>();

        listPlayers = mapInfos.listPlayers;

        // Change that
        cameraXMin = mapInfos.xMinLimit;
        cameraXMax = mapInfos.xMaxLimit;
        cameraYMin = mapInfos.yMinLimit;
        cameraYMax = mapInfos.yMaxLimit;
    }

    void Update()
    {
        xMax = cameraXMin;
        xMin = cameraXMax;
        yMax = cameraYMin;
        yMin = cameraYMax;

        foreach (GameObject player in listPlayers) {
            if (player == null)
                continue;

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

        /* Change that */
        xCamera = (xMax + xMin) / 2;
        yCamera = (yMax + yMin) / 2;

        // On cherche la distance (z) entre la caméra et le plan
        // Trigo:
        // zX = (xMax - xMin) * 2 * tan(fov / 2)
        // zY = (yMax - yMin) * 2 * tan(fov / 2)
        // z = max(zX, zY)
        //
        // z = max(xMax - xMin, yMax - yMin) / tan(fov / 2)

        zCamera = -Mathf.Max(15, Mathf.Max((xMax - xMin) / videoFormat, yMax - yMin) / (2 * Mathf.Tan(CameraFOV / 2)));

        // Les joueurs aux positions extremums sont aux bords de la caméra
        // Pour ajouter de la visibilité, on recule un peu la caméra

        zCamera *= cameraRecul;

        transform.position = new Vector3(xCamera, yCamera, zCamera);
    }
}
