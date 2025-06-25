using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackArea : MonoBehaviour
{
    //private int damage = 3;

    public PlayerHealth playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("this works");

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
            enemy.GetComponent<EnemyHealth>().LoseHealth(2);
            playerHealth.Revive(10);

        }
        /*
         if (playerHealth  != null)
        {
            Debug.Log("does this work");
            playerHealth.TakeDamage(damage);
        }*/
    }

}
