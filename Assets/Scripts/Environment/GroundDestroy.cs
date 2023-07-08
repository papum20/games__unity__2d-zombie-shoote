using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroy : MonoBehaviour
{

    public Transform player;




    void Update()
    {
        if (player.position.x - transform.position.x >= 350)
            Destroy(gameObject);
    }
}
