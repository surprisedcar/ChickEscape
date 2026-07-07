
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeHit : MonoBehaviour
{
  public static Status status;
 private AudioSource audioSource;
    public EggInputController eggInput;
    public AudioClip hitSound;

    private void Awake()
    {
        // 핵심: 이 오브젝트에 붙어있는 AudioSource 컴포넌트를 가져와서 연결합니다.
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(">>> TRIGGER WITH: "+ collision.name);
        
        if (collision.CompareTag("Player"))
        {
          /*  Debug.Log(">>> HITEGG - TRY LODSCENE");
            Time.timeScale = 1f;
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        
        */
        audioSource.PlayOneShot(hitSound);
         Status.instance.TakeDamage(10f);
        }
    }
   
}
