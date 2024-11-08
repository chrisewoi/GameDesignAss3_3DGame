using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Camera mainCamera;
    public float speed;
    public ObservedVector3 PlayerDirection;
    private Vector2 moveInput;
    public bool invertControls;
    private void Awake()
    {
        PlayerDirection.SetReference(transform.forward);
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal")*Time.deltaTime*speed;
        float invertControlMultiplier = invertControls ? -1 : 1;
        moveInput.y = Input.GetAxis("Vertical") * Time.deltaTime*speed*invertControlMultiplier;
        //transform.position += speed * Time.deltaTime * move;
        transform.forward = transform.forward + transform.right * moveInput.x + transform.up * moveInput.y;
        PlayerDirection.SetReference(transform.forward);
    }
}

