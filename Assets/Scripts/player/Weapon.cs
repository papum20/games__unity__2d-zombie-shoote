using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Weapon : MonoBehaviour
{

    public Transform player;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public Text ammoNumber;
    public Text grenadeNumber;

    public int weapon;
    public float fireRate;
    public int damage;
    public int ammo;

    float fireTouch;

    float currentTime = 0f;




    private void Start()
    {
        weapon = 0;
        damage = 20;
        ammo = 1;
        fireRate = 0.3f;

        fireTouch = SaveSystem.LoadOptions().touchControls;
    }




    void Update()
    {

        //SHOOTING TIMER

        currentTime += Time.deltaTime;




        //AMMO HUD

        if (weapon == 0)
            ammoNumber.text = "-";
        else
            ammoNumber.text = ammo.ToString();



        //IF OUT OF AMMO

        if(weapon != 0 && ammo <= 0)
        {
            ammo = 1;
            weapon = 0;
            damage = 20;
            fireRate = 0.3f;
            gameObject.GetComponent<PlayerMovement>().SetGunAnimation();
        }


    }



    private void FixedUpdate()
    {

        //SHOOTING
        if (Input.GetButton("Shoot"))
        {
            if (currentTime >= fireRate)
            {
                Shoot();
                currentTime = 0f;
            }
        }

        //TOUCH SHOOTING
        for(int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).position.x >= Camera.main.scaledPixelWidth / 2f && Input.GetTouch(i).position.y < Camera.main.scaledPixelHeight * fireTouch)
            {
                if (currentTime >= fireRate)
                {
                    Shoot();
                    currentTime = 0f;
                }
                break;
            }
        }


    }





    void Shoot()
    {
        GameObject tmp = Instantiate(bulletPrefab, firePoint.position, firePoint.transform.rotation, bulletParent);
        tmp.GetComponent<Bullet>().player = player;
        tmp.GetComponent<Bullet>().damage = damage;
        if (weapon != 0)
            ammo--;
    }




    public void SetWeapon(int id, int newDamage, float newFireRate, int newAmmo)
    {
        if (id == weapon)
            ammo += newAmmo;
        else
        {
            damage = newDamage;
            fireRate = newFireRate;
            ammo = newAmmo;
            weapon = id;
        }
            
    }





    public void SetTouch(float value)
    {
        fireTouch = value;
    }



}
