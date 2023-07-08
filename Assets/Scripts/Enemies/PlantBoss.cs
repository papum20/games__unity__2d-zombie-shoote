using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBoss : MonoBehaviour
{

    public Transform player;
    public Transform fireStart;
    public GameObject ballPrefab;
    public Transform ballParent;
    public Animator animator;

    BossCamera bossCamera;


    int maxHealth = 2500;
    int health;
    int animatorState = 0;

    float attackRate = 4f;
    float currentTime = 0f;
    float nextTime;
    bool toAttack = false;  //IF PLAYER IS NEAR START ATTACKING




    void Start()
    {
        health = maxHealth;
        nextTime = attackRate;

        bossCamera = gameObject.GetComponent<BossCamera>();
        bossCamera.SetSliderMaxValue(health);

        animator.SetInteger("State", 0);
        animator.SetBool("atk", false);
        animator.SetBool("spAtk", false);
        animator.SetBool("dead", false);
    }



    void Update()
    {

        //CHANGE ANIMATION IF HEALTH LOWERED

        bool atk = animator.GetBool("atk");
        bool spAtk = animator.GetBool("spAtk");
        if (atk == false && spAtk == false)
            animator.SetInteger("State", animatorState);



        //ATTACK

        currentTime += Time.deltaTime;

        if(toAttack && currentTime >= nextTime)
        {
            //EACH 0.5 SECONDS RANDOMLY CHOOSES IF ATTACKING

            int attackOrNot = Random.Range(0, 2);

            if (attackOrNot == 0)
                nextTime += 0.5f;

            else
            {
                //RANDOMLY CHOOSES THE ATTACK TO DO
                int attackChoice = Random.Range(0, 3);

                if (attackChoice == 0)
                    StartCoroutine(Attack());
                else if (attackChoice == 1)
                    StartCoroutine(SpecialAttack());
                else if (attackChoice == 2)
                    StartCoroutine(TripleAttack());


                currentTime = 0f;
                nextTime = attackRate;
            }
        }


    }





    Vector2 ForceCalc(float x)
    {
        float y0 = fireStart.position.y;  //STARTING HEIGHT
        float g = - Physics2D.gravity.y * ballPrefab.GetComponent<Rigidbody2D>().gravityScale;
        float t = Random.Range(0.8f, 2f); //FLYING TIME

        float xVelocity = x / t;
        float yVelocity = (0 + 0.5f * g * Mathf.Pow(t, 2) - y0) / t;

        return new Vector2(-xVelocity, yVelocity);
    }



    //ATTACKS

    IEnumerator Attack()
    {
        animator.SetBool("atk", true);

        yield return new WaitForSeconds(1f);

        GameObject ball = Instantiate(ballPrefab, fireStart.position, Quaternion.identity, ballParent);
        float xTmp = Mathf.Abs(transform.position.x - player.transform.position.x);
        ball.GetComponent<Rigidbody2D>().AddForce(ForceCalc(xTmp), ForceMode2D.Impulse);
        currentTime = 0f;

        yield return new WaitForSeconds(0.75f);
        animator.SetBool("atk", false);      
    }


    IEnumerator SpecialAttack()
    {
        animator.SetBool("spAtk", true);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 3; i++)
        {
            GameObject ball = Instantiate(ballPrefab, fireStart.position, Quaternion.identity, ballParent);
            float xTmp = Mathf.Abs(transform.position.x - player.transform.position.x);
            ball.GetComponent<Rigidbody2D>().AddForce(ForceCalc(xTmp), ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.75f);
        }

        animator.SetBool("spAtk", false);
        currentTime = 0f;
    }


    IEnumerator TripleAttack()
    {
        animator.SetBool("atk", true);

        yield return new WaitForSeconds(1f);

        for (float i = -4f; i <= 4f; i+=4f)
        {
            GameObject ball = Instantiate(ballPrefab, fireStart.position, Quaternion.identity, ballParent);
            float xTmp = Mathf.Abs(transform.position.x - player.transform.position.x) + i;
            ball.GetComponent<Rigidbody2D>().AddForce(ForceCalc(xTmp), ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.75f);
        animator.SetBool("atk", false);
        currentTime = 0f;
    }






    public void TakeDamage(int damage)
    {
        if (toAttack)
        {
            health -= damage;
            bossCamera.SetSliderValue(health);

            if (health <= 2 * maxHealth / 3f)
                animatorState = 1;
            if (health <= maxHealth / 3f)
                animatorState = 2;
            if (health <= 0)
                StartCoroutine(Dying());
        }
    }



    IEnumerator Dying()
    {
        animator.SetBool("atk", false);
        animator.SetBool("dead", true);
        toAttack = false;
        bossCamera.BossFightEndedHud();

        yield return new WaitForSeconds(4f);

        bossCamera.BossFightEndedCamera();
        Destroy(gameObject);
    }




    public void StartAttacking()
    {
        toAttack = true;
    }



}
