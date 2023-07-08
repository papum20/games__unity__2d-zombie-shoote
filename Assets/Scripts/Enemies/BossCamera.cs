using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BossCamera : MonoBehaviour
{

    public Transform player;
    public Camera dynamicCamera;
    public Camera staticCamera;
    public Slider bossHealthSlider;
    public GameObject bossHealthIcon;
    public GameObject invisibleWall;

    bool bossFightStarted = false;

    GameObject ownInvisibleWall;





    void Update()
    {
        if(bossFightStarted == false && Mathf.Abs(transform.position.x - player.position.x) <= 14)
        {
            //CAMERA
            staticCamera.gameObject.SetActive(true);
            dynamicCamera.gameObject.SetActive(false);
            staticCamera.tag = "MainCamera";
            dynamicCamera.tag = "Untagged";
            staticCamera.transform.position = new Vector3(transform.position.x - 14f, staticCamera.transform.position.y, -10f);
            bossFightStarted = true;
            if (gameObject.tag == "plantBoss")
                GetComponent<PlantBoss>().StartAttacking();
            else if (gameObject.tag == "whiteBoss")
                GetComponent<WhiteBoss>().StartAttacking();
            ownInvisibleWall = Instantiate(invisibleWall, new Vector2(transform.position.x - 32.5f, 12), Quaternion.identity);

            //HUD
            bossHealthSlider.gameObject.SetActive(true);
            bossHealthIcon.SetActive(true);
        }
    }



    public void BossFightEndedCamera()
    {
        dynamicCamera.gameObject.SetActive(true);
        staticCamera.gameObject.SetActive(false);
        dynamicCamera.tag = "MainCamera";
        staticCamera.tag = "Untagged";
        Destroy(ownInvisibleWall);
    }


    public void BossFightEndedHud()
    {
        bossHealthSlider.gameObject.SetActive(false);
        bossHealthIcon.SetActive(false);
    }




    public void SetSliderMaxValue(int value)
    {
        bossHealthSlider.maxValue = value;
        bossHealthSlider.value = value;
    }


    public void SetSliderValue(int value)
    {
        bossHealthSlider.value = value;
    }


}
