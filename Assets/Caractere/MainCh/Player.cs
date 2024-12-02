using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
    public bool walking;
    public Transform playerTrans;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerRigid.velocity = transform.forward * w_speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerRigid.velocity = -transform.forward * wb_speed * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("WalkingAnimation");
            playerAnim.ResetTrigger("IdleAnimation");
            walking = true;
            //steps1.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("WalkingAnimation");
            playerAnim.SetTrigger("IdleAnimation");
            walking = false;
            //steps1.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("WalkBack");
            playerAnim.ResetTrigger("IdleAnimation");
            //steps1.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("WalkBack");
            playerAnim.SetTrigger("IdleAnimation");
            //steps1.SetActive(false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
        }

        if (walking == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //steps1.SetActive(false);
                //steps2.SetActive(true);
                w_speed = w_speed + rn_speed;
                playerAnim.SetTrigger("RunAnimation");
                playerAnim.ResetTrigger("WalkingAnimation");
            }

        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //steps1.SetActive(true);
            //steps2.SetActive(false);
            w_speed = olw_speed;
            playerAnim.ResetTrigger("RunAnimation");
            playerAnim.SetTrigger("WalkingAnimation");
        }
    }
}
