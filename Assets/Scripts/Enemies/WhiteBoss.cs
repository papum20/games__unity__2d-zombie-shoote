using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBoss : MonoBehaviour
{

    public GameObject player;
    BossCamera bossCamera;
    Animator animator;
    public GameObject attackPrefab;
    public GameObject trigger;

    int health = 1000;
    int maxHealth = 1000;
    bool toAttack = false;
    int state = 0;

    float attackRate = 5f;
    float currentAttackTime = 0f;




    void Start()
    {
        bossCamera = GetComponent<BossCamera>();
        animator = GetComponent<Animator>();

        animator.SetBool("atk", false);
        animator.SetBool("dying", false);
        animator.SetInteger("state", 0);
    }




    void Update()
    {

        if(animator.GetBool("atk") == false)
            animator.SetInteger("state", state);


        currentAttackTime += Time.deltaTime;

        //RANDOMLY CHOOSES WHETHER TO ATTACK, IF ENOUGH TIME HAS PASSED

        int[] choice = { 0, 1 };
        if (toAttack && currentAttackTime >= attackRate && choice[Random.Range(0,2)] == 1)
        {
            StartCoroutine(AttackNormal());
            currentAttackTime = 0f;
        }


    }




    IEnumerator AttackNormal()
    {
        animator.SetBool("atk", true);

        yield return new WaitForSeconds(0.66f);

        trigger.GetComponent<WhiteBossTriggerAssigner>().InvertColliders();
        GameObject attack = Instantiate(attackPrefab, new Vector2(transform.position.x - 4f, 1f), Quaternion.identity);
        attack.GetComponent<whiteBossHit>().player = player;

        yield return new WaitForSeconds(2f);

        trigger.GetComponent<WhiteBossTriggerAssigner>().InvertColliders();
        animator.SetBool("atk", false);
        currentAttackTime = 0f;
    }




    public void TakeDamage(int damage)
    {
        health -= damage;
        bossCamera.SetSliderValue(health);

        if (health <= maxHealth * 2 / 3)
            state = 1;
        if (health <= maxHealth / 3)
            state = 2;
        if (health <= 0)
            StartCoroutine(Dying());
    }



    IEnumerator Dying()
    {
        bossCamera.BossFightEndedHud();
        toAttack = false;
        animator.SetBool("dying", true);

        yield return new WaitForSeconds(4f);

        bossCamera.BossFightEndedCamera();
        Destroy(gameObject);
    }




    public void StartAttacking()
    {
        toAttack = true;
        bossCamera.SetSliderMaxValue(maxHealth);
        bossCamera.SetSliderValue(health);
        health = maxHealth;
    }


}
