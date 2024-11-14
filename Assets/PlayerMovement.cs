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
    
    public float bankAmount;
    private float bankV;
    public float bankSmoothTime;
    public float currentBank;
    
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
        moveInput.x = Input.GetAxis("Horizontal")*turnSpeed;
        //float bankLerp = Mathf.LerpAngle(-bankAmount, bankAmount, (moveInput.x + 1) / 2);
        float invertControlMultiplier = invertControls ? -1 : 1;
        moveInput.y = Input.GetAxis("Vertical")*turnSpeed*invertControlMultiplier;
        //transform.position += speed * Time.deltaTime * move;
        transform.forward = transform.forward + transform.right * moveInput.x + transform.up * moveInput.y;
        //transform.Rotate(transform.forward, GetSmoothBankAngle());
        //transform.Rotate(0, 0, GetSmoothBankAngle());
        //PlayerDirection.SetReference(transform.forward);
        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
        transform.localEulerAngles = new(transform.localEulerAngles.x, transform.localEulerAngles.y, GetSmoothBankAngle());//Rotate(new Vector3(0f,0f,GetSmoothBankAngle()));
        PlayerTransform.SetReference(transform);

        
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
    public float GetSmoothBankAngle()
    {
        currentBank = transform.localEulerAngles.z;//transform.rotation.z;
        float alpha = moveInput.x;
        
        float bankLerp = Mathf.LerpAngle(bankAmount, -bankAmount, (moveInput.x + 1f) / 2f);
        Debug.Log("FUCK YOU: " + bankLerp);
        Debug.Log(("bankAmount: " + bankAmount));
        float smoothBankLerp = Mathf.SmoothDamp(currentBank, bankLerp, ref bankV, bankSmoothTime);  
        float zRotate = bankLerp;
        Debug.Log("zRotate: " + zRotate);
        Debug.Log("currentBank: " + currentBank);
        return zRotate;
    }
}

