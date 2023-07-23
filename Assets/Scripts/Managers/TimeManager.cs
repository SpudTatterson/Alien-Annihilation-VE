using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
    public void ToggleCursorLock(bool toggle)
    {
        if(!toggle)  Cursor.lockState = CursorLockMode.None;
        if(toggle) Cursor.lockState = CursorLockMode.Locked;
        
    }
}
