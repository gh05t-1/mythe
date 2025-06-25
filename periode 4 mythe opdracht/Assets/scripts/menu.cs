using UnityEngine;

public class menu : MonoBehaviour
{
    [SerializeField] private GameObject MenuButton;
    [SerializeField] private GameObject ResumeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuButton.SetActive(false);
        ResumeButton.SetActive(false);
    }
    public void onMenuButtonClicked()
    {
        Debug.Log("menu button clicked, loading game scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("start menu");
    }
    public void onResumeButtonClicked()
    {
        Debug.Log("resume");
        Time.timeScale = 1;
        MenuButton.SetActive(false);
        ResumeButton.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuButton.SetActive(true);
            ResumeButton.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
        
    }
}
