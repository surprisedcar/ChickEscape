using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject snakePrefab;
    public GameObject rockPrefab;

    public GameObject treePrefab;
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    public Transform rockSpawnPoint;
public Transform snakeSpawnPoint;
public Transform treeSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    System.Collections.IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(delay);
            SpawnObstacle();
        }
    }




void SpawnObstacle()
{
    int random = Random.Range(0, 3);

    if (random == 0)
    {
        Instantiate(rockPrefab, rockSpawnPoint.position, Quaternion.identity);
    }
    else if(random == 1)
    {
        Instantiate(snakePrefab, snakeSpawnPoint.position, Quaternion.identity);
    }
    else{
        Instantiate(treePrefab, treeSpawnPoint.position, Quaternion.identity);
    }
}
}
