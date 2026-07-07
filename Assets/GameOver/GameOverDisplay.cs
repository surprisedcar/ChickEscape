using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    void Start()
    {
        // 1. 저장된 float 값을 가져옵니다.
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);

        // 2. "N0" 포맷을 사용하여 3자리마다 쉼표를 찍고 소수점은 없앱니다.
        // N0의 뜻: Number 포맷, 소수점 아래 0자리까지 표시
        bestScoreText.text = "BEST SCORE : " + highScore.ToString("N0");
    }
}