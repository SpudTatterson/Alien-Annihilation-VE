using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopCameraController : MonoBehaviour
{


    [SerializeField] Camera shopCamera;

    [SerializeField] Canvas gunCanvas;
    [SerializeField] Vector3 gunCameraPostion;
    [SerializeField] Vector3 gunCameraRotation;

    [SerializeField] Canvas tractionCanvas;
    [SerializeField] Vector3 tractionCameraPostion;
    [SerializeField] Vector3 tractionCameraRotation;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gunCanvas.isActiveAndEnabled)
        {
            shopCamera.transform.localPosition = gunCameraPostion;
            shopCamera.transform.localEulerAngles = gunCameraRotation;
        }
        else if(tractionCanvas.isActiveAndEnabled)
        {
            shopCamera.transform.localPosition = tractionCameraPostion;
            shopCamera.transform.localEulerAngles = tractionCameraRotation;
        }

    }
}
