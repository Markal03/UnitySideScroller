using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour
{
    public GameOverMenu gameOverMenu;
    public Animator animator;

    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public bool hasDied;
    // Start is called before the first frame update
    void Start()
    {
        //Spawn player
        hasDied = false;
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (hasDied == true)
        {
            //gameOverMenu.ShowGameOverMenu();
            //StartCoroutine("Die");
        }
    }

    IEnumerator Die()
    {
        gameOverMenu.ShowGameOverMenu();
        yield return null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Hazard"))
        {
            TakeDamage(20);
            //hasDied = true;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth < 1)
        {
            hasDied = true;
            animator.SetTrigger("Death");
        }
    }
}
