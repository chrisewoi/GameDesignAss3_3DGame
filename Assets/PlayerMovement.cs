using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Camera mainCamera;
    public Rigidbody rb;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3( Input.GetAxis("Horizontal") , 0 , Input.GetAxis("Vertical") );
        transform.position +=  speed * Time.deltaTime * move;
        
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis(“Mouse X”) * sensitivityX;

            rotationY += Input.GetAxis(“Mouse Y”) * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis(“Mouse X”) * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis(“Mouse Y”) * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
}
