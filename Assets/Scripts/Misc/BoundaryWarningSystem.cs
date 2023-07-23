using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoundaryWarningSystem : MonoBehaviour
{
   [SerializeField] float warningDuration = 5f; // Duration of the warning in seconds
    [SerializeField] float topBoundsHeight = 515f;
    [SerializeField] GameObject deathRay;
    [SerializeField] UnityEvent warningExceededEvents;

    bool isOutOfBounds = false;
    bool isOutOfTopBounds = false;
    bool hasEnteredTrigger = false;
    float warningTimer = 0f;
    GameObject player;
    UIManager uiManager;
    


    void Start() 
    {
        player = FindObjectOfType<SaucerController>().gameObject;
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if(!isOutOfBounds)
        {
            if(player.transform.position.y >= topBoundsHeight && !isOutOfTopBounds)  
            {
                isOutOfTopBounds = true;
                warningTimer = warningDuration;
            }
            else if(player.transform.position.y < topBoundsHeight)
            {
                isOutOfTopBounds = false;
                warningTimer = 0f;
            }   
        }
        
        if (isOutOfBounds || isOutOfTopBounds)
        {
            uiManager.bounds.SetActive(true);
            if (warningTimer <= 0f)
            {
                GameObject tempBeam = Instantiate(deathRay, player.transform.position, Quaternion.identity);
                Destroy(tempBeam, 2f);
                warningExceededEvents.Invoke();
            }
            else
            {
                warningTimer -= Time.deltaTime;
                uiManager.boundsTimer.text = warningTimer.ToString("F2");
            }
        }
        else if(!isOutOfBounds || !isOutOfTopBounds)
        {
            uiManager.bounds.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasEnteredTrigger)
            return;

        if (other.gameObject.CompareTag("Player") && !isOutOfBounds)
        {
            StartOutOfBounds();
            hasEnteredTrigger = true;
        }
        else if (other.gameObject.CompareTag("Player") && isOutOfBounds)
        {
            ResetOutOfBounds();
            hasEnteredTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hasEnteredTrigger)
        {
            hasEnteredTrigger = false;
        }
    }


    void StartOutOfBounds()
    {
        isOutOfBounds = true;
        warningTimer = warningDuration;
        // Show warning UI or perform other visual/audio feedback to the player
    }

    void ResetOutOfBounds()
    {
        isOutOfBounds = false;
        warningTimer = 0f;
        // Hide warning UI or reset any visual/audio feedback
    }
}
