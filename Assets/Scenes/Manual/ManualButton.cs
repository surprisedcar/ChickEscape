using UnityEngine;
using UnityEngine.SceneManagement;
public class ManualButton : MonoBehaviour
{

    public void OnHomeButton(){

        SceneManager.LoadScene("HomeScene");
    }
}
