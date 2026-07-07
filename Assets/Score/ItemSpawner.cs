using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // 생성할 아이템 프리팹
    public float spawnInterval = 1.5f; // 생성 간격 (초)
    public float minY = -2f; // 아이템이 생성될 최소 높이
    public float maxY = 2f;  // 아이템이 생성될 최대 높이

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    void SpawnItem()
    {
        // 1. 랜덤한 높이 설정
        float randomY = Random.Range(minY, maxY);
        // 2. 생성 위치 설정 (스포너의 X 위치, 랜덤 Y 위치)
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0);

        // 3. 아이템 생성
        Instantiate(itemPrefab, spawnPos, Quaternion.identity);
    }
}