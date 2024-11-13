using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public static Camera mainCamera;
    [FormerlySerializedAs("speed")] public float turnSpeed;
    public float forwardSpeed;
    public float accelRate;
    public float decelRate;
    public float minForwardSpeed;
    public float maxForwardSpeed;
    //public float bankAmount;
    
    public ObservedVector3 PlayerDirection;
    public ObservedTransform PlayerTransform;
    private Vector2 moveInput;
    public bool invertControls;
    private void Awake()
    {
        PlayerDirection.SetReference(transform.forward);
        PlayerTransform.SetReference(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal")*Time.deltaTime*turnSpeed;
        //float bankLerp = Mathf.LerpAngle(-bankAmount, bankAmount, (moveInput.x + 1) / 2);
        float invertControlMultiplier = invertControls ? -1 : 1;
        moveInput.y = Input.GetAxis("Vertical") * Time.deltaTime*turnSpeed*invertControlMultiplier;
        //transform.position += speed * Time.deltaTime * move;
        transform.forward = transform.forward + transform.right * moveInput.x + transform.up * moveInput.y;
        PlayerDirection.SetReference(transform.forward);
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        
        
        //Ship speed controls
        if (Input.GetButton("Accelerate")) forwardSpeed += accelRate * Time.deltaTime;
        if (Input.GetButton("Decelerate")) forwardSpeed -= decelRate * Time.deltaTime;
        // Keep ship speed within bounds
        forwardSpeed = Mathf.Clamp(forwardSpeed, minForwardSpeed, maxForwardSpeed);
    }

    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }
}

