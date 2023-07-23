using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 turn;
    [SerializeField] float speed = 5;
    [SerializeField] float smoothTime = 0.1f;

    [SerializeField] float distance;
    [SerializeField]GameObject player;
    [SerializeField] Vector2 cameraMinMaxHeight;
    //[SerializeField] float forward;
    //Vector3 playerEyePosition => player.transform.position + Vector3.up * player.GetComponent<Collider>().bounds.extents.y;
    [SerializeField] float cameraHeightOffset = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        turn.x += Input.GetAxis("Mouse X") * speed;
        turn.y += -Input.GetAxis("Mouse Y") * speed;

        turn.y = Mathf.Clamp(turn.y ,cameraMinMaxHeight.y ,cameraMinMaxHeight.x);

        transform.localEulerAngles = new Vector3(turn.y, turn.x,0);
        
        //Vector3 inFrontOfplayer = player.transform.position + player.transform.forward * forward;
        Vector3 desiredPosition = player.transform.position - transform.forward * distance + Vector3.up * cameraHeightOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothTime);
    }
}
