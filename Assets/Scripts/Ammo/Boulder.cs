using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] float damageModifier = 2f;

    float lastInteracted;
    float hitCooldownTimer;
    float ESTIMATEDMAXVELOCITY = 100f;
    bool wasInteracted = false;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        lastInteracted += Time.deltaTime;
        hitCooldownTimer  -= Time.deltaTime;

        if(transform.position.y <= 0f)
        {
            Destroy(gameObject); 
        }
        if(lastInteracted > 60f && wasInteracted)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(hitCooldownTimer > 0) return;
        Health health = other.collider.GetComponentInParent<Health>();
        if(health != null)
        {
            Hit(health,other);        
        }  
    }
    void Hit(Health health, Collision collision)
    {
        health.collision = collision;
        
        float speedDamageModifer = Mathf.InverseLerp(0, ESTIMATEDMAXVELOCITY, rb.velocity.magnitude);
        health.TakeDamage(rb.mass * damageModifier * speedDamageModifer);
        if(health.gameObject.tag == "Player") return;
        // Vector3 oppsiteDirection = -(health.transform.position - transform.position);
        // Debug.DrawLine(transform.position, oppsiteDirection.normalized,Color.black,10f);
        // rb.AddForceAtPosition(oppsiteDirection.normalized  * rb.mass , transform.position,ForceMode.Impulse);
        hitCooldownTimer = 0.1f;
        
    }
    public void ResetLastInteracted()
    {
        lastInteracted = 0f;
        wasInteracted = true;
    }
}
