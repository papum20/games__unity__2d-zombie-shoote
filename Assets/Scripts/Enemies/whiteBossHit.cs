using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteBossHit : MonoBehaviour
{

    public GameObject player;
    public PolygonCollider2D colliderHigh;
    public PolygonCollider2D colliderShort;
    public Animator animator;

    float currentTime = 0f; //DURATION TIME
    float nextTime = 0.1f;
    int number = 15;    //NUMBER OF TIMES IT RESPAWNS
    int numberCounter = 0;
    int damage = 20;
    bool harmful = true;

    bool high;



    void Start()
    {
        animator = GetComponent<Animator>();
        harmful = true;
        high = true;
    }



    void Update()
    {
        currentTime += Time.deltaTime;


        if (numberCounter > number)
            Destroy(gameObject);
    }






    private void FixedUpdate()
    {

        //TRANSLATION

        if (currentTime >= nextTime)
        {
            numberCounter++;
            currentTime = 0f;
            transform.Translate(new Vector2(-2f, 0f));

            high = !high;
            colliderHigh.enabled = high;
            colliderShort.enabled = !high;
            harmful = true;

            //ANIMATOR
            animator.SetBool("high", high);
        }
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player" && harmful == true)
        {
            collision.transform.parent.GetComponent<PlayerHealth>().TakeDamage(damage);
            harmful = false;
        }
    }


}
