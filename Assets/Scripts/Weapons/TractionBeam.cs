using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractionBeam : MonoBehaviour
{
    [SerializeField] Transform BeamLocation;
    [SerializeField] float maxDistance = 1000;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask allLayers;
    [SerializeField] float maxWeight = 50f;
    [SerializeField] float throwingPower = 100f;
    [SerializeField] AudioSource audioSource;


    float heldObjectWeight;
    GameObject heldObject;
    public bool isHoldingObject = false;

    Rigidbody rb;
    Camera cam;
    SaucerController saucerController;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        saucerController = GetComponent<SaucerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPosition = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.y));
        Vector3 direction = (worldPosition - cam.transform.position);

        //Debug.DrawRay(cam.transform.position, direction);
        if(isHoldingObject && heldObject == null)
        {
            isHoldingObject = false;
        }
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position , direction, out hit, maxDistance, allLayers, QueryTriggerInteraction.Ignore) && heldObject == null )
        {
            if (Input.GetButton("Fire2") && !isHoldingObject)
            {
                SetBeamLocations(hit.point);
                if (hit.collider.gameObject.layer == 6 && hit.rigidbody.mass <= maxWeight)
                {
                    if (!isHoldingObject)
                    {
                        Debug.Log("Picked up");
                        heldObject = GetHeldObject(hit);
                        PickUpObject(hit);
                        return;
                    }
                }
                else
                {
                    Debug.Log("object too heavy");
                    return;
                }
            }                   
        }

    
        if(Input.GetButtonDown("Fire2") && isHoldingObject)
        {
            ReleaseObject(heldObject);
            ShootObject(heldObject);
        }
        if(heldObject != null && isHoldingObject)
        {
            SetBeamLocations(heldObject.transform.position);
            Boulder boulder;
            if(boulder = TryToGetBoulder()) boulder.ResetLastInteracted();
        }
        if(heldObject == null)
        {
            SetBeamLocations(transform.position);
            lineRenderer.enabled = false;
            audioSource.enabled = false;
        }
    }


    void PickUpObject(RaycastHit hit)
    {
        // Do projectile Specific things
        Explosive explosive;
        if(explosive = TryToGetExplosive()) explosive.canExplode = false;
        Health health;
        if(health = TryToGetHealth()) health.canGetFallDamage = false;
        
        hit.rigidbody.isKinematic = false;
        
        // Move the held object to a new position below the player over time
        StartCoroutine(MoveObjectSmoothly(heldObject, 2f));

        heldObject.transform.position = transform.position + Vector3.down * 20f + transform.forward * 40;
        // Add a fixed joint to connect the object to the player
        var joint = heldObject.AddComponent<FixedJoint>();
        joint.connectedBody = rb;
        joint.breakForce = Mathf.Infinity;

        // Set the object's mass to zero so it doesn't affect physics
        heldObjectWeight = hit.rigidbody.mass;
        hit.rigidbody.mass = 0f;

    }

    Explosive TryToGetExplosive()
    {
        Explosive explosive = heldObject.GetComponent<Explosive>();
        return explosive;
    }
    Boulder TryToGetBoulder()
    {
        Boulder boulder = heldObject.GetComponent<Boulder>();
        return boulder;
    }
    Objective TryToGetObjective()
    {
        Objective objective = heldObject.GetComponent<Objective>();
        return objective;
    }
    Health TryToGetHealth()
    {
        Health health = heldObject.GetComponent<Health>();
        return health;
    }

    IEnumerator MoveObjectSmoothly(GameObject obj, float duration)
    {
        Vector3 startingPosition = obj.transform.position;
        float elapsedTime = 0;
        Vector3 targetPosition;

        while (elapsedTime < duration)
        {
            targetPosition = transform.position + Vector3.down * 20f + transform.forward * 40;
            obj.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            SetBeamLocations(obj.transform.position);
            yield return null;
        }
        Objective objective = TryToGetObjective();
        if(objective) 
        {
            objective.PickUp();
            Destroy(objective.gameObject);
            this.heldObject = null;
            isHoldingObject = false;
            yield break;
        }
        obj.transform.position = transform.position + Vector3.down * 20f + transform.forward * 40;
        isHoldingObject = true;
    }
    void ReleaseObject(GameObject heldObject)
    {
        if (this.heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            rb.mass = heldObjectWeight;
            Explosive explosive;
            if (explosive = TryToGetExplosive()) explosive.canExplode = true;
            Health health;
            if(health = TryToGetHealth()) health.canGetFallDamage = true;
            Destroy(this.heldObject.GetComponent<FixedJoint>());          
        }
    }
    private void ShootObject(GameObject heldObject)
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.y));
        Vector3 direction = mouseWorldPosition - BeamLocation.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(direction.normalized * rb.mass * throwingPower, ForceMode.Impulse);
        this.heldObject = null;
        Invoke("SetIsHoldingObjectFalse", 1f);
    }

    private void SetIsHoldingObjectFalse()
    {
        isHoldingObject = false;
    }

    GameObject GetHeldObject(RaycastHit hit)
    {
        GameObject obj;
        obj = hit.collider.GetComponentInParent<Rigidbody>().gameObject;
        return obj;
    }
    void SetBeamLocations(Vector3 targetPosition)
    {
        if(lineRenderer.enabled == false)
        {
            lineRenderer.enabled = true;
            audioSource.enabled = true;
        }
        lineRenderer.SetPosition(0, BeamLocation.position + BeamLocation.up / 2);
        lineRenderer.SetPosition(1, targetPosition);
    }
    public void SetMaxWeight(float maxWeight)
    {
        this.maxWeight = maxWeight;
    }
    public void SetMaxDistance(float maxDistance)
    {
        this.maxDistance = maxDistance;
    }
}
