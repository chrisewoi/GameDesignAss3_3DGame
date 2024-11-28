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
    public float topSpeedReached;
    
    // the speed of the ship between 0-1 relative to its min and max speed
    public float minMaxSpeed => Mathf.InverseLerp(minForwardSpeed, maxForwardSpeed, forwardSpeed);

    public ParticleSystem[] thrusters;
    public float minThrusterParticles;
    public float maxThrusterParticles;
    private float particleLifetime;
    
    private void Awake()
    {
        PlayerDirection.SetReference(transform.forward);
        PlayerTransform.SetReference(rotationTarget);
        Application.targetFrameRate = 120;
    }
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        particleLifetime = thrusters[0].main.startLifetime.constant;
    }
    void Update()
    {
        if (rotationTarget == null) return;
        if (forwardSpeed > topSpeedReached) topSpeedReached = forwardSpeed;
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
        
        ThrusterRate();
    }

    private void ThrusterRate()
    {
        // Particle thruster amount based on speed;
        foreach (ParticleSystem thruster in thrusters)
        {
            // Use the minMaxSpeed lerp to calculate the relative particle value between its own min and max
            float rate = Mathf.Lerp(minThrusterParticles, maxThrusterParticles, minMaxSpeed);
            var emission = thruster.emission;
            emission.rateOverTime = rate;

            var main = thruster.main;
            main.startLifetime = Mathf.Clamp(particleLifetime * minMaxSpeed, particleLifetime / 1.5f, particleLifetime);
        }
    }

    void RotationHandler()
    {
        angles += moveInput * Time.deltaTime;
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

