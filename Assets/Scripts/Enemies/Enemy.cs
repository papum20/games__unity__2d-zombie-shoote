using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    public EnemySpawner spawner;
    public Animator animator;

    public Transform rayStart;
    public float rayLength = 0.02f;
    public LayerMask whatIsAlive;


    public float speed = 0.5f;
    bool facingRight;
    float lastPositionX;


    int health = 50;
    public bool harmful = true;
    public int damage = 20;
    public float attackRate = 2f;
    float currentAttackTime = 0f;

    float damageAnimationTime = 0.2f;
    float currentDamageTime = 0f;





    private void Start()
    {
        lastPositionX = transform.position.x;

        //WHERE IS FACING AT SPAWNING
        facingRight = true;

    }



    void Update()
    {

        currentAttackTime += Time.deltaTime;

        if (animator.GetBool("damage") == true)
        {
            currentDamageTime += Time.deltaTime;
            if (currentDamageTime >= damageAnimationTime)
                animator.SetBool("damage", false);
        }



        //IF FALLS OUT OF THE WORLD

        if (gameObject.transform.position.y < -15)
            Destroy(gameObject);
    }





    private void FixedUpdate()
    {

        //MOVEMENT
        if (Mathf.Abs(transform.position.x - player.transform.position.x) > 0.75f)
            gameObject.transform.Translate(new Vector2(speed, 0f));
        if (transform.position.x != lastPositionX)
        {
            animator.SetBool("moving", true);
            lastPositionX = transform.position.x;
        }
        else
            animator.SetBool("moving", false);



        //CHECK WHERE THE PALYER IS AND CHANGE DIRECTION

        if (!(player.transform.position.y > transform.position.y && Mathf.Abs(player.transform.position.x - transform.position.x) < 1f))
        {
            if ((player.transform.position.x > transform.position.x && facingRight == false) || (player.transform.position.x < transform.position.x && facingRight == true))
                Flip();
        }




    }







    public void TakeDamage(int damage)
    {
        animator.SetBool("damage", true);
        currentDamageTime = 0f;

        health -= damage;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }





    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }





    void Attack()
    {
        if (currentAttackTime >= attackRate)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            currentAttackTime = 0f;
            StartCoroutine(AttackAnimation());
        }

    }


    IEnumerator AttackAnimation()
    {
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(attackRate / 2f);
        animator.SetBool("attacking", false);
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "player")
            Attack();
    }


}
