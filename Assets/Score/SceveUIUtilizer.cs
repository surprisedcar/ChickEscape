using UnityEngine;
using TMPro;

public class SceneUIInitializer : MonoBehaviour
{
    public TextMeshProUGUI sceneScoreText;
    public TextMeshProUGUI sceneItemText;

    void Start()
    {
        // 씬 시작하자마자 기존의 ScoreManager를 찾아 새 UI를 전달함
        if (ScoreManager.instance != null)
        {
            //ScoreManager.instance.SetupNewSceneUI(sceneScoreText, sceneItemText);
        }
    }
}