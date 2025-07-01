using UnityEngine;

public class options : MonoBehaviour
{
    [SerializeField] private GameObject MenuButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuButton.SetActive(true);

    }
    public void onMenuButtonClicked()
    {
        //Debug.Log("menu button clicked, loading game scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("start menu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
        