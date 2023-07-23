using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour
{
    [SerializeField] float launchForce = 50f;
    [SerializeField] float followForce = 50f;
    [SerializeField] float timeToFollowTarget = 0.5f;
    
    [HideInInspector] public Vector3 target;
    float timer = 0f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * launchForce * rb.mass, ForceMode.Impulse);
        //target = Utility.GetComponentInRadius<AiAim>(transform.position, 1000).transform;
    }

    // Update is called once per frame
    void Update()
    {
        // if(rb.velocity.magnitude < 2f) 
        // {
        //     Explosive explosive = GetComponent<Explosive>();
        //     explosive.Explode();
        //     explosive.ExplosionVFX();
        // }
        timer += Time.deltaTime;
        if(timer < timeToFollowTarget)
        {
            return;
        } 
        
        Vector3 dir = target - transform.position;
        rb.AddForce(dir * followForce, ForceMode.Force);
    }

    
}
