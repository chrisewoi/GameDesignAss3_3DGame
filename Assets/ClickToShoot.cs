using System;
using System.Collections;
using System.Collections.Generic;
using Damage;
using UnityEngine;
using UnityEngine.Events;

public class ClickToShoot : MonoBehaviour
{
    public LineRenderer line;
    public LineRenderer upgradedLine;
    public UnityEvent click;
    public Transform LaserGun1;
    public Transform LaserGun2;
    public Transform UpgradedLaserGun;
    public bool nextLaserGun;

    public GameObject PS_LaserHit;
    public float laserForce;
    public float laserDamage;

    public bool upgraded;
    public float powerupCooldown;
    public float powerupActive;





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
        upgradedLine.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("clicked");
            Ray ray;
            // TryToHit() comes from our parent script (RaycastFromScreenCentre)
            // If we hit something, hit.collider will have a value. Else, hit.collider will be null
            RaycastHit hit = TryToHit(out ray);
        
            // If we did hit something...
            if (hit.collider)
            {
                if (hit.collider.CompareTag("ship")) return;
                GameObject laserHit = Instantiate(PS_LaserHit);
                laserHit.transform.position = hit.point;
                laserHit.transform.forward = hit.normal;
                
                if (hit.rigidbody)
                {
                    //agent.TakeDamage(damageSource.GetDamage());
                    //hit.rigidbody.AddForce(hit.transform.forward * laserForce, ForceMode.Impulse);
                    //hit.rigidbody.AddExplosionForce(laserForce, hit.point, laserForce);

                    float powerupForceMulti = 1f;
                    if (powerupActive > 0) powerupForceMulti = 10f;
                    hit.rigidbody.AddForceAtPosition(ray.direction * (laserForce * powerupForceMulti), hit.point,ForceMode.Impulse);
                    if (hit.rigidbody.TryGetComponent<Damageable>(out Damageable damageable))
                    {
                        float powerupDmgMulti = 1f;
                        if (powerupActive > 0) powerupDmgMulti = 10f;
                        damageable.TakeDamage(laserDamage * powerupDmgMulti);
                    } 
                    // hit.rigidbody.AddTorque(hit.normal, ForceMode.Impulse);
                }
            }
            Play(hit.point);
        }
        
        // if a powerup has been picked up, set the cooldown
        if (upgraded)
        {
            upgraded = false;
            powerupActive = powerupCooldown;
        }

        powerupActive -= Time.deltaTime;
        powerupActive = Mathf.Clamp(powerupActive, 0, powerupCooldown);
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
        if (powerupActive > 0)
        {
            line.enabled = false;
            upgradedLine.enabled = true;
            upgradedLine.SetPosition(0, transform.InverseTransformPoint((UpgradedLaserGun.position)));
            upgradedLine.SetPosition(1, transform.InverseTransformPoint(hitPosition));
        }
        line.SetPosition(0, transform.InverseTransformPoint(GetNextLaserGunPos()));
        line.SetPosition(1, transform.InverseTransformPoint(hitPosition));
       
        StartCoroutine("StopAfterDelay");
    }

    private IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(0.1f + (powerupActive > 0f ? 0.1f : 0));
        line.enabled = false;
        upgradedLine.enabled = false;
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

    /*public LineRenderer ChooseLine()
    {
        if (powerupActive > 0)
        {
            return upgradedLine;
        }
        return 
    }*/
}
