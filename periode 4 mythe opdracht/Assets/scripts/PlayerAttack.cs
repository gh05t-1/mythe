using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]private GameObject attackArea;

    private bool attacking = false;

    private float attackTime = 0.25f;
    private float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {

            Attack();
            
        }

        if (attacking)
        {
            
            timer += Time.deltaTime;

            if (timer >= attackTime)
            {
                timer = 0f;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    private void Attack()
    {
       
        attacking = true;
        attackArea.SetActive(attacking);
    }


}
