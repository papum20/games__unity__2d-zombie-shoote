using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleWeapon : MonoBehaviour
{

    public BoxCollider2D trigger;

    public int damage;
    public float fireRate;
    public int identifier;
    public int ammo;



    private void Awake()
    {
        trigger.enabled = false;
    }


    void Start()
    {
        //WAIT FOR MAKING IT COLLECTIBLE

        StartCoroutine(triggerEnabler());
    }

    



    IEnumerator triggerEnabler()
    {
        yield return new WaitForSeconds(2f);
        trigger.enabled = true;
    }


}
