using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public int damage = 2;
    public PlayerHealth playerHealth;
    bool inShadow=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("Damage", 2.0f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Damage()
    {
        if (inShadow == false)
        {
            //Debug.Log("rwar");
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Shadow")
        {
            inShadow = true;
            //Debug.Log("yippe");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shadow")
        {
            Debug.Log("yippe");
            inShadow = false;
        }
    }

    

}
