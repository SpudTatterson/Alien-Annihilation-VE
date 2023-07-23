using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] float damageModifier = 2f;
    [SerializeField] float maxExplosionDamage = 10f;
    [SerializeField] float explosionForce = 20f;
    public float minLifeTimeToCollide = 0.1f;
    [SerializeField] GameObject VFX;
    [SerializeField] float explosionRadius = 30f;
    [SerializeField] ParticleSystem trail;

    Rigidbody rb;

    [HideInInspector] public float lifeTime = 0f;
    
    [HideInInspector]public bool canExplode = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
    }
    private void OnCollisionEnter(Collision other)
    {
        if(rb.velocity.magnitude < 1) return;
        if(((lifeTime < minLifeTimeToCollide) || !canExplode) ) return;
        //if(GetComponent<Bullet>() != null && other.collider.tag == "Player")  return;
        Health health = other.collider.GetComponentInParent<Health>();
        if(health != null)
        {
            Hit(health, other);
        }
        Explode();
        ExplosionVFX();
    }
    void Hit(Health health, Collision collision)
    {
        health.TakeDamage(rb.mass * damageModifier);
        health.collision = collision;
    }
    public void Explode()
    {
        
        List<Health> hitObjects = Utility.GetHealthObjectsInRadius(transform.position, explosionRadius);

        for (int i = 0; i < hitObjects.Count; i++)
        {
            float distanceDamageModifier = GetDistanceDamageModifier(hitObjects[i].gameObject);
            float calculatedDamage = distanceDamageModifier * maxExplosionDamage;
            Health health = hitObjects[i].GetComponent<Health>();
            health.TakeDamage(calculatedDamage);
            
        }
        List<GameObject> hitGameObjects = Utility.GetGameObjectsInRadius(transform.position, explosionRadius);
        for (int i = 0; i < hitGameObjects.Count; i++)
        {
            Rigidbody rb = hitGameObjects[i].GetComponent<Rigidbody>();
            if(rb)  rb.AddExplosionForce(explosionForce * GetDistanceDamageModifier(hitGameObjects[i]), transform.position, explosionRadius);
        }
    }

    float GetDistanceDamageModifier(GameObject gameObject)
    {
        float distance = Vector3.Distance(transform.position, gameObject.transform.position);
        float distanceDamageModifier = Mathf.InverseLerp(0, explosionRadius, distance);
        if(distanceDamageModifier == 1) distanceDamageModifier = 0;
        return 1 - distanceDamageModifier;
    }

    public void ExplosionVFX()
    {
        GameObject ExplosionVFX = Instantiate(VFX, transform.position, transform.rotation);
        if(trail) DetachParticles();   
        Destroy(gameObject);
    }
    public void DetachParticles()
    {
        // This splits the particle off so it doesn't get deleted with the parent
        trail.transform.parent = null;

        // this stops the particle from creating more bits
        trail.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        // This finds the particleAnimator associated with the emitter and then
        // sets it to automatically delete itself when it runs out of particles
        Destroy(trail.gameObject, 5);
    }
    // private void OnDrawGizmos() {
    //     Gizmos.DrawSphere(transform.position,explosionRadius);
    // }
}
