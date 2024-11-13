using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipGraphic : MonoBehaviour
{
    public ObservedVector3 ObservedPlayerDirection;
    public ObservedTransform ObservedPlayerTransform;
    public ObservedTransform ShipTransform;
    public float smoothTime;
    private Vector3 v;

    public PlayerMovement playerMovement;
    public Vector2 moveInput;



    private void Awake()
    {
        ShipTransform.SetReference(transform);
        //playerMovement = FindObjectOfType<PlayerMovement>();

    }
    private void OnEnable()
    {
        ObservedPlayerDirection.Register(PlayerDirection_OnSetReference);
        ShipTransform.Register(ShipTransform_OnSetReference);
        ObservedPlayerTransform.Register(ObservedPlayerTransform_OnSetReference);
    }
    private void OnDisable()
    {
        ObservedPlayerDirection.Unregister(PlayerDirection_OnSetReference);
        ShipTransform.Unregister(ShipTransform_OnSetReference);
        ObservedPlayerTransform.Unregister(ObservedPlayerTransform_OnSetReference);

    }

    private void Update()
    {
        Transform ship = ShipTransform.GetReference();
        Transform player = ObservedPlayerTransform.GetReference();
        moveInput.x = Input.GetAxis("Horizontal");
        
        
        transform.position = player.transform.position;
        ship = transform;
        ShipTransform.SetReference(transform);
    }

    public void PlayerDirection_OnSetReference(Vector3 previousReference, Vector3 newReference)
    {
        //currentBank = transform.eulerAngles.z;
        transform.forward = Vector3.SmoothDamp(transform.forward, newReference, ref v, smoothTime / playerMovement.GetForwardSpeed());
        Debug.Log("smoothTimeMod: " + smoothTime / playerMovement.GetForwardSpeed());
        
        //float bankLerp = Mathf.LerpAngle(-bankAmount, bankAmount, (moveInput.x + 1) / 2);
        //transform.Rotate(0f, 0f, zRotate);
        //Debug.Log("zRotate: " + zRotate);*/
    }

    public void ShipTransform_OnSetReference(Transform previousReference, Transform newReference)
    {

    }

    public void ObservedPlayerTransform_OnSetReference(Transform previousReference, Transform newReference)
    {
        transform.forward = Vector3.SmoothDamp(transform.forward, newReference.transform.forward, ref v, smoothTime / playerMovement.GetForwardSpeed());
        transform.Rotate(0,0,newReference.transform.eulerAngles.z);
        

        //transform.Rotate(transform.forward * moveInput.x * bankAmount, Space.Self);
        //transform.forward = Vector3.SmoothDamp(transform.forward, )
    }


}
