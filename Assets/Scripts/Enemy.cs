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

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // Update is called once per frame
    public void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        FlashColor(flashTime);
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
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
}
