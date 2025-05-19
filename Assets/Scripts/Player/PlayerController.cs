using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;


    [Header("Look")]
    [SerializeField]
    private Transform cameraContainer;
    
    public float minXLook;
    public float maxXLook;
    private float camCurXRotate;
    public float lookSensitivity;
    private Vector2 mouseDelta;


    private Rigidbody rigidBody;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CamerLook();
    }
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right *curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidBody.velocity.y;

        rigidBody.velocity = dir;
    }

    void CamerLook()
    {
        camCurXRotate += mouseDelta.y * lookSensitivity;
        camCurXRotate = Mathf.Clamp(camCurXRotate, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRotate, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnrMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed/*context.phase == InputActionPhase.Started*/)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            rigidBody.AddForce(Vector2.up*jumpPower,ForceMode.Impulse);
        }
    }

    bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up*0.01f),Vector3.down)
        };
        for (int i = 0; i < rays.Length; ++i)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
}
