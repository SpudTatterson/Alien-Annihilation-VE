using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] float explosionForce = 50f;
    [SerializeField] float explosionRadius = 10f;
    [SerializeField] bool destroy = true;
    [HideInInspector] public Collision collision;

    
    Rigidbody[] parts;
    // Start is called before the first frame update
    void Start()
    {
        parts = GetComponentsInChildren<Rigidbody>();
        ExplodeDestructible();
        if (destroy)   Destroy(gameObject, 20f);
    }
    public void ExplodeDestructible()
        {
            if(collision != null)
            {
                foreach (var rb in parts)
                {
                    if (rb)
                    {
                        rb.AddExplosionForce(Random.Range(0.1f, 0.5f) * explosionForce,
                                            collision.contacts[0].point, explosionRadius * Random.Range(0.5f, 2f),
                                            0.1f,
                                            ForceMode.Impulse);
                    }
                }
            }
        }

}
