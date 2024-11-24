using System;
using System.Collections;
using System.Collections.Generic;
using Damage;
using UnityEngine;
using UnityEngine.Events;

public class ClickToShoot : MonoBehaviour
{
    public LineRenderer line;
    public UnityEvent click;
    public Transform LaserGun1;
    public Transform LaserGun2;
    public bool nextLaserGun;

    public GameObject PS_LaserHit;
    public float laserForce;
    public float laserDamage;





    [Tooltip("The physics layers to try to hit with this raycast")] [SerializeField]
    private LayerMask hitLayer;

    [Tooltip("The maximum distance this raycast can travel")] [SerializeField]
    private float maxDistance;

    [SerializeField] public Camera camera;
    
    // protected = like private, but child scripts can see
    // virtual = lets a child script override this function with its own version
    void Start()
    {
        //camera = FindObjectOfType<Camera>();
        line = GetComponentInChildren<LineRenderer>();
        line.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("clicked");
            Ray ray;
            // TryToHit() comes from our parent script (RaycastFromScreenCentre)
            // If we hit something, hit.collider will have a value. Else, hit.collider will be null
            RaycastHit hit = TryToHit(out ray);
        
            // If we did hit something...
            if (hit.collider)
            {
                GameObject test = Instantiate(PS_LaserHit);
                test.transform.position = hit.point;
                test.transform.forward = hit.normal;
                if (hit.rigidbody)
                {
                    //agent.TakeDamage(damageSource.GetDamage());
                    //hit.rigidbody.AddForce(hit.transform.forward * laserForce, ForceMode.Impulse);
                    //hit.rigidbody.AddExplosionForce(laserForce, hit.point, laserForce);
                  
                    hit.rigidbody.AddForceAtPosition(ray.direction * laserForce, hit.point,ForceMode.Impulse);
                    if (hit.rigidbody.TryGetComponent<Damageable>(out Damageable damageable))
                    {
                        damageable.TakeDamage(laserDamage);
                    }
                   // hit.rigidbody.AddTorque(hit.normal, ForceMode.Impulse);
                }
            }
            Play(hit.point);
        }
    }

    public Vector3 GetNextLaserGunPos()
    {
        nextLaserGun = !nextLaserGun;
        if (nextLaserGun) return LaserGun1.position;
        return LaserGun2.position;
    }

    public void Play(Vector3 hitPosition)
    {
        //StopCoroutine("StopAfterDelay");
        line.enabled = true;
        line.SetPosition(0, transform.InverseTransformPoint(GetNextLaserGunPos()));
        line.SetPosition(1, transform.InverseTransformPoint(hitPosition));
        StartCoroutine("StopAfterDelay");
    }

    private IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        line.enabled = false;
    }

    public RaycastHit TryToHit(out Ray outRay)
    {
        // a struct cannot be "null", so we initialise an empty struct instead
        RaycastHit hit = new RaycastHit();

        Ray ray = camera.ScreenPointToRay(Input
            .mousePosition); //camera.ScreenPointToRay(new Vector3(camera.pixelWidth, camera.pixelHeight) * 0.5f);
        outRay = ray;

        if (Physics.Raycast(ray, out hit, maxDistance, hitLayer))
        {
            return hit;
        }

        // If we hit nothing, record the furthest point we *could* have hit
        hit.point = ray.origin + ray.direction * maxDistance;

        // then we can return the otherwise empty hit
        return hit;
    }
}
