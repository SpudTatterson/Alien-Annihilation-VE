using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowable : MonoBehaviour
{
    [SerializeField] float baseDamage = 10f;
    [SerializeField] bool canGetStuck = true;

    public float damageModifier = 1f;


    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 10f);
        rb = GetComponent<Rigidbody>();      
    }

    private void OnCollisionEnter(Collision other) 
    {
        Health health = other.collider.GetComponentInParent<Health>();
        if(health != null)
        {
            Hit(health, other);
        }

        if(canGetStuck)
        {
            float rnd = Random.Range(1,4);
            if(rnd == 1)
            {
                transform.position = other.contacts[0].point;
                transform.rotation = Quaternion.LookRotation(transform.forward, other.contacts[0].normal);
                transform.parent = other.transform;
                Destroy(rb);
            }
        }
        
        Destroy(this);
    }
    void Hit(Health health, Collision collision)
    {
        health.TakeDamage(baseDamage * damageModifier);
        health.collision = collision;
    }
}
