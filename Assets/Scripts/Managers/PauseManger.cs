using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManger : MonoBehaviour
{
    [SerializeField] KeyCode pauseKey;
    [SerializeField] UnityEvent pauseEvents;
    [SerializeField] UnityEvent unpauseEvents;

    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void UseUnpauseEvents()
    {
        unpauseEvents.Invoke();
        isPaused = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            if(isPaused)
            {
                UseUnpauseEvents();
            }
            else
            {
                pauseEvents.Invoke();
                isPaused = true;
            }
        }
    }
}
