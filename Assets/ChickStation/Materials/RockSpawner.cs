using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rockPrefab;
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;
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
            SpawnRock();
        }
    }

    void SpawnRock()
    {
        Instantiate(rockPrefab, transform.position, Quaternion.identity);
    }
}

