using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 2f; // Adjust this value in the Unity editor to control rotation speed

    [SerializeField]
    private GameObject projectilePrefab; // Reference to the projectile prefab

    [SerializeField]
    private Transform launchPoint; // The point from which the projectile is launched

    [SerializeField]
    private float shotCooldown = 2f; // Cooldown duration between shots

    [SerializeField]
    private float interceptTime = 2f; // Time in seconds to intercept the player

    private GameObject player;
    private Rigidbody rb;
    private bool canShoot = true;
    //private float elapsedTime = 0f;

    private void Start()
    {
        player = FindObjectOfType<SaucerController>().gameObject;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;

        Vector3 targetDirection = PredictIntercept(playerPosition, playerVelocity, interceptTime);

        // Set the y component of the targetDirection to 0 to restrict rotation on the y-axis
        targetDirection.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // Smoothly rotate towards the target rotation only on the y-axis
        Quaternion yRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, yRotation, rotationSpeed * Time.deltaTime);


        if (CanShoot() && canShoot)
        {
            float launchForce = CalculateLaunchForce(targetDirection.magnitude);
            LaunchProjectile(targetDirection, launchForce);
            StartCoroutine(ShotCooldown());
        }
    }

    private bool CanShoot()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;

        Vector3 targetDirection = PredictIntercept(playerPosition, playerVelocity, interceptTime);

        // Calculate the dot product between the forward direction of the catapult and the target direction
        float dotProduct = Vector3.Dot(transform.forward.normalized, targetDirection.normalized);

        // Calculate the angle in degrees using the dot product
        float angleToPlayer = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;


        bool canShoot = angleToPlayer < 30f;

        return canShoot;
    }


    private void LaunchProjectile(Vector3 targetDirection, float launchForce)
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.LookRotation(targetDirection));
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(targetDirection.normalized * launchForce, ForceMode.Impulse);
    }

    private IEnumerator ShotCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotCooldown);
        canShoot = true;
    }

    private Vector3 PredictIntercept(Vector3 targetPosition, Vector3 targetVelocity, float time)
    {
        Vector3 predictedPosition = targetPosition + targetVelocity * time;
        Vector3 interceptDirection = predictedPosition - transform.position;
        return interceptDirection;
    }

    private float CalculateLaunchForce(float distance)
    {
        // Customize this calculation based on your game's requirements
        // Example: The farther the player, the higher the launch force
        float launchForce = Mathf.Lerp(50f, 120f, distance / 100f);
        return launchForce;
    }
}
