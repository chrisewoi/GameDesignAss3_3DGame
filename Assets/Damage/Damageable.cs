using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Damage
{
    [RequireComponent(typeof(Rigidbody))]
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private float healthMax;
        [SerializeField] private UnityEvent onHealthZero;

        private float healthCurrent;
        public GameObject destroyedPrefab;

        public float pointsValue;

        private bool isDead;
        // Start is called before the first frame update
        void Start()
        {
            healthCurrent = healthMax;
            isDead = false;
        }

        public void TakeDamage(float amount)
        {
            if (isDead) return;
        
            Debug.Log($"The agent {name} took {amount} damage!");
            healthCurrent -= amount;

            if (healthCurrent <= 0) HealthZero();
        
        }

        public void RestoreHealth(float amount)
        {
            isDead = false;
            healthCurrent += amount;
            if (healthCurrent > healthMax)
            {
                healthCurrent = healthMax;
            }
        }

        private void HealthZero()
        {
            isDead = true;
            healthCurrent = 0;
            UIDisplay.Score += pointsValue;
            GameObject ps = Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
            ps.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<Rigidbody>().velocity);
            //onHealthZero.Invoke();
            Destroy(gameObject);
            //OnHealthZeroDelay(0.1f);
            Debug.Log($"The agent {name} has died!");
        }

        private IEnumerator OnHealthZeroDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            onHealthZero.Invoke();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckForDamage(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckForDamage(other.gameObject);
        }

        private void OnParticleCollision(GameObject other)
        {
            CheckForDamage(other);
        }

        private void CheckForDamage(GameObject possibleSource)
        {
            if (possibleSource.TryGetComponent<DamageSource>(out DamageSource damageSource))
            {
                if (possibleSource.CompareTag(tag))
                {
                    return;
                }
                //TakeDamage(damageSource.GetDamage());
            }
        }

        private void FixedUpdate()
        {
            
        }
    }
}
