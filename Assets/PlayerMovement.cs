using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Camera mainCamera;
    public float turnSpeed;
    public ObservedVector3 PlayerDirection;
    public ObservedTransform PlayerTransform;
    private Vector2 moveInput;
    public Vector2 pitchLimits;
    private Vector2 angles;
    public bool invertControls;
    public Transform rotationTarget;
    public float rollTime;
    public AnimationCurve rollCurve;
    private float roll_v;
    public float rollSmoothTime;
    public float rollResetTime;
    public float rollAmplitude;

    public float forwardSpeed;
    public float accelRate;
    public float decelRate;
    public float minForwardSpeed;
    public float maxForwardSpeed;
    private void Awake()
    {
        PlayerDirection.SetReference(transform.forward);
        PlayerTransform.SetReference(rotationTarget);
    }
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal") * turnSpeed;
        float invertControlMultiplier = invertControls ? -1 : 1;
        moveInput.y = Input.GetAxis("Vertical") * turnSpeed * invertControlMultiplier;

        float moddedRollSmoothTime = rollSmoothTime;
        if (!Input.GetButton("Horizontal"))
        {
            moddedRollSmoothTime = rollResetTime;
        }
        rollTime = Mathf.SmoothDamp(rollTime, Input.GetAxis("Horizontal"), ref roll_v, moddedRollSmoothTime);

        rollTime = Mathf.Clamp(rollTime, -1f,1f);
        
        RotationHandler();

        if (Input.GetButton("Accelerate"))
        {
            forwardSpeed += accelRate * Time.deltaTime;
        }
        if(Input.GetButton("Decelerate"))
        {
            forwardSpeed -= decelRate * Time.deltaTime;
        }

        forwardSpeed = Mathf.Clamp(forwardSpeed, minForwardSpeed, maxForwardSpeed);
        transform.position += forwardSpeed * Time.deltaTime * rotationTarget.transform.forward;
        rotationTarget.position = transform.position;
        PlayerTransform.SetReference(transform);
        
    }

    void RotationHandler()
    {
        angles += moveInput;
        angles.y = Mathf.Clamp(angles.y, pitchLimits.x, pitchLimits.y);
        Quaternion yawRot = Quaternion.AngleAxis(angles.x, Vector3.up);
        Quaternion pitchRot = Quaternion.AngleAxis(angles.y, Vector3.right);
        Quaternion rollRot = Quaternion.AngleAxis(-rollTime*rollAmplitude, Vector3.forward);
        //Quaternion rollRot = Quaternion.AngleAxis(rollCurve.Evaluate(rollTime)*rollAmplitude, Vector3.forward); 
        rotationTarget.localRotation = yawRot * pitchRot * rollRot;
    }

    public float GetSpeed()
    {
        return forwardSpeed;
    }

    /*public List<Depression> emptyWillToLive = new ();
    public List<List<Depression>> nestedEmptyWillToLive = new();

    public void DestroyDepression()
    { 
        Depression depression = FindObjectOfType<Depression>();
        if (depression == null)
        {
            foreach (var emptyWillToLiveInstance in nestedEmptyWillToLive)
            {
                foreach (var depressionMaybe in emptyWillToLiveInstance)
                {
                    depression = FindObjectOfType<Depression>();
                    Destroy(depression);
                }
            }
        }
        else
        {
            Destroy(depression);
        }
        throw new ArgumentException("No depression present.. but still depress");
    }
}

public class Depression : MonoBehaviour
{*/


}

