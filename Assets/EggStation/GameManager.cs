using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        {
            Instance = this;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;
    }
}
