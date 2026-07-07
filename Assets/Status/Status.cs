using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Status : MonoBehaviour
{
    [Header("UI Reference")]
    public Image HeartBar;
    public SpriteRenderer playerSprite;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Invincible Settings")]
    public float invincibilityDuration = 1.5f;
    private bool isInvincible = false;
    private bool isDead = false;

    public static Status instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // UI가 아닌 독립적인 빈 게임오브젝트에 이 스크립트가 있어야 합니다.
            transform.SetParent(null); 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetHealth();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 즉시가 아니라, 한 프레임 뒤에 UI를 찾도록 코루틴 실행
        StartCoroutine(FindReferencesAfterLoad(scene.name));
    }

    private IEnumerator FindReferencesAfterLoad(string sceneName)
    {
        // 씬 내의 모든 오브젝트가 완전히 Initialize될 때까지 한 프레임 대기
        yield return null;

        if (sceneName == "EggStationScene") 
        {
            ResetHealth();
        }

        // 1. 하트바 다시 연결 (이름이 정확해야 함)
        GameObject healthBarObj = GameObject.Find("HeartBarImage");
        if (healthBarObj != null)
        {
            HeartBar = healthBarObj.GetComponent<Image>();
            // Image Type이 Filled인지 확인 (코드에서 강제 설정 가능)
            if(HeartBar.type != Image.Type.Filled) HeartBar.type = Image.Type.Filled;
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Status: 'HeartBarImage'를 찾을 수 없습니다.");
        }

        // 2. 플레이어 다시 연결
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerSprite = player.GetComponent<SpriteRenderer>();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 코루틴 중복 방지를 위해 기존 코루틴을 멈추고 새로 시작하는 것이 안전합니다.
            StopAllCoroutines(); 
            StartCoroutine(BecomeInvincible());
        }
    }

    void UpdateUI()
    {
        if (HeartBar != null)
        {
            HeartBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        isInvincible = false;
        UpdateUI();
    }

    private void Die()
    {
        isDead = true;
        if (ScoreManager.instance != null) ScoreManager.instance.SaveHighScore();
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        float elapsed = 0f;
        float blinkSpeed = 0.1f;

        while (elapsed < invincibilityDuration)
        {
            if (playerSprite != null)
            {
                Color c = playerSprite.color;
                c.a = (c.a == 1f) ? 0.4f : 1f;
                playerSprite.color = c;
            }
            else break;

            yield return new WaitForSeconds(blinkSpeed);
            elapsed += blinkSpeed;
        }

        if (playerSprite != null)
        {
            Color finalColor = playerSprite.color;
            finalColor.a = 1f;
            playerSprite.color = finalColor;
        }
        
        isInvincible = false;
    }
}