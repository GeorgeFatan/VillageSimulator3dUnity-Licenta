using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ThirdPersonController : MonoBehaviour
{
    //input fields
    private ThirdPersonActionsAsset playerActionAsset;
    private InputAction move; //referinta catre move action

    // movement fields

    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1.0f;
    [SerializeField]
    private float jumpForce = 5.0f;
    [SerializeField]
    private float maxSpeed = 5.0f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera playerCamera;


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionsAsset();

    }

    private void OnEnable()
    {
        playerActionAsset.Player.Jump.started += DoJump;
        move = playerActionAsset.Player.Move;
        playerActionAsset.Player.Enable();

    }

    private void OnDisable()
    {
        playerActionAsset.Player.Jump.started -= DoJump;
        playerActionAsset.Player.Disable();

    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection,ForceMode.Impulse);

        forceDirection = Vector3.zero;


        if(rb.velocity.y < 0)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime; //increase acceleration
            //as we fall fall = faster
        }
         
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        LookAt();
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
       Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext context)
    {
        if(IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;

        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if(Physics.Raycast(ray,out RaycastHit hit , 0.3f))
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}
