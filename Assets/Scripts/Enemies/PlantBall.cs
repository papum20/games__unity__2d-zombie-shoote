using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantBall : MonoBehaviour
{

    Animator animator;
    public CircleCollider2D circleCollider;
    public BoxCollider2D boxCollider;

    int damage = 20;
    bool damaged = false;
    bool grounded = false;





    private void Start()
    {
        boxCollider.enabled = false;
        animator = GetComponent<Animator>();
    }






    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(damaged == false && collision.tag == "player")
        {
            collision.transform.parent.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            damaged = true;
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(grounded == false && collision.gameObject.tag == "ground")
        {
            boxCollider.enabled = true;
            circleCollider.enabled = false;
            animator.SetBool("hit", true);
            grounded = true;
            StartCoroutine(BallDestroy());
        }
    }



    IEnumerator BallDestroy()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }


}
