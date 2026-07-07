using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverManager : MonoBehaviour
{

    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalItemText;

    void Start()
    {
        // ScoreManager가 존재하는지 확인 후 데이터 가져오기
        if (ScoreManager.instance != null)
        {
            // ScoreManager에 저장된 변수 이름에 맞춰 호출하세요.
            // (앞서 수정했던 currentScore와 totalItemCount를 사용합니다)
            float score = ScoreManager.instance.GetCurrentScore(); 
            int items = ScoreManager.instance.GetTotalItemCount();

            finalScoreText.text = $"FINAL SCORE\n{((int)score).ToString("N0")}";
            finalItemText.text = $"TOTAL ITEMS\n{items}";
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void OnHomeButton()
   {
    if (ScoreManager.instance != null)
    {
        // ScoreManager에 만든 ResetData()를 호출하여 점수/아이템 초기화
        ScoreManager.instance.ResetData(); 
    }
    SceneManager.LoadScene("HomeScene");
   }


}
