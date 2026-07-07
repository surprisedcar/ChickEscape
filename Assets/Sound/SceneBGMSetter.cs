using UnityEngine;

public class SceneBGMSetter : MonoBehaviour
{
    public AudioClip musicForThisScene; // 인스펙터에서 음악 파일을 넣어주세요.

    void Start()
    {
        // 게임 시작 시 매니저에게 음악 재생 요청
        if (BackgroundMusic.instance != null)
        {
            BackgroundMusic.instance.PlayBGM(musicForThisScene);
        }
    }
}