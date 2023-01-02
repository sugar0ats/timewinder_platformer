using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public int health;

    private Renderer myRenderer;
    public int numBlinks;
    public float seconds;
    public float dieTime;

    private Animator anim;

    private bool invulnerable;

    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        invulnerable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (!invulnerable)
        {
            health -= damage;
        }
        
        
        if (health <= 0)
        {
            anim.SetTrigger("Death");
            invulnerable = true;
            Invoke("DestroyPlayer", dieTime);
            
        }

        BlinkPlayer(numBlinks, seconds);
    }

    public void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return health;
    }

    void BlinkPlayer(int numBlinks, float seconds)
    {
        invulnerable = true;
        StartCoroutine(Blinks(numBlinks, seconds));
    }

    IEnumerator Blinks(int numBlinks, float seconds)
    {
        for (int i = 0; i < numBlinks * 2; i++)
        {
            myRenderer.enabled = !myRenderer.enabled;

            yield return new WaitForSeconds(seconds);
        }
        myRenderer.enabled = true;
        invulnerable = false;
    }
}
