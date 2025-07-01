using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackArea : MonoBehaviour
{
    //private int damage = 3;
    private GameObject player;

    private PlayerHealth Health; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Debug.Log("this works");
        player = GameObject.FindGameObjectWithTag("Player");
        Health = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit something");
        var enemy = collision.gameObject;
        if (enemy.CompareTag("Enemy")) {

            Debug.Log("hit an anemy");
            enemy.GetComponent<EnemyHealth>().LoseHealth(5);
            Health.Revive(50);

        }
        /*
         if (playerHealth  != null)
        {
            Debug.Log("does this work");
            playerHealth.TakeDamage(damage);
        }*/
    }

}
