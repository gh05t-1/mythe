using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int lives = 5;
    private ParticleSystem ps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoseHealth(int amount) {
        lives -= amount;
        //Debug.Log("enemy lost life");
        if (lives <= 0)
        {
            ps.Play();
            GameObject.Destroy(gameObject, 0.30f);
            
        }
    }
}
