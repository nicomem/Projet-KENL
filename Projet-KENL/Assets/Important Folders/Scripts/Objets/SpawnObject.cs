using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] ObjectPrefabs;
    public float spawnTime = 5f;
    public Vector3 positionToSpawn;

    private float time;
    private GameObject objectSpawned;
    private System.Random rand;

	// Use this for initialization
	void Start ()
    {
        rand = new System.Random();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {   
        // Do not spawn multiple objects at the same time
        if (objectSpawned == null)
            time += Time.deltaTime;

        // Check si cst le bon moment de spawn les objets
        if (time >= spawnTime)
        {
            time = 0;
            int indexObject = GetObjectIndexRandom();

            objectSpawned = Instantiate(ObjectPrefabs[indexObject], positionToSpawn,
                Quaternion.identity);
        }
    }

    private int GetObjectIndexRandom()
    {
        return rand.Next(ObjectPrefabs.Length);
    }
}
