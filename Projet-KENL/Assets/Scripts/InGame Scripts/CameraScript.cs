using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera mainCam;
    private GameObject[] listPlayers;
    private PlayerScript[] listPlayerScripts;
    private MapInfosScript mapInfos;
    private float cameraRecul;
    private float CameraFOV;
    private float videoFormat;

    public float cameraXMin, cameraXMax, cameraYMin, cameraYMax;
    private float xMax, xMin, yMax, yMin;
    private float posX, posY;
    private float xCamera, yCamera, zCamera;
    private bool initCamera;

    // Use this for initialization
    void Start()
    {
        mainCam = Camera.main;
        cameraRecul = 1.5f;

        CameraFOV = Mathf.Deg2Rad * mainCam.fieldOfView; // radians
        videoFormat = mainCam.aspect; // Video format quotient (ex: 16/9)

        mapInfos = GameObject.Find("Map Infos").GetComponent<MapInfosScript>();
    }

    void Update()
    {
        if (!initCamera)
            InitCamera();
        else
            MoveCameraAuto();
    }

    private void InitCamera()
    {
        if (mapInfos.InitPlayersFinished) {
            listPlayers = mapInfos.ListPlayers;
            listPlayerScripts = mapInfos.ListPlayerScripts;

            cameraXMin /= cameraRecul;
            cameraXMax /= cameraRecul;
            cameraYMin /= cameraRecul;
            cameraYMax /= cameraRecul;

            initCamera = true;
        }
    }

    private void MoveCameraAuto()
    {
        xMax = cameraXMin;
        xMin = cameraXMax;
        yMax = cameraYMin;
        yMin = cameraYMax;
        
        for (int i = 0; i < listPlayers.Length; i++) {
            var player = listPlayers[i];
            var playerScript = listPlayerScripts[i];

            // We don't couont if ko (== not in scene)
            if (playerScript.isKO)
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

        xCamera = xMin + ((xMax - xMin) / 2);
        yCamera = yMin + ((yMax - yMin) / 2);
        zCamera = (90 / mainCam.fieldOfView) * cameraRecul;
        zCamera = Mathf.Max(25f, zCamera);

        Vector3 unSmoothCameraPos = new Vector3(xCamera, yCamera, -zCamera);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position,
            unSmoothCameraPos, Time.deltaTime * 5f);
    }
}