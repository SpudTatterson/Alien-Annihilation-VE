using UnityEngine;

public class AiAim : MonoBehaviour
{
     
    
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject heldProjectile;

    [SerializeField] float launchForce = 10f;
    [SerializeField] float detectionRadius = 100f;
    [SerializeField] Vector2 shotCooldown = new Vector2(5 ,7);
    [SerializeField] float rotationSpeed = 40f;
    [SerializeField] bool multipleProjectile;
    [SerializeField] bool useDistanceModifier;

    float shotTimer;
    Vector3 interceptDirection;
    Quaternion yRotation;
    bool canShoot = false;

    Rigidbody playerRigidbody;
    Transform player;

    void Start()
    {
        // projectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        // heldProjectile = transform.Find("HeldProjectileGFX").gameObject;
        player = FindObjectOfType<SaucerController>().gameObject.transform;
        playerRigidbody = player.gameObject.GetComponent<Rigidbody>();
        shotTimer = shotCooldown.x;
        // GameObject staticProjectile = Instantiate(projectile, heldProjectile.transform.position, heldProjectile.transform.rotation, heldProjectile.transform);
        // Rigidbody staticProjectileRB = staticProjectile.GetComponent<Rigidbody>();
        // if(staticProjectileRB) staticProjectileRB.isKinematic = true;
        
    }

    void Update()
    {
        Quaternion targetRotation = Quaternion.identity;
        if (IsPlayerInRange() && HasLineOfSightToPlayer())
        {
            Vector3 playerPosition = player.position;
            Vector3 playerVelocity = playerRigidbody.velocity;
            Vector3 predictedPosition;
            

            float time = CalculateTimeToTarget(playerPosition);

            if(useDistanceModifier) predictedPosition = (playerPosition - player.up * DistanceModifier()) + playerVelocity * time;
            else predictedPosition = playerPosition + playerVelocity * time;

            projectileSpawnPoint.LookAt(predictedPosition);

            interceptDirection = predictedPosition - transform.position;
            
            if(canShoot && InCorrectRotation(yRotation))
            {
                if(multipleProjectile) LaunchMultipleProjectile(interceptDirection);
                else LaunchProjectile(interceptDirection);

                StartCooldown();
            }
            else CountdownCooldown();
        }
        
        if(HasLineOfSightToPlayer())
        {
            targetRotation = Quaternion.LookRotation(interceptDirection);
        }
        

        // Smoothly rotate towards the target rotation only on the y-axis
        yRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, yRotation, rotationSpeed * Time.deltaTime);

        
    }

    void StartCooldown()
    {
        canShoot = false;
        shotTimer = Random.Range(shotCooldown.x, shotCooldown.y);
        heldProjectile.SetActive(false);
    }

    void CountdownCooldown()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0f)
        {
            heldProjectile.SetActive(true);
            canShoot = true;
        }
    }
    bool IsPlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= detectionRadius;
    }
    bool HasLineOfSightToPlayer()
    {
        Vector3 direction = player.position - projectileSpawnPoint.position;

        RaycastHit hit;
        if (Physics.Raycast(projectileSpawnPoint.position, direction, out hit))
        { 
            // Check if the raycast hit something other than the player
            if (hit.collider.tag != "Player")
            {
                return false;
            }
        }

        return true;
    }
    bool InCorrectRotation(Quaternion requiredRotation)
    {   
        float angleToTarget = Quaternion.Angle(transform.rotation, requiredRotation);
        float thresholdAngle = 5f; // Adjust this value based on your needs
        return (angleToTarget <= thresholdAngle);
    }
    float DistanceModifier()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float distanceModifier = Mathf.Clamp(distance / 100, 3, detectionRadius / 100);
        
        if(distanceModifier > 3) distanceModifier = -distanceModifier - 1;
        return distanceModifier + 2;

    }
    void LaunchProjectile(Vector3 interceptDirection)
    {
        GameObject projectileInstance = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody spawnedProjectileRB = projectileInstance.GetComponent<Rigidbody>();
        float calculatedLaunchForce = launchForce * spawnedProjectileRB.mass;
        spawnedProjectileRB.AddForce(interceptDirection.normalized * calculatedLaunchForce, ForceMode.Impulse);
        
    }
    void LaunchMultipleProjectile(Vector3 interceptDirection)
    {
        GameObject projectileInstance = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody[] spawnedProjectileRB = projectileInstance.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < spawnedProjectileRB.Length; i++)
        {
            float calculatedLaunchForce = launchForce * spawnedProjectileRB[i].mass;
            spawnedProjectileRB[i].AddForceAtPosition(interceptDirection.normalized * calculatedLaunchForce,transform.position,ForceMode.Impulse);
        }
        
    }
    
    float CalculateTimeToTarget(Vector3 targetPosition)
    {
        // Calculate the distance to the target
        Vector3 distance = targetPosition - projectileSpawnPoint.position;
        float horizontalDistance = new Vector3(distance.x, 0f, distance.z).magnitude;

        // Calculate the time to reach the target
        float time = horizontalDistance / launchForce;

        // Calculate the height difference between the target and the catapult
        float verticalDistance = distance.y;
        float gravity = Physics.gravity.y;

        // Check if the initial velocity is enough to reach the target's height
        if ((2f * (launchForce / gravity) + (2f * verticalDistance / gravity)) >= 0)
        {
            float timeToFall = Mathf.Sqrt(-2f * verticalDistance / gravity);
            float timeToRise = Mathf.Sqrt(2f * (launchForce / gravity) + (2f * verticalDistance / gravity));
            float totalTime = timeToFall + timeToRise;


            // Adjust the time to account for the height difference
            time += totalTime;
        }
        else
        {
           // Debug.Log("Initial velocity is not enough to reach the target's height.");
        }

        return time;
    }
    // private void OnDrawGizmosSelected()      
    // {
    //     Gizmos.color = Color.white;
    //     Gizmos.DrawSphere(transform.position,detectionRadius);    
    // }  
}
