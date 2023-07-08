using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyJoystick : MonoBehaviour
{
    public GameObject player;

    float radius = 100;
    bool clicking = false;
    Vector3 startPosition;

    [SerializeField] int movementSensibility = 70;




    void Start()
    {
        startPosition = transform.position;

        OptionsSave optionsData = SaveSystem.LoadOptions();
        radius = radius * Screen.currentResolution.height / 720;  //optionsData.resHeight
        movementSensibility = optionsData.sensibility * Screen.currentResolution.height / 720;    //optionsData.resHeight
    }


    void Update()
    {
        clicking = false;

        //IF TOUCHING IT

        for(int i = 0; i < Input.touchCount; i++)
        {
            Vector2 touchPosition = Input.GetTouch(i).position;
            Vector2 PositionDifference = new Vector2(touchPosition.x - startPosition.x, touchPosition.y - startPosition.y); //DISTANCE TOUCH-JOYSTICK
            float distance = Mathf.Sqrt(Mathf.Pow(PositionDifference.x, 2) + Mathf.Pow(PositionDifference.y, 2));

            /*Debug.Log(touchPosition);
            Debug.Log(startPosition);
            Debug.Log(PositionDifference);
            Debug.Log(distance);*/

            if (touchPosition.x <= Camera.main.scaledPixelWidth / 2f)
            {
                if (distance <= radius)
                {
                    transform.position = touchPosition;
                    clicking = true;

                    if (Mathf.Abs(PositionDifference.x) >= movementSensibility)
                        player.GetComponent<PlayerMovement>().MovementJoystick(PositionDifference.x / radius);

                    break;
                }
                else if (transform.position != startPosition)
                {
                    transform.position = new Vector3(startPosition.x + radius * (PositionDifference.x / distance), startPosition.y + radius * (PositionDifference.y / distance));   //TRIGONOMETRY
                    clicking = true;

                    player.GetComponent<PlayerMovement>().MovementJoystick(PositionDifference.x / Mathf.Abs(PositionDifference.x));

                    break;
                }


            }


        }


        if (clicking == false)
            transform.position = startPosition;


    }





    public void SetSensibility(int value)
    {
        movementSensibility = value * Screen.currentResolution.height / 720;
    }

    public void SetWithResolution(int currentHeight, int lastHeight)
    {
        movementSensibility *= currentHeight / lastHeight;
        radius *= currentHeight / lastHeight;
    }



}
