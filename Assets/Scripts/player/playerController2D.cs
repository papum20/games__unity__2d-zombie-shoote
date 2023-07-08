using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController2D : MonoBehaviour
{

    public CharacterController2D controller;

    float Horizontal;
    bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Horizontal = Input.GetAxisRaw("Horizontal") * 4000f;
        if (Input.GetButtonDown("Jump"))
            jump = true;

            

        controller.Move(Horizontal * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
