using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{

    public BoxCollider2D trigger;

    public int identifier;



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
