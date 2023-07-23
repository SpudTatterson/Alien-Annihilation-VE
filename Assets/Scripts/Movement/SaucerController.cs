using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerController : MonoBehaviour
{
    [SerializeField] float speed = 200;
    [SerializeField] float verticalityResponsivness = 14000;
    [SerializeField] Transform GFX;
    [SerializeField] float speedBoostModifier = 1.5f;
    [SerializeField] float speedSlowModifier = 0.5f;

    float yaw;
    float forward;
    float vertical;
    public float throttle;
    bool canPlayerMove = true;
    public float speedBoost;


    Rigidbody rb;
    Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        RotatePlayerGFX();
        // SetRotation();
    }
    void RotatePlayerGFX()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.y));
        Vector3 planePostion = new Vector3(0f, transform.position.y, 0f);
        //- rb.velocity.magnitude * forward
        //planePostion.y = Mathf.Clamp(planePostion.y, transform.position.y - 30f, transform.position.y + 30f);
        Vector3 worldPosition = Vector3.ProjectOnPlane(mouseWorldPosition, Vector3.up) + planePostion;
        //worldPosition += transform.right * yaw * 40;
        transform.LookAt(worldPosition);
    }
    void HandleInputs()
    {
        forward = Input.GetAxis("Forward");
        yaw = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        throttle = Input.GetAxis("Throttle");

        GetSpeedModifier();
    }

    private void GetSpeedModifier()
    {
        float absThrottle = Mathf.Abs(throttle);
        if (throttle > 0)
        {
            speedBoost = speedBoostModifier;
            cam.fieldOfView = Mathf.Lerp(60f, 75f, absThrottle);
        }
        else if (throttle < 0)
        {
            speedBoost = speedSlowModifier;
            cam.fieldOfView = Mathf.Lerp(60f, 50f, absThrottle);
        }
        else if (throttle == 0)
        {
            speedBoost = 1;
            cam.fieldOfView = 60f;
        }
    }

    private void FixedUpdate()
    {
        if (canPlayerMove)
        {
            Vector3 flatForward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
            Vector3 flatRight = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;

            Vector3 movement = flatForward * forward + flatRight * yaw;
            movement.Normalize(); // Normalize the movement vector

            rb.AddForce(movement * speed * 100 * speedBoost);
            rb.AddForce(transform.up * vertical * verticalityResponsivness);
        }
    }

    public void ToggelPlayerMovement(bool canMove)
    {
        canPlayerMove = canMove;
    }
    void SetRotation()
    {
        // Vector3 point = GFX.position + GFX.forward * forward * rb.velocity.magnitude + GFX.right * yaw * rb.velocity.magnitude 
        // - (GFX.up * forward) ;
        // Debug.DrawLine(GFX.position, point, Color.blue);
        // GFX.LookAt(point,Vector3.up);
        float absForward = Mathf.Abs(forward);
        Debug.Log(Mathf.Lerp(0f, 90f, absForward));
        Vector3 rotation = new Vector3(0f, 0f, 0f);
        GFX.Rotate(rotation, Space.Self);
    }

    public float GetVelocity()
    {
        return rb.velocity.magnitude;
    }

}
