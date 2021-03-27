using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stamina : MonoBehaviour
{

    public int maxStamina = 100;
    public int currentStamina;
    public float staminaRegenerationRate = 5;

    public HealthBar staminaBar;


    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxHealth(maxStamina);
    }

    // Update is called once per frame
    void Update()
    {

        while (currentStamina < maxStamina)
        {
            currentStamina += (int)(maxStamina / staminaRegenerationRate);
            staminaBar.SetHealth(currentStamina);
        }
    }


    public void ConsumeStamina(int amount)
    {
        currentStamina -= amount;
        staminaBar.SetHealth(currentStamina);
        //StartCoroutine(RegenStamina());
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2);

        while (currentStamina < maxStamina)
        {
            currentStamina += (int)(maxStamina / staminaRegenerationRate);
            staminaBar.SetHealth(currentStamina);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public bool CanDash() => currentStamina > 19;
}
