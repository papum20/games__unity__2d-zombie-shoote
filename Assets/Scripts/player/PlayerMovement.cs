using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{

    public GameObject player;
    public GameObject firePoint;
    public Animator animator;
    public Weapon weapon;
    public PlayerHealth playerHealth;
    public Text grenadeNumber;
    public GameObject grenade;
    public GameObject grenadeButton;

    public GameObject groundedPoint1;
    public GameObject groundedPoint2;
    float groundedRadius = 0.01f;
    public LayerMask whatIsGround;

    public float runSpeed;
    public float jumpHeight;

    public int maxHealth = 100;
    public float maxSpeed = 0.5f;
    public float maxJump = 6f;
    public float jumpTimer = 0.3f;
    bool canJump = true;
    float jumpTouch;

    float grenadeThrowPower;


    bool facingRight = true;
    Vector3 horizontalMovement;

    //float[] maxHeights = {4.01f, 4.96f, 5.11f, 5.99f, 6.98f, 7.11f, 8.16f, 9.02f, 10.12f, 11.07f}; //JUP LEVEL: 1500 - 2400





    private void Start()
    {

        grenadeButton.SetActive(false);

        OptionsSave optionsData = SaveSystem.LoadOptions();
        grenadeThrowPower = optionsData.GrenadeThrowDistance;

        jumpTouch = SaveSystem.LoadOptions().touchControls;
    }





    void Update()
    {

        //INPUT

        horizontalMovement.x = Input.GetAxisRaw("Horizontal");



        if (grenadeNumber.text != "0")
            grenadeButton.SetActive(true);
        
    }




    private void FixedUpdate()
    {

        //MOVEMENT
        animator.SetFloat("speed", Mathf.Abs(horizontalMovement.x));
        horizontalMovement.x *= runSpeed;

        if ((facingRight == true && horizontalMovement.x < 0) || (facingRight == false && horizontalMovement.x > 0))
            Flip();

        player.GetComponent<Transform>().Translate(horizontalMovement, Space.World);


        //JUMP

        if (GroundedFunction() && canJump)
        {
            if (Input.GetButton("Jump"))
            {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1500 + 100 * jumpHeight));
                StartCoroutine(JumpEnabler());
            }

            else
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).position.x >= Camera.main.scaledPixelWidth / 2f && Input.GetTouch(i).position.y > Camera.main.scaledPixelHeight * jumpTouch)
                    {
                        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1500 + 100 * jumpHeight));
                        StartCoroutine(JumpEnabler());
                        break;
                    }
                }
            }

        }

        //GRENADE INPUT

        if (Input.GetButtonDown("Grenade"))
            ThrowGrenade();


    }




    #region MOVEMENT

    public void MovementJoystick(float value)
    {
        horizontalMovement = new Vector3(value, 0, 0);
    }


    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }




    bool GroundedFunction()
    {
        RaycastHit2D[] hitInfo1 = Physics2D.RaycastAll(groundedPoint1.transform.position, Vector2.down, groundedRadius, whatIsGround);
        if (hitInfo1.Length != 0)
            return true;
        RaycastHit2D[] hitInfo2 = Physics2D.RaycastAll(groundedPoint2.transform.position, Vector2.down, groundedRadius, whatIsGround);
        if (hitInfo2.Length != 0)
        {
            return true;
        }
        return false;
    }




    IEnumerator JumpEnabler()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpTimer);
        canJump = true;
    }


    public void SetTouch(float value)
    {
        jumpTouch = value;
    }


    #endregion



    //IF FINDS COLLECTIBLE ITEM OR WEAPON

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "chest")
        {
            collision.gameObject.GetComponent<Chest>().Open();
        }

        else
        {
            Transform collectibleParent = collision.transform.parent;

            if (collectibleParent.tag == "weapon")
            {
                CollectibleWeapon collectible = collectibleParent.gameObject.GetComponent<CollectibleWeapon>();
                weapon.SetWeapon(collectible.identifier, collectible.damage, collectible.fireRate, collectible.ammo);                
                animator.SetInteger("weapon", collectible.identifier);

                Destroy(collectibleParent.gameObject);
            }

            else if(collectibleParent.tag == "grenade")
            {
                grenadeNumber.text = (Convert.ToDecimal(grenadeNumber.text) + 1).ToString();
                Destroy(collectibleParent.gameObject);
                grenadeButton.SetActive(true);
            }

            else if (collectibleParent.tag == "item")
            {
                int identifier = collectibleParent.GetComponent<CollectibleItem>().identifier;
                bool collected = true;

                if (identifier == 0)
                {
                    if (playerHealth.health == 100) collected = false;
                    else playerHealth.health = Mathf.Min(100, playerHealth.health + 20);
                }
                else if (identifier == 1)
                {
                    if (runSpeed == maxSpeed) collected = false;
                    else
                    {
                        runSpeed += 0.1f;
                        playerHealth.speed = runSpeed;
                    }
                }
                else if (identifier == 2)
                {
                    if (jumpHeight == maxJump) collected = false;
                    else
                    {
                        jumpHeight += 1;
                        playerHealth.jump = jumpHeight;
                    }
                }

                if (collected == true)
                {
                    playerHealth.SetStats();
                    Destroy(collectibleParent.gameObject);
                }
            }

        }


    }





    public void SetGunAnimation()
    {
        animator.SetInteger("weapon", 0);
    }






    public void ThrowGrenade()
    {
        if (grenadeNumber.text != "0")
        {
            GameObject tmp = Instantiate(grenade, transform.position, Quaternion.identity);
            Vector2 throwDirection = new Vector2(grenadeThrowPower * (facingRight ? 1 : -1), 2 * grenadeThrowPower);
            tmp.GetComponent<Rigidbody2D>().AddForce(throwDirection, ForceMode2D.Impulse);

            grenadeNumber.text = (Convert.ToDecimal(grenadeNumber.text) - 1).ToString();

            if (grenadeNumber.text == "0")
                grenadeButton.SetActive(false);
        }
    }

    public void ChangeGrenadeThrowDistance(float value)
    {
        grenadeThrowPower = value;
    }


}
