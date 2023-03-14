using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damage;

    private SpriteRenderer sr;
    private Color originalColor;
    public float flashTime;
    public GameObject bloodEffect;

    public GameObject deathExplosion; // DEATH EXPLOSION!!

    public PlayerHealth playerHealth;

    [SerializeField] private AudioSource enemyHurtSFX;
    [SerializeField] private AudioSource playerWinSFX;

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(deathExplosion, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        FlashColor(flashTime);
        enemyHurtSFX.Play();
        if (health > 0) {
            playerWinSFX.Play();
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }
    }

    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor", time); // wait for time ms to execute the resetcolor function
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("this works?");
        
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Took damage, current player health is " + playerHealth.GetHealth());
            }
        }
    }
}
