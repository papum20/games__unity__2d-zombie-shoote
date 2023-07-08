using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Transform player;

    public int damage = 40;
    public float bulletSpeed = 2000f;
    float disappearDistance = 25f;

    float startingPosition;



    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
        startingPosition = gameObject.transform.position.x;
    }


    void Update()
    {
        if (Mathf.Abs(gameObject.transform.position.x - player.position.x) > disappearDistance)
        {
            Destroy(gameObject);
            Debug.Log("destroy");
        }
    }



    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "enemy")
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if (target.tag == "plantBoss")
        {
            target.transform.parent.gameObject.GetComponent<PlantBoss>().TakeDamage(damage);
        }
        else if(target.tag == "whiteBoss")
            target.transform.parent.gameObject.GetComponent<WhiteBoss>().TakeDamage(damage);


        if (target.tag != "player")
            Destroy(gameObject);
    }

}
