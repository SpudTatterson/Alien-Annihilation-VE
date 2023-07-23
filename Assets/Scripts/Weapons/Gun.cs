using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject bullets;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject muzzleFlashVFX;
    [SerializeField] Transform muzzleFlashSpawnPoint;
    [SerializeField] float fireRate;
    [SerializeField] LayerMask layerMask;
    public bool aimAssist;
    [SerializeField] float aimAsistRadius = 2f;
    Camera cam;
    float timeSinceLastShot;
    [SerializeField] bool flipGun;
    RaycastHit hit;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(raycast, out hit,Mathf.Infinity ,layerMask))
        //Vector3 worldPosition 
        {
            Vector3 lookPoint;
            if(aimAssist && hit.collider.tag != "Enemey") lookPoint = LookForEnemy();
            else lookPoint = hit.point;

            transform.LookAt(lookPoint);
            bulletSpawnPoint.LookAt(lookPoint);
        }
        else{
            transform.rotation = cam.transform.rotation;
            bullets.transform.rotation = cam.transform.rotation;
        }
        if(flipGun)
        {
            Vector3 tempEuler = transform.eulerAngles;
            tempEuler.z -= 150;
            transform.eulerAngles = tempEuler;
        }
        //bulletSpawnPoint.LookAt(worldPosition);
        //Debug.DrawLine(bulletSpawnPoint.position, worldPosition);
        if (Input.GetButton("Fire1") && timeSinceLastShot >= fireRate)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    void Shoot()
    {
        Instantiate(bullets, bulletSpawnPoint.position , transform.rotation, null); 
        Instantiate(muzzleFlashVFX, muzzleFlashSpawnPoint.position, transform.rotation,transform);
        if(audioSource) ChangeAndPlayOneShot();
        
    }
    public void ReplaceBullets(GameObject newBullets)
    {
        bullets = newBullets;
    }
    Vector3 LookForEnemy()
    {
        List<AiAim> enemies = Utility.GetComponentsInRadius<AiAim>(hit.point, aimAsistRadius);
        if(enemies.Count != 0) return enemies[0].transform.position + enemies[0].transform.up * 2;
        else return hit.point;
    }
    void ChangeAndPlayOneShot()
    {
        //audioSource.pitch = Random.Range(0.9f, 1.1f); // Add random pitch variation
        //audioSource.volume = Random.Range(0.09f, 0.12f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
