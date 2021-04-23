using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    public float attackRange = 1.0f;
    public int attackDamage = 10;
    public LayerMask playerLayers;
    public bool isBoss = false;
    public Transform attackPoint;

    public int maxHealth = 100;

    private int currentHealth;
    private bool hasDied = false;
    public HealthBar healthBar;
    public GameObject heartPrefab;

    private DamageSound damageSound;
    private SpriteRenderer sr;
    private Material materialWhite;
    private Material materialDefault;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        materialWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        materialDefault = sr.material;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        damageSound = gameObject.GetComponent<DamageSound>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasDied == true)
        {
            StartCoroutine(nameof(Die));
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        damageSound.PlayDamageSound();

        sr.material = materialWhite;
        Invoke(nameof(ResetMaterial), .1f);

        if (currentHealth < 1)
        {
            hasDied = true;
        }

    }

    void Attack()
    {

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in hitPlayers)
        {
            Debug.Log("We hit" + player.name);
            player.GetComponent<Player_Health>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator Die()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Death");
        DisableColliders();
        GetComponentInChildren<HealthBar>().gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        
        if(isBoss)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (Random.value < .2f) // 20% chance to spawn a heart
            SpawnHeart();
    }

    private void DisableColliders()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        Collider2D collider2D = GetComponent<Collider2D>();
        if (collider2D) collider2D.enabled = false;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D) boxCollider2D.enabled = false;
    }

    private void SpawnHeart()
    {
        Instantiate(heartPrefab, new Vector3(
            gameObject.transform.position.x,
            gameObject.transform.position.y + 0.5f,
            gameObject.transform.position.z),
            Quaternion.identity);
    }

    private void ResetMaterial()
    {
        sr.material = materialDefault;
    }
}
