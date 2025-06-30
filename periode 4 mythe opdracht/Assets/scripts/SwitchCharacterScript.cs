using System.Collections;
using UnityEngine;

public class SwitchCharacterScript : MonoBehaviour
{
    public GameObject avatar1; // Vampire (child of Player)
    public GameObject avatar2; // Bat (child of Player)

    [Header("Transformation Settings")]
    public float batDuration = 5f;
    public float cooldownDuration = 10f;
    public GameObject GetCurrentPlayer()
    {
        return player;
    }


    private bool isOnCooldown = false;
    private bool isTransforming = false;



    private GameObject player;

    void Start()
    {

        player = Instantiate(avatar1, transform);


        //avatar1.SetActive(true);
        //avatar2.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Transform") && !isOnCooldown && !isTransforming)
        {
            StartCoroutine(TemporaryTransformation());
        }
        else if (Input.GetButtonDown("Transform") && isTransforming)
        {
            Destroy(player);
            player = Instantiate(avatar1, transform);






            isTransforming = false;
            StartCoroutine(CooldownTimer());
        }
    }

    private IEnumerator TemporaryTransformation()
    {
        isTransforming = true;

        /*
        // Use current position of the active avatar
        Vector3 position = avatar1.activeSelf ? avatar1.transform.position : avatar2.transform.position;
        */

        /*
        // Transform to bat
        avatar1.SetActive(false);
        avatar2.transform.position = position;
        avatar2.SetActive(true);
        */

        Destroy(player);
        player = Instantiate(avatar2, transform);





        yield return new WaitForSeconds(batDuration);


        /*
        // Transform back to vampire
        position = avatar2.transform.position;
        avatar2.SetActive(false);
        avatar1.transform.position = position;
        avatar1.SetActive(true);

        */

        Destroy(player);
        player = Instantiate(avatar1, transform);






        isTransforming = false;
        StartCoroutine(CooldownTimer());
    }

    private IEnumerator CooldownTimer()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }
}
