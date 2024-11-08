using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public ObservedTransform ObservedShipTransform;
    Vector3 v_pos;
    Vector3 v_forward;
    public float smoothTime;
    public Vector3 cameraOffset;
    public Vector3 finalOffset;
    private Transform ship => ObservedShipTransform.GetReference();

    private void Update()
    {
        finalOffset = cameraOffset;
        finalOffset = ship.right * cameraOffset.x + ship.up * cameraOffset.y + ship.forward * cameraOffset.z;
        transform.position = Vector3.SmoothDamp(transform.position, ship.position + finalOffset, ref v_pos, smoothTime);
        transform.forward = Vector3.SmoothDamp(transform.forward, ship.forward, ref v_forward, smoothTime);
    }
}
