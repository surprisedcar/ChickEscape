using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStart : MonoBehaviour
{
    public void OnStartButton(){
        SceneManager.LoadScene("EggStationScene");
    }

    public void OnManualButton(){

        SceneManager.LoadScene("ManualScene");
    }
}
