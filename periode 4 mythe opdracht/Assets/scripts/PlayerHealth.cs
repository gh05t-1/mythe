using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 10;
    public Slider Slider;
    [SerializeField] private Transform respawnPoint;
    private Renderer[] renderers;
    private Collider[] colliders;
    private SpriteRenderer[] spriteRenderers;
    [SerializeField] private SwitchCharacterScript switchCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        Slider.maxValue = maxHealth;
        Slider.value = health;
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
        //spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        //Debug.Log(health);
        Slider.value = health;

        if (health <= 0)
        {
            //Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    /*private IEnumerator RespawnCoroutine()
    {
        SetPlayerVisible(false);

        ToggleColliders(false);

        health = maxHealth;
        Slider.value = health;

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }

        SetPlayerVisible(true);
        ToggleColliders(true);

        yield return null; 
    }*/
    
    private void SetPlayerVisible(bool visible)
    {
        var player = switchCharacter.GetCurrentPlayer(); // get the current one
        if (player == null) return;

        foreach (var sr in player.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sr != null)
                sr.enabled = visible;
        }
        //foreach (var sr in spriteRenderers)
        {
            //sr.enabled = visible;
        }
    }
    
    public void Revive(int amount) {
        //Debug.Log("try to revive");
        if (health + amount <= maxHealth)
        {
            health += amount;
            Slider.value = health;
        }
        else
        {
            health = maxHealth;
        }
    }
    

    private void ToggleColliders(bool active)
    {
        foreach (var c in colliders)
        {
            c.enabled = active;
        }
    }
    
}