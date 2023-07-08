using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public Transform player;

    public Animator animator;
    public Transform itemParent;
    public BoxCollider2D trigger;

    public GameObject[] weapons;
    public GameObject[] items;
    public bool[] maxStats = { false, false, false };

    bool CanOpen = true;




    private void Update()
    {
        if (player.position.x - transform.position.x >= 350)
            Destroy(gameObject);
    }





    public void Open()
    {
        if (CanOpen)
        {
            animator.SetBool("open", true);

            StartCoroutine(ItemSpawn());
        }
    }



    IEnumerator ItemSpawn()
    {
        CanOpen = false;
        trigger.enabled = false;    //
        yield return new WaitForSeconds(1f);


        GameObject[] itemGenerator = new GameObject[6];
        int index = 0;
        for(int i = 0; i < weapons.Length; i++)
        {
            itemGenerator[index++] = weapons[i];
        }
        for(int i = 0; i < items.Length; i++)
        {
            if (maxStats[i] == false)
                itemGenerator[index++] = items[i];
        }

        Vector2 spawPosition = new Vector2(transform.position.x, transform.position.y + 0.8f);

        /*for (int i = 0; i < index; i++)
        {
            Debug.Log(i);
            Debug.Log(itemGenerator[i].name);
        }*/

        GameObject item = Instantiate(itemGenerator[Random.Range(0, index)], spawPosition, Quaternion.identity, itemParent);
    }



}
