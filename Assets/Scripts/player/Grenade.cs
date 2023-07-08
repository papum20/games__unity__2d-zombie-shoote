using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public LayerMask whatIsAlive;

    float explosionRadius = 3.5f;

    float explosionTime = 3f;
    float timer = 0f;




    void Update()
    {
        Debug.Log(transform.position.y);
        timer += Time.deltaTime;

        if(timer >= explosionTime)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsAlive);

            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "player")
                    colliders[i].gameObject.GetComponent<PlayerHealth>().TakeDamage(50);
                else if (colliders[i].gameObject.tag == "enemy")
                    colliders[i].gameObject.GetComponent<Enemy>().TakeDamage(50);
            }

            Destroy(gameObject);
        }

    }
}
