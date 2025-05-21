using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    [Header("Ability 1")]
    public Image abilityImage1;
    public float cooldown1 = 5f;
    private bool isCooldown = false;
    public KeyCode ability1;

    private float cooldownTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        abilityImage1.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
    }

    void Ability1()
    {
        if (Input.GetKeyDown(ability1) && !isCooldown)
        {
            isCooldown = true;
            cooldownTimer = cooldown1;
            abilityImage1.fillAmount = 1f;
        }

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            abilityImage1.fillAmount = cooldownTimer / cooldown1;

            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                abilityImage1.fillAmount = 0f;
            }
        }
    }
}
