using UnityEngine;

public class startmenu : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject OptionsButton;
    public void onStartButtonClicked()
    {
        Debug.Log("Start button clicked, loading game scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("start scene");
        Time.timeScale = 1;
    }
    public void onExitButtonClicked()
    {
        Debug.Log("Exit button clicked, quitting application...");
        Application.Quit();
    }
    public void onOptionsButtonClicked()
    {
        Debug.Log("options button clicked, loading game scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Options");
    }
}
