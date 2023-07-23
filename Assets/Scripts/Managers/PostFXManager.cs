using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostFXManager : MonoBehaviour
{
    [SerializeField] Volume closeToDeathVolume;

    SaucerController player;
    Health playerHealth;
    void Start() 
    {
        player = FindObjectOfType<SaucerController>();
        playerHealth = player.GetComponent<Health>();
    }
    void Update() 
    {
        if(playerHealth.IsCloseToDeath())
            UpdateDeathVolume(Mathf.InverseLerp(25f, 0f, playerHealth.GetHealthPrecentage()));
        else
            UpdateDeathVolume(0f);
    }
    void UpdateDeathVolume(float weight)
    {
        closeToDeathVolume.weight = weight;
    }

}
