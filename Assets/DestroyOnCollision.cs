using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public GameObject Ship;
    public GameObject PS_Destroy;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter()
    {
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("powerup"))
        {
            Destroy(other.gameObject);
            Ship.GetComponent<ClickToShoot>().upgraded = true;
            return;
        }

        if (other.CompareTag("terrain"))
        {
            GameObject ps = Instantiate(PS_Destroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("terrain"))
        {
            GameObject ps = Instantiate(PS_Destroy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
