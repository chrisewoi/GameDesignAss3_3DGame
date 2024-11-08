using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGraphic : MonoBehaviour
{
    public ObservedVector3 ObservedPlayerDirection;
    public ObservedTransform ShipTransform;
    public float smoothTime;
    private Vector3 v;

    private void Awake()
    {
        ShipTransform.SetReference(transform);
    }
    private void OnEnable()
    {
        ObservedPlayerDirection.Register(PlayerDirection_OnSetReference);
    }
    private void OnDisable()
    {
        ObservedPlayerDirection.Unregister(PlayerDirection_OnSetReference);
    }
    public void PlayerDirection_OnSetReference(Vector3 previousReference, Vector3 newReference)
    {
        transform.forward = Vector3.SmoothDamp(transform.forward, newReference, ref v, smoothTime);
    }
}
