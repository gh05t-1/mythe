using System.Collections;
using UnityEngine;

public class SwitchCharacterScript : MonoBehaviour
{
    public GameObject avatar1; // Vampire (child of Player)
    public GameObject avatar2; // Bat (child of Player)

    [Header("Transformation Settings")]
    public float batDuration = 5f;
    public float cooldownDuration = 10f;

    private bool isOnCooldown = false;
    private bool isTransforming = false;

    void Start()
    {
        avatar1.SetActive(true);
        avatar2.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isOnCooldown && !isTransforming)
        {
            StartCoroutine(TemporaryTransformation());
        }
    }

    private IEnumerator TemporaryTransformation()
    {
        isTransforming = true;

        // Use current position of the active avatar
        Vector3 position = avatar1.activeSelf ? avatar1.transform.position : avatar2.transform.position;

        // Transform to bat
        avatar1.SetActive(false);
        avatar2.transform.position = position;
        avatar2.SetActive(true);

        yield return new WaitForSeconds(batDuration);

        // Transform back to vampire
        position = avatar2.transform.position;
        avatar2.SetActive(false);
        avatar1.transform.position = position;
        avatar1.SetActive(true);

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
