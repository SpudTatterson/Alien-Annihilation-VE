using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] GameObject rocket;
    [SerializeField] Transform rocketSpawnPoint;
    [SerializeField] float fireRate = 2f;
    [SerializeField] float TimeToLockToTarget;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera cam;
    

    //Camera cam;

    RaycastHit hit;
    Vector3 target;
    float shotTimer = 0f;
    float timer;
    // Start is called before the first frame update
    void Awake()
    {
        cam = FindObjectOfType<CameraController>().GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {  
        if(cam == null) cam = FindObjectOfType<CameraController>().GetComponent<Camera>();
        Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(raycast, out hit,Mathf.Infinity ,layerMask))
        {
            target = LookForTarget();
        }
        shotTimer -= Time.deltaTime;
        if(Input.GetButtonDown("Fire1") && shotTimer <= 0)
        {
            SpawnMissile();
        }
    }

    private Vector3 LookForTarget()
    {
        Health health;
        if((health = Utility.GetComponentInRadius<Health>(hit.point, 5f)) != null)
        {
            return health.transform.position;
        }
        else return hit.point;
        
    }

    private void StartTimer()
    {
        timer = TimeToLockToTarget;
    }

    void SpawnMissile()
    {
        GameObject tempRocketGameObject = Instantiate(rocket, rocketSpawnPoint.position, rocketSpawnPoint.rotation, null);
        Rocket tempRocket = tempRocketGameObject.GetComponent<Rocket>();
        tempRocket.target = target;
        shotTimer = fireRate;
    }
}
