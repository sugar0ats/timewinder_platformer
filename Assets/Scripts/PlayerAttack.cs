using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int damage;
    public float startTime;
    public float duration;

    private Animator anim;
    private PolygonCollider2D coll2d;

    private bool canAttack;

    [SerializeField] private AudioSource shingSFX;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        coll2d = GetComponent<PolygonCollider2D>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (canAttack && Input.GetButtonDown("Attack"))
        {
            //coll2d.enabled = true;
            canAttack = false;
            anim.SetTrigger("Attack");
            shingSFX.Play();
            Debug.Log("attack triggered, value of attack trigger is " + anim.GetBool("Attack"));
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(startTime);
        coll2d.enabled = true;
        StartCoroutine(disableHitBox());
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(duration);
        coll2d.enabled = false ;
        Debug.Log("attack trigger value is " + anim.GetBool("Attack"));
        canAttack = true;
        //anim.ResetTrigger("Attack");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") )
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
