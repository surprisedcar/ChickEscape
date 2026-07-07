using UnityEngine;

public class Item : MonoBehaviour
{
     private AudioSource audioSource;
     public AudioClip itemSound;

      private void Awake()
    {
        // 핵심: 이 오브젝트에 붙어있는 AudioSource 컴포넌트를 가져와서 연결합니다.
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 물체의 태그가 "Player"인 경우
        if (collision.CompareTag("Player"))
        {
           AudioSource.PlayClipAtPoint(itemSound, transform.position);

        // 2. 스코어 증가 로직
        ScoreManager sm = Object.FindFirstObjectByType<ScoreManager>();
        if (sm != null)
        {
            sm.AddItemCount(1);
        }

        // 3. 이제 파괴해도 소리는 안전하게 나옵니다.
        Destroy(gameObject);
        }
    }
}