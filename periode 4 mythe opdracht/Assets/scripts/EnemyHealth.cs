using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int lives = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoseHealth(int amount) {
        lives -= amount;
        Debug.Log("enemy lost life");
    }
}
