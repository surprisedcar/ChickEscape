using UnityEngine;

public class ItemMove : MonoBehaviour
{
    public float speed = 5f; // 배경 이동 속도와 맞춰주세요.
    private float leftBoundary = -15f; // 화면 왼쪽 밖으로 완전히 나가는 지점

    void Update()
    {
        // 1. 매 프레임마다 왼쪽으로 이동
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 2. 화면 밖으로 나가면 메모리 관리를 위해 파괴
        if (transform.position.x < leftBoundary)
        {
            Destroy(gameObject);
        }
    }
}