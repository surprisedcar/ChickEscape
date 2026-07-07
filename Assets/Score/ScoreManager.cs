using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public EggController egg;

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI itemText;
    public GameObject jumpTextObject; // 씬 이동 시 다시 찾아야 함

    [Header("Score Settings")]
    public float scoreMultiplier = 100f;
    public int targetItemCount = 20;
    public int targetItemCountForJump = 20;

    private bool hasShownJumpText = false;
    private float currentScore = 0f;
    private int itemCount = 0;
    private int totalItemCount = 0;
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null); // 최상위 부모로 이동 (에러 방지)
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. UI 텍스트 다시 찾기 (씬마다 Tag 설정 필수!)
        GameObject scoreObj = GameObject.FindWithTag("ScoreText");
        if (scoreObj != null) scoreText = scoreObj.GetComponent<TextMeshProUGUI>();

        GameObject itemObj = GameObject.FindWithTag("ItemText");
        if (itemObj != null) itemText = itemObj.GetComponent<TextMeshProUGUI>();

        // 2. 점프 텍스트 오브젝트 다시 찾기 (중요!)
        jumpTextObject = GameObject.FindWithTag("JumpText");
        if (jumpTextObject != null) jumpTextObject.SetActive(false);

        // 3. 새 씬의 Egg 찾기
        egg = Object.FindAnyObjectByType<EggController>();
        
        // 4. 메인 씬으로 돌아왔을 때 초기화
        if (scene.name == "EggStationScene")
        {
            ResetData();
        }
    }

    void Update()
    {
        if (isGameOver) return;

        currentScore += Time.deltaTime * scoreMultiplier;
        UpdateUI();

        // 씬별 로직
        if (SceneManager.GetActiveScene().name == "EggStationScene" && egg != null)
        {
            if (totalItemCount < 20 && egg.brokenGauge >= 20) GameOver();
            else if (totalItemCount >= 20 && egg.brokenGauge >= 20) Evolve();
        }

        // 점프 안내 텍스트 출력 조건
       if (totalItemCount >= targetItemCountForJump && !hasShownJumpText)
    {
        hasShownJumpText = true; // 여기서 즉시 true로 만들어 Update가 다시 안 들어오게 함
        ShowJumpText();
    }
    }

 void ShowJumpText()
{
    if (jumpTextObject != null)
    {
        // hasShownJumpText = true; // Update에서 미리 하는 것이 더 안전함
        jumpTextObject.SetActive(true);
        // 특정 코루틴만 멈추는 것이 안전합니다.
        StopCoroutine("AnimateJumpText"); 
        StartCoroutine(AnimateJumpText());
    }
}

   IEnumerator AnimateJumpText()
{
    if (jumpTextObject == null) yield break;

    RectTransform rect = jumpTextObject.GetComponent<RectTransform>();
    TextMeshProUGUI text = jumpTextObject.GetComponent<TextMeshProUGUI>();

    if (rect != null && text != null)
    {
        // UI 앵커가 중앙일 때 가장 안전한 좌표 (화면 너비 기준)
        float screenW = Screen.width; 
        Vector2 startPos = new Vector2(-screenW / 1.5f, 0); // 너무 멀지 않게 조정
        Vector2 targetPos = Vector2.zero;
        Vector2 exitPos = new Vector2(screenW / 1.5f, 0);
        
        float duration = 0.8f; // 속도가 너무 빠르면 안 보이니 약간 조절
        float elapsed = 0f;

        // 1. 등장
        while (elapsed < duration)
        {
            if (rect == null) yield break;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // 부드러운 움직임을 위해 곡선(SmoothStep) 적용 추천
            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, Mathf.SmoothStep(0, 1, t));
            text.alpha = t;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f); // 중앙에 머무는 시간

        // 2. 퇴장
        elapsed = 0f;
        while (elapsed < duration)
        {
            if (rect == null) yield break;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rect.anchoredPosition = Vector2.Lerp(targetPos, exitPos, Mathf.SmoothStep(0, 1, t));
            text.alpha = 1 - t;
            yield return null;
        }

        jumpTextObject.SetActive(false);
    }
}
   public void AddItemCount(int amount = 1)
    {
        if (isGameOver) return;
        itemCount += amount;
        totalItemCount += amount;
        
        // 현재 활성화된 씬 이름을 가져옴
        string currentScene = SceneManager.GetActiveScene().name;

        // 씬별로 다른 목표치 설정
        int currentTarget = targetItemCount; // 기본값 (20)

        // 만약 병아리 스테이지라면 목표치를 더 높게 잡음 (예: 30)
        if (currentScene == "ChickStation")
        {
            currentTarget = 50; //  병아리에서 닭이 되려면 30개 필요!
        }

        if (currentScene != "EggStationScene" && itemCount >= currentTarget)
        {
            Evolve();
        }
    }

 void Evolve()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        itemCount = 0; // 스테이지가 바뀌면 현재 씬에서 모은 개수 초기화

        if (currentSceneName == "EggStationScene") 
        {
            SceneManager.LoadScene("ChickStation");
        }
        else if (currentSceneName == "ChickStation") 
        {
            SceneManager.LoadScene("ChickenStationScene");
        }
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"SCORE: {(int)currentScore:N0}";
        if (itemText != null) itemText.text = $" : {totalItemCount}";
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        SaveHighScore();
        SceneManager.LoadScene("GameOverScene");
    }

    public void SaveHighScore()
    {
        float oldHighScore = PlayerPrefs.GetFloat("HighScore", 0);
        if (currentScore > oldHighScore)
        {
            PlayerPrefs.SetFloat("HighScore", currentScore);
            PlayerPrefs.Save();
        }
    }

    public void ResetData()
    {
        currentScore = 0f;
        itemCount = 0;
        totalItemCount = 0;
        isGameOver = false;
        hasShownJumpText = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public float GetCurrentScore() { return currentScore; }
    public int GetTotalItemCount() { return totalItemCount; }
}