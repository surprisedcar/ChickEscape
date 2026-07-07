using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        // 1. 중복 생성 방지 및 파괴 방지 (싱글톤)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            if (audioSource.clip != null) 
        {
            audioSource.Play();
        }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 2. 음악 변경 함수 (핵심 로직)
    public void PlayBGM(AudioClip newClip)
    {
        // 현재 재생 중인 클립과 새로 들어온 클립이 같으면 무시 (음악이 끊기지 않음)
        if (audioSource.clip == newClip) 
            return;

        // 다른 음악이라면 교체 후 재생
        audioSource.clip = newClip;
        audioSource.Play();
    }
}