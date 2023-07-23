using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float Force;
    [SerializeField] float baseDamage = 10f;

    [SerializeField] float timeToUnhide = 0.1f;
    public float damageModifier = 1f;
    float timer;

    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * Force, ForceMode.Impulse);
        Destroy(gameObject, 2f);      
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        timer += Time.deltaTime;
        if(timer < timeToUnhide)
        {
            renderer.enabled = false;
        }
        else
        {           
            renderer.enabled = true;
        }

    }
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player") return;
        Health health = other.collider.GetComponentInParent<Health>();
        if(health != null)
        {
            Hit(health, other);
        }
        if(other.gameObject.tag != "Player")
            Destroy(gameObject);
    }
    void Hit(Health health, Collision collision)
    {
        health.TakeDamage(baseDamage * damageModifier);
        health.collision = collision;
    }
}
