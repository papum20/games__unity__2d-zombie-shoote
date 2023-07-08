using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{

    public GameObject player;
    public Animator animator;

    public Slider healthSlider;
    public Slider speedSlider;
    public Slider jumpSlider;


    public int health = 100;
    public float speed = 0.1f;
    public float jump = 0f;

    float damageAnimationTime = 0.2f;
    float currentDamageTime = 0.2f;

    //GAME OVER
    public Canvas gameOverCanvas;
    bool canLose = true;




    void Start()
    {
        health = 100;
        speed = 0.1f;
        jump = 0f;

        player.GetComponent<PlayerMovement>().runSpeed = speed;
        player.GetComponent<PlayerMovement>().jumpHeight = jump;

        healthSlider.value = health;
        speedSlider.value = speed * 10;
        jumpSlider.value = jump;

        animator.SetBool("damage", false);

        gameOverCanvas.gameObject.SetActive(false);
    }




    private void Update()
    {
        if (animator.GetBool("damage") == true)
        {
            if (currentDamageTime >= damageAnimationTime)
                animator.SetBool("damage", false);
            else
                currentDamageTime += Time.deltaTime;
        }
        
    }






    public void TakeDamage(int damage)
    {
        if (currentDamageTime >= damageAnimationTime)
        {
            animator.SetBool("damage", true);
            currentDamageTime = 0f;

            health -= damage;
            if (health < 0)
                health = 0;
            Debug.Log(damage);

            healthSlider.value = health;

            if (health <= 0)
                GameOver();
        }
    }


    public void SetStats()
    {
        healthSlider.value = health;
        speedSlider.value = speed * 10;
        jumpSlider.value = jump;
    }





    void GameOver()
    {
        if(canLose)
        {
            gameOverCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }


}
