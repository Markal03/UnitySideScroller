using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickup : MonoBehaviour
{

    public int healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<Player_Health>().Heal(healAmount);
            gameObject.SetActive(false);
        }
    }


}
