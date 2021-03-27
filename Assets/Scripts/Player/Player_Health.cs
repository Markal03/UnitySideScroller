using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour
{
    public GameOverMenu gameOverMenu;
    public Animator animator;

    public int maxHealth = 100;
    public static int currentHealth;

    public HealthBar healthBar;

    public bool hasDied;
    public bool isImmune;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn player
        hasDied = false;
        isImmune = false;

        Time.timeScale = 1f;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (hasDied == true)
        {
            StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2.5f);
        gameOverMenu.ShowGameOverMenu();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Hazard"))
        {
            TakeDamage(20);
        }

        if (collision.collider.CompareTag("Death"))
        {
            TakeDamage(99999);
        }

    }

    public void TakeDamage(int damage)
    {
        if (!isImmune)
        {
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);

            if (currentHealth < 1)
            {
                hasDied = true;
                isImmune = true;
                animator.SetTrigger("Death");
            }
        }
    }
}
